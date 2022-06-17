using System;
using System.Xml.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public enum Direct
    {
        Left = 0,
        Right = 1
    }

    public enum TypeRoom
    {
        Bedroom = 0,
        Kitchen = 1,
        Bathroom = 2,
        Playroom = 3,
        LivingRoom = 4
    }

    [SerializeField]
    public class PossibleDamage
    {
        
        [SerializeField] private int _minDamage;
        [SerializeField] private int _maxDamage;
        private Damage _level;

        public XElement GetElement()
        {
            XAttribute min = new XAttribute("Min", _minDamage);
            XAttribute max = new XAttribute("Max", _maxDamage);
            XAttribute level = new XAttribute("Level",(int)_level);
            return new XElement("PossibleDamage", min, max, level);
        }
        public string Info
        {
            get
            {
                switch (_level)
                {
                    case Damage.Level1: return "Дуже охайна людина, зберігає речі в чистоті";
                    case Damage.Level2: return "Досить охайно відноситься до речей, але не любить прибиратись";
                    case Damage.Level3: return "Часто проводить прибирання, але не охайно відноситься до речей";
                    case Damage.LevelMax: return "Не любить прибиратись, не охайно відоситься до речей";
                }
                throw new Exception("Іноформація не вибрана");
            }

        }



    public PossibleDamage(Damage level)
        {
            _level = level;
            switch (level)
            {
                case Damage.Level1:
                    _minDamage = Random.Range(0, 3);
                    _maxDamage = _minDamage + Random.Range(1, 3);
                    break;
                case Damage.Level2:
                    _minDamage = Random.Range(1, 4);
                    _maxDamage =  _minDamage + Random.Range(1, 3);
                break;
                case Damage.Level3:
                    _minDamage = Random.Range(5, 10);
                     _maxDamage = _minDamage + Random.Range(1,5);
                    break;
                case Damage.LevelMax:
                    _minDamage = Random.Range(8, 12);
                    _maxDamage =  _minDamage + Random.Range(2, 9);
                break;
            }
        }

    public PossibleDamage(XElement value)
    {
        _minDamage = int.Parse(value.Attribute("Min").Value);
        _maxDamage = int.Parse(value.Attribute("Max").Value);
        _level = (Damage) int.Parse(value.Attribute("Level").Value);

    }


    public int ReceiveDamage()
        {
            return Random.Range(_minDamage, _maxDamage);
        }
    }
    public enum Damage
    {
        Level1 = 0,
        Level2,
        Level3,
        LevelMax
    }
   
    public static class MyClass
    {
        public static float ConvertToFloat(string element)
        {
            if (Application.platform == RuntimePlatform.Android) return int.Parse(element);
            
            string str = element;
            string str2 = null;
            foreach (char symbol in str)
            {
                if (symbol == '.') str2 += ",";
                else str2 += symbol;
            }
            return float.Parse(str2);
        }
        public static string ToStringAge (int age)
        {

            string str = age.ToString();
            if (str[str.Length - 1] == '1') return " рік";
            if (str[str.Length - 1] > '1' && str[str.Length - 1] < '5') return " роки";
            return " років";
        }
    }

}