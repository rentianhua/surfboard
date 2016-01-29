using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace Cedar.Framework.Common.BaseClasses
{
    /// <summary>
    ///     Excel辅助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        ///     保存路径
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        ///     生成Excel
        /// </summary>
        /// <param name="json">数据</param>
        /// <param name="addheader">是否添加列头</param>
        /// <param name="sheetName">sheet名称</param>
        public void WriteExcel(string json, bool addheader = true, string sheetName = "")
        {
            if (!json.StartsWith("["))
            {
                json = "[" + json;
            }
            if (!json.EndsWith("]"))
            {
                json += "]";
            }
            var ds = new DataSet();
            var dt = new DataTable();
            try
            {
                dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (!string.IsNullOrWhiteSpace(sheetName))
                {
                    dt.TableName = sheetName;
                }
            }
            catch
            {
                dt.Clear();
            }
            ds.Tables.Add(dt);
            WriteExcel(ds, addheader);
        }

        /// <summary>
        ///     Json保存成Excel
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <param name="title">表头</param>
        /// <param name="addheader">是否添加列头</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="addPic">是否添加图片</param>
        /// <param name="alignJson">列对齐方式</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="alignName">Name of the align.</param>
        public void WriteExcel(string json, string title, bool addheader, string extInfo = "", bool addPic = false,
            string alignJson = "", string sheetName = "table1", string alignName = "table2")
        {
            #region 格式化Json字符

            //数据
            if (!string.IsNullOrWhiteSpace(json))
            {
                if (!json.StartsWith("["))
                {
                    json = "[" + json;
                }
                if (!json.EndsWith("]"))
                {
                    json += "]";
                }
                json = "\"" + sheetName + "\":" + json;
            }
            //列对齐方式
            if (!string.IsNullOrWhiteSpace(alignJson))
            {
                if (!alignJson.StartsWith("["))
                {
                    alignJson = "[" + alignJson;
                }
                if (!alignJson.EndsWith("]"))
                {
                    alignJson += "]";
                }
                alignJson = ",\"" + alignName + "\":" + alignJson;
            }

            #endregion

            var formatJson = "{" + json + alignJson + "}";

            var ds = new DataSet();
            try
            {
                ds = JsonConvert.DeserializeObject<DataSet>(formatJson);
            }
            catch (Exception ex)
            {
                ds.Clear();
            }
            try
            {
                WriteExcel(ds, title, addheader, extInfo, addPic);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="set">数据源</param>
        /// <param name="addheader">是否添加列头</param>
        public void WriteExcel(DataSet set, bool addheader)
        {
            if (!string.IsNullOrWhiteSpace(SavePath))
            {
                var fi = new FileInfo(SavePath);
                //文件夹不存在则创建
                if (fi.DirectoryName != null && !Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                //去掉文件夹只读、隐藏属性
                if (fi.Directory != null)
                {
                    fi.Directory.Attributes &= ~FileAttributes.ReadOnly;
                    fi.Directory.Attributes &= ~FileAttributes.Hidden;
                }

                //文件存在则删除重新创建
                if (fi.Exists)
                {
                    System.IO.File.Delete(SavePath);
                    fi = new FileInfo(SavePath);
                }

                using (var package = new ExcelPackage(fi))
                {
                    if (set != null)
                    {
                        //第一个table为数据
                        var table = set.Tables[0];

                        if (table != null)
                        {
                            //添加sheet到workbook
                            var worksheet = AddSheet(package, table.TableName);
                            worksheet.Cells["A1"].LoadFromDataTable(table, addheader);

                            var rowCount = table.Rows.Count;
                            var dateColumns = from DataColumn d in table.Columns
                                where d.DataType == typeof (DateTime) || d.ColumnName.Contains("Date")
                                select d.Ordinal + 1;

                            var fromRow = addheader ? 2 : 1;
                            var toRow = fromRow + rowCount;

                            foreach (var dc in dateColumns)
                            {
                                worksheet.Cells[fromRow, dc, toRow, dc].Style.Numberformat.Format =
                                    "yyyy-MM-dd HH:mm:ss";
                            }
                        }
                    }
                    package.Save();
                }
                try
                {
                    //去掉文件只读、隐藏属性
                    fi.Attributes &= ~FileAttributes.ReadOnly;
                    fi.Attributes &= ~FileAttributes.Hidden;
                    ////LogUtility.Warn("设置文件权限，路径为：" + fi.FullName);
                }
                catch (Exception ex)
                {
                    ////LogUtility.Error("设置文件权限发生异常，路径为：" + fi.FullName + ",异常信息：" + ex.Message);
                }
            }
        }

        /// <summary>
        ///     写入Excel
        /// </summary>
        /// <param name="set">数据源</param>
        /// <param name="title">表头</param>
        /// <param name="addheader">是否需要添加标题</param>
        /// <param name="extInfo">信息</param>
        /// <param name="addPic">是否添加图片</param>
        public void WriteExcel(DataSet set, string title, bool addheader, string extInfo = "", bool addPic = false)
        {
            if (!string.IsNullOrWhiteSpace(SavePath))
            {
                var fi = new FileInfo(SavePath);
                //文件夹不存在则创建
                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                try
                {
                    //去掉文件夹只读、隐藏属性
                    if (fi.Directory != null)
                    {
                        fi.Directory.Attributes &= ~FileAttributes.ReadOnly;
                        fi.Directory.Attributes &= ~FileAttributes.Hidden;
                    }
                    ////LogUtility.Warn("设置文件夹权限，路径：" + fi.DirectoryName);
                }
                catch (Exception ex)
                {
                    ////LogUtility.Error("设置文件夹权限发生异常，路径：" + fi.DirectoryName + ",异常信息：" + ex.Message);
                }
                try
                {
                    //文件存在则删除重新创建
                    if (fi.Exists)
                    {
                        System.IO.File.Delete(SavePath);
                        fi = new FileInfo(SavePath);
                    }
                }
                catch (Exception ex)
                {
                    ////LogUtility.Error("生成文件发生异常，文件路径：" + fi.FullName + ",异常信息：" + ex.Message);
                }

                try
                {
                    using (var package = new ExcelPackage(fi))
                    {
                        if (set != null && set.Tables.Count > 0)
                        {
                            for (var i = 0; i < set.Tables.Count; i++)
                            {
                                var table = set.Tables[i];
                                if (table != null)
                                {
                                    #region 生成Excel

                                    //添加sheet到workbook
                                    var worksheet = AddSheet(package, table.TableName);
                                    //无网格线
                                    worksheet.View.ShowGridLines = false;
                                    //第一列宽度为1
                                    worksheet.Column(1).Width = 2;

                                    if (addPic)
                                    {
                                        AddPic(worksheet);
                                    }

                                    if (string.IsNullOrWhiteSpace(table.TableName))
                                    {
                                        AddTitle(worksheet, title);
                                    }
                                    else
                                    {
                                        AddTitle(worksheet, table.TableName);
                                    }

                                    AddExtInfo(worksheet, extInfo);
                                    worksheet.Cells["B5"].LoadFromDataTable(table, addheader);

                                    var rowCount = table.Rows.Count;
                                    var fromCol = 2;
                                    var toCol = 1 + table.Columns.Count;
                                    var fromRow = addheader ? 6 : 5;
                                    var toRow = fromRow - 1 + rowCount;
                                    //是否添加表头
                                    if (addheader)
                                    {
                                        worksheet.Cells[5, fromCol, 5, toCol].Style.Fill.PatternType =
                                            ExcelFillStyle.Solid;
                                        worksheet.Cells[5, fromCol, 5, toCol].Style.Fill.BackgroundColor.SetColor(
                                            Color.Gray);
                                        worksheet.Cells[5, fromCol, 5, toCol].Style.Font.Color.SetColor(Color.WhiteSmoke);
                                        worksheet.Cells[5, fromCol, 5, toCol].Style.Font.Bold = true;
                                        worksheet.Cells[5, fromCol, 5, toCol].Style.HorizontalAlignment =
                                            ExcelHorizontalAlignment.Center;
                                        worksheet.Cells[5, fromCol, 5, toCol].Style.VerticalAlignment =
                                            ExcelVerticalAlignment.Center;
                                        worksheet.Row(5).Height = 26.25;
                                    }
                                    //列表数据
                                    worksheet.Cells[5, fromCol, toRow, toCol].Style.Border.Left.Style =
                                        ExcelBorderStyle.Thin;
                                    worksheet.Cells[5, fromCol, toRow, toCol].Style.Border.Top.Style =
                                        ExcelBorderStyle.Thin;
                                    worksheet.Cells[5, fromCol, toRow, toCol].Style.Border.Right.Style =
                                        ExcelBorderStyle.Thin;
                                    worksheet.Cells[5, fromCol, toRow, toCol].Style.Border.Bottom.Style =
                                        ExcelBorderStyle.Thin;
                                    worksheet.Cells[5, fromCol, toRow, toCol].Style.Font.Name = "Arial";

                                    //设置对齐方式
                                    if (set.Tables.Count > 1)
                                    {
                                        var dtAlign = set.Tables[1];
                                        if (dtAlign != null && dtAlign.Rows.Count > 0)
                                        {
                                            for (var j = 0; j < dtAlign.Columns.Count; j++)
                                            {
                                                var colNo = j + fromCol;
                                                var align = dtAlign.Rows[0][j].ToString().ToLower();
                                                switch (align)
                                                {
                                                    case "center":
                                                        worksheet.Cells[fromRow, colNo, toRow, colNo].Style
                                                            .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                        break;
                                                    case "right":
                                                        worksheet.Cells[fromRow, colNo, toRow, colNo].Style
                                                            .HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                        break;
                                                    default:
                                                        worksheet.Cells[fromRow, colNo, toRow, colNo].Style
                                                            .HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                                        break;
                                                }
                                            }
                                        }
                                    }

                                    //自适应列宽
                                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                                    #endregion
                                }
                            }
                        }
                        package.Save();
                    }
                }
                catch (Exception ex)
                {
                    ////LogUtility.Error("生成Excel文件发生异常，文件路径：" + fi.FullName + ",异常信息：" + ex.Message);
                }

                try
                {
                    //去掉文件只读、隐藏属性
                    fi.Attributes &= ~FileAttributes.ReadOnly;
                    fi.Attributes &= ~FileAttributes.Hidden;
                    ////LogUtility.Warn("设置文件权限，路径为：" + fi.FullName);
                }
                catch (Exception ex)
                {
                    //LogUtility.Error("设置文件权限发生异常，路径为：" + fi.FullName + ",异常信息：" + ex.Message);
                }
            }
        }

        /// <summary>
        ///     Json保存成Excel
        /// </summary>
        /// <param name="datalocation">datalocation(String)</param>
        /// <param name="json">Json字符串</param>
        /// <param name="title">表头</param>
        /// <param name="headerJson">headerJson(String)</param>
        /// <param name="extInfo">扩展信息</param>
        /// <param name="addPic">是否添加图片</param>
        /// <param name="alignJson">列对齐方式</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="alignName">Name of the align.</param>
        public void WriteExcel(string datalocation, string json, string title, string headerJson, string extInfo = "",
            bool addPic = false, string alignJson = "", string sheetName = "table1", string alignName = "table2")
        {
            #region 格式化Json字符

            //数据
            if (!string.IsNullOrWhiteSpace(json))
            {
                if (!json.StartsWith("["))
                {
                    json = "[" + json;
                }
                if (!json.EndsWith("]"))
                {
                    json += "]";
                }
                json = "\"" + sheetName + "\":" + json;
            }
            //列对齐方式
            if (!string.IsNullOrWhiteSpace(alignJson))
            {
                if (!alignJson.StartsWith("["))
                {
                    alignJson = "[" + alignJson;
                }
                if (!alignJson.EndsWith("]"))
                {
                    alignJson += "]";
                }
                alignJson = ",\"" + alignName + "\":" + alignJson;
            }

            #endregion

            var formatJson = "{" + json + alignJson + "}";

            var ds = new DataSet();
            try
            {
                ds = JsonConvert.DeserializeObject<DataSet>(formatJson);
            }
            catch (Exception ex)
            {
                ds.Clear();
                //LogUtility.Error("序列化导出数据失败，异常信息：" + ex.Message);
            }
            try
            {
                WriteSpecialExcel(datalocation, ds, title, headerJson, extInfo, addPic);
            }
            catch (Exception ex)
            {
                //LogUtility.Error("生成Excel失败，异常信息：" + ex.Message);
            }
        }

        /// <summary>
        ///     写入Excel
        /// </summary>
        /// <param name="datalocation">数据加载位置</param>
        /// <param name="set">数据源</param>
        /// <param name="title">表头</param>
        /// <param name="headerJson">headerJson(String)</param>
        /// <param name="extInfo">信息</param>
        /// <param name="addPic">是否添加图片</param>
        public void WriteSpecialExcel(string datalocation, DataSet set, string title, string headerJson,
            string extInfo = "", bool addPic = false)
        {
            if (string.IsNullOrWhiteSpace(SavePath)) return;
            var lst = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(headerJson);

            var fi = new FileInfo(SavePath);
            //文件夹不存在则创建
            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }
            try
            {
                //去掉文件夹只读、隐藏属性
                if (fi.Directory != null)
                {
                    fi.Directory.Attributes &= ~FileAttributes.ReadOnly;
                    fi.Directory.Attributes &= ~FileAttributes.Hidden;
                }
                //LogUtility.Warn("设置文件夹权限，路径：" + fi.DirectoryName);
            }
            catch (Exception ex)
            {
                //LogUtility.Error("设置文件夹权限发生异常，路径：" + fi.DirectoryName + ",异常信息：" + ex.Message);
            }
            try
            {
                //文件存在则删除重新创建
                if (fi.Exists)
                {
                    System.IO.File.Delete(SavePath);
                    fi = new FileInfo(SavePath);
                }
            }
            catch (Exception ex)
            {
                //LogUtility.Error("生成文件发生异常，文件路径：" + fi.FullName + ",异常信息：" + ex.Message);
            }

            try
            {
                using (var package = new ExcelPackage(fi))
                {
                    if (set != null && set.Tables.Count > 0)
                    {
                        var table = set.Tables[0];

                        if (table != null)
                        {
                            #region 生成Excel

                            //添加sheet到workbook
                            var worksheet = AddSheet(package, table.TableName);
                            //无网格线
                            worksheet.View.ShowGridLines = false;
                            //第一列宽度为1
                            worksheet.Column(1).Width = 2;

                            if (addPic)
                            {
                                AddPic(worksheet);
                            }
                            AddTitle(worksheet, title);
                            AddExtInfo(worksheet, extInfo);

                            for (var i = worksheet.Dimension.Start.Column; i < worksheet.Dimension.End.Column; i++)
                            {
                                worksheet.Column(i).AutoFit(1);
                            }

                            if (lst != null)
                            {
                                foreach (var obj in lst)
                                {
                                    worksheet.Cells[obj.Key].Merge = true;
                                    worksheet.Cells[obj.Key].Value = obj.Value;
                                }
                            }

                            var toRow = worksheet.Cells["B5:" + datalocation].End.Row - 1;
                            worksheet.Cells[datalocation].LoadFromDataTable(table, false);

                            var rowCount = table.Rows.Count;
                            const int fromCol = 2;
                            var toCol = 1 + table.Columns.Count;

                            worksheet.Cells[5, fromCol, toRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[5, fromCol, toRow, toCol].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                            worksheet.Cells[5, fromCol, toRow, toCol].Style.Font.Color.SetColor(Color.WhiteSmoke);
                            worksheet.Cells[5, fromCol, toRow, toCol].Style.Font.Bold = true;
                            worksheet.Cells[5, fromCol, toRow, toCol].Style.HorizontalAlignment =
                                ExcelHorizontalAlignment.Center;
                            worksheet.Cells[5, fromCol, toRow, toCol].Style.VerticalAlignment =
                                ExcelVerticalAlignment.Center;
                            worksheet.Cells[5, fromCol, toRow, toCol].AutoFitColumns(20d);
                            worksheet.Row(toRow).Height = 26.25;

                            //列表数据
                            worksheet.Cells[5, fromCol, toRow + rowCount, toCol].Style.Border.Left.Style =
                                ExcelBorderStyle.Thin;
                            worksheet.Cells[5, fromCol, toRow + rowCount, toCol].Style.Border.Top.Style =
                                ExcelBorderStyle.Thin;
                            worksheet.Cells[5, fromCol, toRow + rowCount, toCol].Style.Border.Right.Style =
                                ExcelBorderStyle.Thin;
                            worksheet.Cells[5, fromCol, toRow + rowCount, toCol].Style.Border.Bottom.Style =
                                ExcelBorderStyle.Thin;
                            worksheet.Cells[5, fromCol, toRow + rowCount, toCol].Style.Font.Name = "Arial";

                            //设置对齐方式
                            if (set.Tables.Count > 1)
                            {
                                var dtAlign = set.Tables[1];
                                if (dtAlign != null && dtAlign.Rows.Count > 0)
                                {
                                    for (var j = 0; j < dtAlign.Columns.Count; j++)
                                    {
                                        var colNo = j + fromCol;
                                        var align = dtAlign.Rows[0][j].ToString().ToLower();
                                        switch (align)
                                        {
                                            case "center":
                                                worksheet.Cells[toRow + 1, colNo, toRow + rowCount, colNo].Style
                                                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                break;
                                            case "right":
                                                worksheet.Cells[toRow + 1, colNo, toRow + rowCount, colNo].Style
                                                    .HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                break;
                                            default:
                                                worksheet.Cells[toRow + 1, colNo, toRow + rowCount, colNo].Style
                                                    .HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                                break;
                                        }
                                    }
                                }
                            }

                            //自适应列宽
                            worksheet.Cells[
                                worksheet.Dimension.Start.Row, worksheet.Dimension.Start.Column,
                                worksheet.Dimension.End.Row, worksheet.Dimension.Start.Column].AutoFitColumns();

                            #endregion
                        }
                    }
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                //LogUtility.Error("生成Excel文件发生异常，文件路径：" + fi.FullName + ",异常信息：" + ex.Message);
            }

            try
            {
                //去掉文件只读、隐藏属性
                fi.Attributes &= ~FileAttributes.ReadOnly;
                fi.Attributes &= ~FileAttributes.Hidden;
                //LogUtility.Warn("设置文件权限，路径为：" + fi.FullName);
            }
            catch (Exception ex)
            {
                //LogUtility.Error("设置文件权限发生异常，路径为：" + fi.FullName + ",异常信息：" + ex.Message);
            }
        }

        /// <summary>
        ///     将Excel转成DataSet
        /// </summary>
        /// <returns></returns>
        public static DataSet ReadExcelToDataSet(string excelPath)
        {
            return ReadExcelToDataSet(excelPath, true);
        }

        /// <summary>
        ///     将Excel转成DataSet
        /// </summary>
        /// <param name="excelPath">excelPath(String)</param>
        /// <param name="existheader">是否有表头</param>
        /// <param name="top">取多少行</param>
        /// <returns>
        ///     数据集
        /// </returns>
        public static DataSet ReadExcelToDataSet(string excelPath, bool existheader, int? top = null)
        {
            var ds = new DataSet();

            if (System.IO.File.Exists(excelPath))
            {
                //后缀名(.xls或.xlsx)
                var extension = Path.GetExtension(excelPath);
                if (extension != null)
                {
                    var ext = extension.ToLower().Trim();
                    if (ext.Equals(".xlsx"))
                    {
                        ds = ReadXlsx(excelPath, existheader, top);
                    }
                    //else if (ext.Equals(".xls"))
                    //{
                    //    ds = ReadXls(excelPath, existheader, top);
                    //}
                }
            }

            return ds;
        }

        /// <summary>
        ///     添加Sheet
        /// </summary>
        /// <param name="package">Excel</param>
        /// <param name="name">Sheet名称</param>
        /// <returns>ExcelSheet</returns>
        private static ExcelWorksheet AddSheet(ExcelPackage package, string name)
        {
            return package.Workbook.Worksheets.Add(name);
        }

        /// <summary>
        ///     根据sheet名称生成DataTable
        /// </summary>
        /// <param name="worksheet">ExcelSheet</param>
        /// <returns>DataTable</returns>
        private static DataTable AddDataTable(ExcelWorksheet worksheet)
        {
            return new DataTable(worksheet.Name);
        }

        /// <summary>
        ///     添加数据行
        /// </summary>
        /// <param name="worksheet">ExcelSheet</param>
        /// <param name="dt">数据源</param>
        /// <param name="existheader">是否存在标题</param>
        /// <param name="top">top(Int32})</param>
        private static void AddRow(ExcelWorksheet worksheet, DataTable dt, bool existheader, int? top)
        {
            var colCount = worksheet.Dimension.End.Column;
            var rowCount = worksheet.Dimension.End.Row;
            var firstRow = 1;

            if (top.HasValue)
            {
                rowCount = rowCount > top.Value ? top.Value : rowCount;
            }

            if (existheader)
            {
                for (var i = 1; i <= colCount; i++)
                {
                    var colName = (string) worksheet.Cells[1, i].Value;
                    if (!string.IsNullOrWhiteSpace(colName))
                    {
                        dt.Columns.Add(new DataColumn(colName));
                    }
                }
                firstRow++;
            }

            //获取实际的列数
            colCount = dt.Columns.Count;

            for (var i = firstRow; i <= rowCount; i++)
            {
                var row = dt.NewRow();
                for (var j = 1; j <= colCount; j++)
                {
                    //判断单元格格式是否是日期型
                    if (worksheet.Cells[i, j].Style.Numberformat.Format.Contains("hh:mm:ss"))
                    {
                        var dtValue = worksheet.Cells[i, j].GetValue<DateTime>();
                        row[j - 1] = dtValue.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        var strValue = worksheet.Cells[i, j].GetValue<string>() ?? string.Empty;
                        row[j - 1] = strValue.Trim();
                    }
                }
                dt.Rows.Add(row);
            }
        }

        /// <summary>
        ///     读取Excel2007
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="header">是否有表头</param>
        /// <param name="top">取多少行</param>
        /// <returns>数据集</returns>
        private static DataSet ReadXlsx(string path, bool header, int? top)
        {
            var ds = new DataSet();
            var existingFile = new FileInfo(path);
            if (existingFile.Exists)
            {
                using (var package = new ExcelPackage(existingFile))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        var dt = AddDataTable(worksheet);
                        AddRow(worksheet, dt, header, top);
                        ds.Tables.Add(dt);
                        //只读第一个sheet，有新需求之后再做调整
                        break;
                    }
                }
                return ds;
            }
            return null;
        }
        
        /// <summary>
        ///     添加图片到Excel
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        private static void AddPic(ExcelWorksheet worksheet)
        {
            //var model = XmlHelper.GetParameterBykey("PictureReportURL");
            //var picPath = string.Empty;
            //if (model != null)
            //{
            //    picPath = AppDomain.CurrentDomain.BaseDirectory + model.Value;
            //}
            //var img = new Bitmap(picPath);
            //var pic = worksheet.Drawings.AddPicture("SmartacRewards", img);
            //pic.SetPosition(0, 5, 1, 10);
            //worksheet.Cells["B1:F1"].Merge = true;
            //worksheet.Cells["B1"].Value = "SmartRewards";
            //worksheet.Cells["B1"].Style.Font.Bold = true;
            //worksheet.Cells["B1"].Style.Font.Italic = true;
            //worksheet.Cells["B1"].Style.Font.Name = "Arial";
            //worksheet.Cells["B1"].Style.Font.Size = 18;
            //worksheet.Row(1).Height = img.Height - 12;
            //worksheet.Cells["B1"].Style.Indent = 8;
            //worksheet.Cells["B1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        /// <summary>
        ///     添加表头
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="title">标题</param>
        private static void AddTitle(ExcelWorksheet worksheet, string title)
        {
            worksheet.Cells["B2:F2"].Merge = true;
            worksheet.Cells["B2"].Value = title;
            worksheet.Cells["B2"].Style.Font.Bold = true;
            worksheet.Cells["B2"].Style.Font.Name = "宋体";
            worksheet.Cells["B2"].Style.Font.Size = 14;
            worksheet.Row(2).Height = 19;
            worksheet.Cells["B2"].Style.Indent = 8;
            worksheet.Cells["B2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        /// <summary>
        ///     添加时间
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="info">信息</param>
        private static void AddExtInfo(ExcelWorksheet worksheet, string info)
        {
            if (string.IsNullOrWhiteSpace(info))
            {
                info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            //自适应高度
            var result = info.Split(new[] {"\r\n"}, StringSplitOptions.None);
            var count = result.Count();
            worksheet.Cells["B3"].Value = info;
            worksheet.Cells["B3"].Style.Font.Bold = true;
            worksheet.Cells["B3"].Style.Font.Name = "宋体";
            worksheet.Cells["B3"].Style.Font.Size = 10;
            worksheet.Cells["B3"].Style.Indent = 8;
            worksheet.Cells["B3"].Style.WrapText = true;
            worksheet.Row(3).Height = worksheet.Row(3).Height*count;
            worksheet.Cells["B3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["B3:F3"].Merge = true;
        }

        ///// <summary>
        /////     生成图文excel报表
        ///// </summary>
        ///// <param name="query">query(CommonReportExportModel)</param>
        ///// <param name="dt">dt(DataTable)</param>
        ///// <param name="valueJson">valueJson(String)</param>
        ///// <returns>
        /////     String
        ///// </returns>
        ///// { Created At Time:[ 2014/5/6 10:32], By User:admin, On Machine:APP-DEV-YANLI }
        //public string WriteExcelReport(CommonReportExportModel query, DataTable dt, string valueJson = "")
        //{
        //    //string accountid = GetSocialAccountBySocialType(query.Tennid, 1);
        //    //string title = query.starttime;           
        //    //指定列明
        //    var title = query.chartMainTitle;
        //    var fileName = title;
        //    var sheetname = title;
        //    const string colstr = "ABCDE";
        //    dt.Columns[0].ColumnName = query.xTitle;
        //    if (query.columnDesc.Length >= 1)
        //    {
        //        dt.Columns[1].ColumnName = query.columnDesc[0];
        //    }
        //    if (query.columnDesc.Length >= 2)
        //    {
        //        dt.Columns[2].ColumnName = query.columnDesc[1];
        //    }
        //    if (query.columnDesc.Length >= 3)
        //    {
        //        dt.Columns[3].ColumnName = query.columnDesc[2];
        //    }
        //    //移除无用列

        //    for (var i = dt.Columns.Count - 1; i > query.columnDesc.Length; i--)
        //    {
        //        dt.Columns.RemoveAt(i);
        //    }

        //    #region + 生成Excel文件

        //    var files = new FileInfo(SavePath);
        //    // 如果文件夹不存在则创建
        //    if (!Directory.Exists(files.DirectoryName))
        //    {
        //        Directory.CreateDirectory(files.DirectoryName);
        //    }
        //    // 如果文件已存在，则删除后重新创建
        //    if (files.Exists)
        //    {
        //        files.Delete();
        //        files = new FileInfo(SavePath);
        //    }

        //    #endregion

        //    #region + 生成数据

        //    // 生成Excel
        //    using (var packpage = new ExcelPackage(files))
        //    {
        //        var fillColor = new ColorConverter();
        //        // 设置sheet名称
        //        var sheet = packpage.Workbook.Worksheets.Add(sheetname);
        //        sheet.Row(1).Height = 30;
        //        sheet.Column(1).Width = 1;
        //        // 上传图片（logo）
        //        AddPic(sheet);
        //        // 加载数据
        //        sheet.Cells["B7"].LoadFromDataTable(dt, true);

        //        #region 绘制Excel标题

        //        //设置Title
        //        sheet.Cells["B2:F2"].Merge = true;
        //        sheet.Cells["B2"].Value = sheetname;
        //        sheet.Cells["B2"].Style.Font.Bold = true;
        //        sheet.Cells["B2"].Style.Font.Size = 14;
        //        sheet.Cells["B2"].Style.Font.Name = "宋体";
        //        sheet.Cells["B2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        var extinfo = "";
        //        if (query.StarTime.ToString("yyyy-MM-dd") != "0001-01-01")
        //        {
        //            extinfo = query.StarTime.ToString("yyyy-MM-dd");
        //        }
        //        if (query.EndTime.ToString("yyyy-MM-dd") != "0001-01-01")
        //        {
        //            if (extinfo != "")
        //            {
        //                extinfo += " to ";
        //            }
        //            extinfo += query.EndTime.ToString("yyyy-MM-dd");
        //        }
        //        if (extinfo != "")
        //        {
        //            //// 数据时间区间
        //            sheet.Cells["B4:F4"].Merge = true;
        //            sheet.Cells["B4"].Value = extinfo;
        //            sheet.Cells["B4"].Style.Font.Size = 10;
        //            sheet.Cells["b4"].Style.Font.Name = "Arial";
        //            sheet.Cells["B4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }


        //        //求合计
        //        sheet.Cells["B" + (dt.Rows.Count + 8)].Value = "Total";
        //        for (var i = 0; i < query.columnDesc.Length; i++)
        //        {
        //            var sum = colstr[i + 2].ToString() + (dt.Rows.Count + 8);
        //            var name = sheet.Names.Add("SubTotalName" + i, sheet.Cells[sum]);
        //            if (dt.Columns[i + 1].DataType == typeof (decimal) || dt.Columns[i + 1].DataType == typeof (int) ||
        //                dt.Columns[i + 1].DataType == typeof (double)
        //                || dt.Columns[i + 1].DataType == typeof (float))
        //            {
        //                name.Style.Font.Name = "Calibri";
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //name.Formula = "SUBTOTAL("+(9+i).ToString()+"," + colstr[i+2].ToString() + "8:"+ colstr[i+2].ToString()+ (dt.Rows.Count + 7) + ")";
        //                    name.Formula = "SUM(" + colstr[i + 2] + "8:" + colstr[i + 2] + (dt.Rows.Count + 7) + ")";
        //                }
        //                else
        //                {
        //                    name.Value = 0;
        //                }
        //            }
        //            else
        //            {
        //                name.Value = "-";
        //            }
        //        }

        //        var cellstitle = sheet.Cells[7, 2, 7, 1 + dt.Columns.Count];
        //        //var cellstext = sheet.Cells[8, 2, 7, 2 +dt.Columns.Count];
        //        var cellsall = sheet.Cells[7, 2, 8 + dt.Rows.Count, 1 + dt.Columns.Count];

        //        //设置网格                    
        //        SetWorksheetBorder(fillColor, cellsall);
        //        //设置表格背景色
        //        cellstitle.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        var convertFromString = fillColor.ConvertFromString("#9b9b9b");
        //        if (convertFromString != null)
        //            cellstitle.Style.Fill.BackgroundColor.SetColor((Color) convertFromString);
        //        cellstitle.Style.Font.Color.SetColor(Color.White);

        //        //设置列的自适应
        //        cellsall.AutoFitColumns();
        //        cellsall.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        // 第二个表格背景设置
        //        //int count = drList.Rows.Count + dtFansCount.Rows.Count + 6;
        //        //sheet.Cells["B" + count + ":E" + count].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        //sheet.Cells["B" + count + ":E" + count].Style.Fill.BackgroundColor.SetColor((Color)fillColor.ConvertFromString("#9b9b9b"));
        //        //sheet.Cells["B" + count + ":E" + count].Style.Font.Color.SetColor(System.Drawing.Color.White);

        //        #endregion

        //        #region 生成图表

        //        if (query.chartType.ToLower() == "pie") //饼图
        //        {
        //            #region 饼图

        //            var pieChart = sheet.Drawings.AddChart("pie", eChartType.Pie) as ExcelPieChart;
        //            if (pieChart != null)
        //            {
        //                pieChart.SetPosition(6, 5, 7, 0);
        //                pieChart.SetSize(650, 500);

        //                if (dt.Rows.Count > 0)
        //                {
        //                    ExcelRangeBase pieX = sheet.Cells[8, 2, dt.Rows.Count + 7, 2];
        //                    ExcelRangeBase pieY = sheet.Cells[8, 3, dt.Rows.Count + 7, 3];
        //                    var engagement = pieChart.Series.Add(pieY, pieX);
        //                    engagement.Header = "Members";
        //                }

        //                pieChart.DataLabel.ShowCategory = true;
        //                pieChart.DataLabel.ShowPercent = true;
        //                pieChart.DataLabel.ShowLeaderLines = true;
        //                pieChart.DataLabel.ShowBubbleSize = true;
        //                pieChart.DataLabel.ShowValue = true;
        //                pieChart.DataLabel.NumberFormat = "0%";

        //                pieChart.Legend.Border.LineStyle = eLineStyle.Solid;
        //                pieChart.Legend.Border.Fill.Style = eFillStyle.SolidFill;
        //                pieChart.Legend.Border.Fill.Color = Color.DarkBlue;
        //                pieChart.Title.Text = query.chartMainTitle;
        //            }
        //            //pieChart.DataLabel
        //            //var doc = pieChart.ChartXml;
        //            //var node = doc.SelectSingleNode("//c:dLbls");
        //            //var snode = doc.CreateNode(XmlNodeType.Element, "c:numFmt", null);
        //            //var attrformat = doc.CreateAttribute("formatCode");
        //            //attrformat.InnerText = "0.00%";
        //            //snode.Attributes.Append(attrformat);
        //            //node.InsertAfter(snode, null);

        //            #endregion
        //        }
        //        else
        //        {
        //            #region 线形图或柱状图

        //            ExcelChart chartLine;
        //            if (query.chartType.ToLower() == "column")
        //            {
        //                chartLine = sheet.Drawings.AddChart("column", eChartType.ColumnClustered);
        //            }
        //            else
        //            {
        //                chartLine = sheet.Drawings.AddChart("LineChart", eChartType.Line);
        //            }
        //            chartLine.SetPosition(5, 8, 7, 0);
        //            chartLine.SetSize(800, 400);
        //            chartLine.Title.Text = query.chartMainTitle;
        //            if (dt.Rows.Count > 0)
        //            {
        //                // X轴
        //                ExcelRangeBase lineX = sheet.Cells[8, 2, dt.Rows.Count + 7, 2];
        //                lineX.Style.Numberformat.Format = "yy-MM-dd"; //NumberFormatLocal = @"yyyy-mm-dd"    
        //                if (query.reportCols != null && query.reportCols.Length > 0)
        //                {
        //                    foreach (var col in query.reportCols)
        //                    {
        //                        // Y轴
        //                        ExcelRangeBase lineY = sheet.Cells[8, 3 + col, dt.Rows.Count + 7, 3 + col];
        //                        var content = chartLine.Series.Add(lineY, lineX);
        //                        content.Header = query.columnDesc[col];
        //                    }
        //                }
        //                else
        //                {
        //                    for (var i = 0; i < query.columnDesc.Length; i++)
        //                    {
        //                        // Y轴
        //                        ExcelRangeBase lineY = sheet.Cells[8, 3 + i, dt.Rows.Count + 7, 3 + i];
        //                        var content = chartLine.Series.Add(lineY, lineX);
        //                        content.Header = query.columnDesc[i];
        //                    }
        //                }
        //            }
        //            else //无数据
        //            {
        //                // X轴
        //                ExcelRangeBase lineX = sheet.Cells[12, 2, 12, 2];
        //                if (query.reportCols != null && query.reportCols.Length > 0)
        //                {
        //                    foreach (var col in query.reportCols)
        //                    {
        //                        // Y轴
        //                        ExcelRangeBase lineY = sheet.Cells[8, 3 + col, 12, 3 + col];
        //                        var content = chartLine.Series.Add(lineY, lineX);
        //                        content.Header = query.columnDesc[col];
        //                    }
        //                }
        //                else
        //                {
        //                    // Y轴                       
        //                    for (var i = 0; i < query.columnDesc.Length; i++)
        //                    {
        //                        // Y轴
        //                        ExcelRangeBase lineY = sheet.Cells[8, 3 + i, 12, 3 + i];
        //                        var content = chartLine.Series.Add(lineY, lineX);
        //                        content.Header = query.columnDesc[i];
        //                    }
        //                }
        //            }

        //            #endregion
        //        }

        //        #endregion

        //        #region 设置列显示格式

        //        if (query.colShowTypes != null)
        //        {
        //            foreach (var item in query.colShowTypes)
        //            {
        //                //带上合计
        //                var formatcol = sheet.Cells[8, item.Key + 2, dt.Rows.Count + 8, item.Key + 2];
        //                formatcol.Style.Numberformat.Format = item.Value;
        //                //if (item.Value == "0.00%" && dt.Rows.Count > 0)//临时策略,合计默认写成100%
        //                //{
        //                //    var value = sheet.Cells[dt.Rows.Count + 8, item.Key + 2, dt.Rows.Count + 8, item.Key + 2].Value;
        //                //    if (Convert.ToDecimal(value)>1)
        //                //    {
        //                //        sheet.Cells[dt.Rows.Count + 8, item.Key + 2, dt.Rows.Count + 8, item.Key + 2].Value = 1;
        //                //    }                              

        //                //}
        //                //colstr[item.Key]
        //            }
        //        }

        //        #endregion

        //        sheet.View.PageLayoutView = false;
        //        //设置是否显示网格线
        //        sheet.View.ShowGridLines = false;
        //        packpage.SaveAs(files);

        //        #endregion
        //    }

        //    return fileName;
        //}

        /// <summary>
        ///     设置边框
        /// </summary>
        /// <param name="wcc"></param>
        /// <param name="rangeArea"></param>
        private void SetWorksheetBorder(ColorConverter wcc, ExcelRange rangeArea)
        {
            rangeArea.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            var convertFromString = wcc.ConvertFromString("#7F7F7F");
            if (convertFromString != null)
            {
                rangeArea.Style.Border.Bottom.Color.SetColor((Color) convertFromString);
                rangeArea.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rangeArea.Style.Border.Top.Color.SetColor((Color) convertFromString);
                rangeArea.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rangeArea.Style.Border.Left.Color.SetColor((Color) convertFromString);
                rangeArea.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rangeArea.Style.Border.Right.Color.SetColor((Color) convertFromString);
                rangeArea.Style.Font.Name = "Arial";
                rangeArea.Style.Font.Size = 10;
            }
        }
    }
}