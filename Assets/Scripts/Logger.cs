using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Logger
    {
        private static string _path = Application.persistentDataPath + "/Loggs";
        private static XElement _root = null;// new XElement("Root");
        
        static Logger()
        {
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            
            _path +="/"+  DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ".xml";
            UnityEngine.Debug.Log(_path);
            if(File.Exists(_path)) _root =  XDocument.Parse(File.ReadAllText(_path)).Element("Root");

        }
        public static void Debug(string message)
        {
            XElement debugs;
            
            XElement mess = new XElement("Message", message);
            XElement data = new XElement("Time",
                DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
        //    debugs.Add("Debug",mess,data);
           
            
            
            if (_root == null)
            {
                _root = new XElement("Root");
                debugs = new XElement("Debugs");
                debugs.Add("Debug",mess,data);
                _root.Add(debugs);
            }
            else _root.Element("Debugs")?.Add("Debug",mess,data);
            
            
            
            Save();
        }

        private static void Save()
        {
            XDocument saveDoc = new XDocument(_root);
            File.WriteAllText(_path, saveDoc.ToString());
        }
    }
}