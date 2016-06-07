using System;
using System.Data;
using System.IO;
using System.Text;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    ///     纯文本类型读取类
    /// </summary>
    public class TxtUtility
    {
        /// <summary>
        ///     读取纯文本到DataTable中
        /// </summary>
        /// <param name="fullpath">文件完整路径</param>
        /// <returns>读取内容数据集</returns>
        public static DataSet ReadTxt(string fullpath)
        {
            return ReadTxt(fullpath, Encoding.Default, true);
        }

        /// <summary>
        ///     读取纯文本到DataTable中
        /// </summary>
        /// <param name="fullpath">文件完整路径</param>
        /// <param name="encoding">字符编码集</param>
        /// <returns>读取内容数据集</returns>
        public static DataSet ReadTxt(string fullpath, Encoding encoding)
        {
            return ReadTxt(fullpath, encoding, true);
        }

        /// <summary>
        ///     读取纯文本到DataTable中
        /// </summary>
        /// <param name="fullpath">文件完整路径</param>
        /// <param name="encoding">字符编码集</param>
        /// <param name="firstRow">首行是否标题行</param>
        /// <returns>读取内容数据集</returns>
        public static DataSet ReadTxt(string fullpath, Encoding encoding, bool firstRow)
        {
            var ds = new DataSet();

            //数据转换是否正常
            var dt = new DataTable();
            dt.Columns.AddRange(new[]
            {
                new DataColumn("result", typeof (bool)),
                new DataColumn("errorMsg", typeof (string))
            });
            var dr = dt.NewRow();
            dr["result"] = true;
            dr["errorMsg"] = string.Empty;

            //数据转换结果
            var dtContent = new DataTable();

            try
            {
                if (File.Exists(fullpath))
                {
                    using (var sr = new StreamReader(fullpath, encoding))
                    {
                        string line;
                        while (!string.IsNullOrWhiteSpace(line = sr.ReadLine()))
                        {
                            if (firstRow)
                            {
                                AddColumns(dtContent, line);

                                firstRow = false;
                            }
                            else
                            {
                                AddRows(dtContent, line);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dr["result"] = false;
                dr["errorMsg"] = ex.GetBaseException().Message;
            }

            dt.Rows.Add(dr);

            ds.Tables.Add(dt);
            ds.Tables.Add(dtContent);

            return ds;
        }

        /// <summary>
        ///     新增数据列
        /// </summary>
        /// <param name="dt">数据集对象</param>
        /// <param name="content">数据列</param>
        private static void AddColumns(DataTable dt, string content)
        {
            var columns = content.Split(',');

            if (columns.Length > 0)
            {
                foreach (var column in columns)
                {
                    if (!dt.Columns.Contains(column))
                    {
                        dt.Columns.Add(column);
                    }
                }
            }
        }

        /// <summary>
        ///     新增数据行
        /// </summary>
        /// <param name="dt">数据集对象</param>
        /// <param name="content">数据行</param>
        private static void AddRows(DataTable dt, string content)
        {
            var dr = dt.NewRow();
            var cells = content.Split(',');

            if (cells.Length > 0)
            {
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = cells.Length > i ? cells[i] : string.Empty;
                }
            }

            dt.Rows.Add(dr);
        }
    }
}