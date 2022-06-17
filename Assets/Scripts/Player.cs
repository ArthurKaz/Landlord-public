using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Assets.Scripts.Floor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {

        
        private static List<Floor.Floor> _myFloors = new List<Floor.Floor>();
        private static XElement root = new XElement("root");

        private static XElement money = new XElement("root");

        [SerializeField] private static int _balance;
        private static Text _balanceText;
        
      //  private static bool _saveTime = false;
        
        private static string path;// = Application.persistentDataPath + "/Info.xml";
        private static string path2;
        private string path3;
        [SerializeField] private Button _informationAboutTenant;
        [SerializeField] private GameObject _show;

        [SerializeField] private GameObject _scrollView;
     //  [SerializeField] private Room.Room[] _prefabs;
        
        [SerializeField] private Room.Room _commonRoom;
        [SerializeField] private Room.Room _middleRoom;
        
        [SerializeField] private GameObject _myList;

        [SerializeField] private FloorTemplate _floorTemplate;
        
        public void Start()
        {
            
            _myFloors.Clear();
            _balanceText = GameObject.Find("MoneyView").GetComponent<Text>();
            path = Application.persistentDataPath + "/Info.xml";
//            Debug.Log(path);
            path2 = Application.persistentDataPath + "/Money.xml";
            path3 = Application.persistentDataPath + "/Time.xml";
            //   MiddleFloor.buy += AddFloor;
            FurnitureGrid.save += SaveAll;
            foreach (Floor.Floor floor in _myFloors)
            {
                
                //Debug.Log(floor);
            }
//            Debug.Log("Start");
            //Load();
           // _floor.Generation();
           //Load();
           if (File.Exists(path))
           {
               root = XDocument.Parse(File.ReadAllText(path)).Element("root");
           }

           if (File.Exists(path2))
           {
               money = XDocument.Parse(File.ReadAllText(path2)).Element("root");
               _balanceText.text = money.Attribute("balance").Value;
               _balance = int.Parse(money.Attribute("balance").Value);
           }

           if (!File.Exists(path2) && !File.Exists(path))
           {
               EarnMoney(30000);
           }
           

           StartTimer();
           if (!PlayerPrefs.HasKey("lastSession")) PlayerPrefs.SetString("lastSession", DateTime.Now.ToString());
        //   Load();
        Debug.Log(path);
        }

        private void StartTimer()
        {
            StartCoroutine(ExecuteAfterTime(1));
        }

        public void EarnMoney(int earnMoney)
        {

            if (earnMoney < 0) return;
            _balance += earnMoney;
            Debug.Log("Ваш зароботок - " + money);
            _balanceText.text = _balance.ToString();
            
            money = new XElement("root");
            XAttribute balance = new XAttribute("balance", _balance);
            money.Add(balance);
            XDocument saveDoc = new XDocument(money);
            File.WriteAllText(path2, saveDoc.ToString());
        }
        public static bool  SpendMoney(int spendMoney)
        {
            if (spendMoney < 0)
            {
               
                return false;
            }
            if (spendMoney > _balance)
            {
                Message.ShowMessage("Покупка", "Недостатньо коштів ("+Math.Abs(_balance-spendMoney)+")");
                Logger.Debug("Покупка невдалася;"+"Невистачає - "+ Math.Abs(_balance - spendMoney));
                return false;
            }
            _balance -= spendMoney;
            if(_balanceText!=null)_balanceText.text = _balance.ToString();

            money = new XElement("root");
            XAttribute balance = new XAttribute("balance", _balance);
            money.Add(balance);
            XDocument saveDoc = new XDocument(money);
            File.WriteAllText( path2, saveDoc.ToString());
            Logger.Debug("Покупка превдеена успішно;"+"Ціна - "+ spendMoney);
            return true;
        }
        private void OnApplicationPause(bool pause)
        {
            if(pause) Debug.Log("pause");
        }
        IEnumerator ExecuteAfterTime(float timeInSec)
        {
            int seconds = 0;
            int minutes = 0;


            if (PlayerPrefs.HasKey("lastSession"))
            {
                TimeSpan ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("lastSession"));
                int allSeconds = (ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds);
                float timeForEarn = 1440.0f;
                float iterations = allSeconds / timeForEarn;

                if (iterations < 1.0f)
                {
                    seconds = ts.Seconds;
                    minutes = ts.Minutes;
                    
                  //  Clock.StartTimer(seconds,minutes,delegate { Debug.Log("Action"); });
                }
                else
                {
                    iterations = (iterations > 4) ? 4 : iterations;
                    int earnings = 0;
                    Load();
                    for (int i = 0; i < (int) iterations; i++)
                    {

                        allSeconds = allSeconds - (int) timeForEarn;
                        foreach (Floor.Floor floor in _myFloors)
                        {
                            Debug.Log(floor.GetMoney());
                            earnings += floor.GetMoney();
                        }
                        
                    }
                    EarnMoney(earnings);

                    if (iterations != 4)
                    {
                        Debug.Log("All Seconds - " + allSeconds);
                        int allMinutes = allSeconds / 60;
                        minutes = allMinutes;
                        Debug.Log("All minutes - " + allMinutes);
                        int allHours = allMinutes / 60;
                        seconds = allMinutes;                        
                        Debug.Log("All hours - " + allHours);
                    }
                    else
                    {
                        seconds = 0;
                        minutes = 0;
                    }

                    PlayerPrefs.DeleteKey("lastSession");
                }
            }


            while (true)
            {
                seconds++;
                if (seconds == 60)
                {
                    seconds = 0;
                    minutes++;
                }

                if (minutes == 24)
                {
                    minutes = 0;
                    int earnings = 0;
                    foreach (Floor.Floor floor in _myFloors)
                    {
                        earnings += floor.GetMoney();
                    }
                    EarnMoney(earnings);
                    PlayerPrefs.SetString("lastSession", DateTime.Now.ToString());
                }

                yield return new WaitForSeconds(timeInSec);

            }
        }


        public static void SaveAll()
        {
            root =  new XElement("root");
            foreach (Floor.Floor floor in _myFloors)
            {
                root.Add(floor.GetElement());
                
            }
            XDocument saveDoc = new XDocument(root);
            File.WriteAllText(path, saveDoc.ToString());
        }

        public void Load()
        {
           root = null;
            if (File.Exists(path))
            {
                root = XDocument.Parse(File.ReadAllText(path)).Element("root");
                foreach (XElement floor in root.Elements("floor"))
                {
                    GameObject myFloor = new GameObject();
                    myFloor.name = "Floor";

                    switch (floor.Attribute("name").Value)
                    {
                        case  "commonFloor":
                            myFloor.AddComponent<CommonFloor>().Load(floor,_commonRoom);
                            break;
                        case "middleFloor":
                            myFloor.AddComponent<MiddleFloor>().Load(floor,_middleRoom);
                            break;
                    }
                    _myFloors.Add(myFloor.GetComponent<Floor.Floor>());
                    myFloor.SetActive(false);
                }
            }

        }

        public static void AddFloor(Floor.Floor floor)
        {
            if (SpendMoney(floor.GetPrice()))
            {
                if (root == null) root = new XElement("root");
                try
                {
                    root.Add(floor.GetElement());
                }
                catch
                {
                    return;
                }
                XDocument saveDoc = new XDocument(root);
                File.WriteAllText(path, saveDoc.ToString());
                
            }


        }

        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        public void ShowList()
        {
            Clear();
            _informationAboutTenant.gameObject.SetActive(false);
            Load();
            if (_myFloors.Count == 0) return;


            _show.SetActive(true);
            _scrollView.SetActive(true);
            int i = 0;
            foreach (Floor.Floor element in _myFloors)
            {
                element.gameObject.SetActive(false);
                Floor.Floor obj = element;
                FloorTemplate temp = Instantiate(_floorTemplate);
                temp.transform.SetParent(_show.transform);
                temp.tag = "myFloor";
                temp.SetImage(obj.GetPath());
                temp.AddFunction(delegate { Show(obj); });
                temp.AddFunction(delegate { HideList(); });
            }

        }



        private void Show(Floor.Floor obj)
        {
            
            obj.gameObject.SetActive(true);
            
            obj.Subscribe();
            TenantTemplate.Floor = obj;
            if (obj.Tenant == null)
            {
                _informationAboutTenant.gameObject.SetActive(false);
                TenantTemplate.ShowButton();
                
            }
            else
            {
                _informationAboutTenant.gameObject.SetActive(true);
                TenantTemplate.HideButton();
                _informationAboutTenant.onClick.AddListener(delegate { TenantTemplate.ShowInfo(obj.Tenant,false); });
                obj.ShowTimer();
                
            }
        }

        public static void Clear()
        {
            foreach (var floor in _myFloors)
            {
                Destroy(floor.gameObject);
            }
            _myFloors.Clear();
        }
        private void HideList()
        {
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("myFloor");
            foreach (GameObject button  in buttons)
            {
                Destroy(button);
            }
            _show.SetActive(false);
           _scrollView.SetActive(false);
            
        }
    }
}
