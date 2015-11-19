using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Configuration;
using System.Xml.Serialization;
using System.Text;

namespace CCN.Resource.Common
{
    public class ReadXML
    {
        public static XmlConfig GetMenuList()
        {
            var path = ConfigurationManager.AppSettings["MenuSettingPath"];
            var xmlFilePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, path);
          //  XDocument doc = XDocument.Load(xmlFilePath);
            XmlConfig menuList = DeserializeFromXml<XmlConfig>(xmlFilePath);
            return menuList;
        }

        /// <summary>
        /// XML序列化某一类型到指定的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public static void SerializeToXml<T>(string filePath, T obj)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    xs.Serialize(writer, obj);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 从某一XML文件反序列化到某一类型
        /// </summary>
        /// <param name="filePath">待反序列化的XML文件名称</param>
        /// <param name="type">反序列化出的</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                    throw new ArgumentNullException(filePath + " not Exists");

                using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    T ret = (T)xs.Deserialize(reader);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }

    [XmlType(TypeName = "MenuConfig")]
    public class XmlConfig
    {
        [XmlArray("MenuGroups")]
        public List<MenuGroup> menugroups { get; set; }
    }

    /// <summary>
    /// 一级菜单
    /// </summary>
    [XmlType(TypeName = "MenuGroup")]
    public class MenuGroup
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string icon { get; set; }
        [XmlAttribute]
        public bool submenu { get; set; }
        [XmlAttribute]
        public string url { get; set; }
        [XmlArray("MenuArray")]
        public List<paramsItem> MenuArray { get; set; }
    }

    /// <summary>
    /// 二级菜单
    /// </summary>
    [XmlType(TypeName = "Menu")]
    public class paramsItem
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string url { get; set; }
        [XmlAttribute]
        public string icon { get; set; }
        [XmlAttribute]
        public string info { get; set; }
        [XmlAttribute]
        public bool submenu { get; set; }
        [XmlArray("MenuSubArr")]
        public List<paramsSub> MenuArray { get; set; }
    }

    /// <summary>
    /// 三级菜单
    /// </summary>
    [XmlType(TypeName = "MenuSub")]
    public class paramsSub
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string url { get; set; }
        [XmlAttribute]
        public string info { get; set; }
        [XmlAttribute]
        public string icon { get; set; }
    }
}