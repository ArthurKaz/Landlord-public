using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public static class Money
{
    public static int Balance
    {
        get {
            XElement money = XDocument.Parse(File.ReadAllText(path)).Element("root");
            return int.Parse(money.Attribute("balance").Value);
        }
        private set
        {
            if (value < 0) throw new Exception("Баланс не може бути від'ємним" );
            Balance = value;
        }
    }
    private static string path = Application.persistentDataPath + "/Money.xml";
}
