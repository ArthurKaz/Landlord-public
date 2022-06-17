using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Floor
{
    public class NeedFurniture
    {
        private string _tag;
        private int _amount;
        private string _furniture;

        public string Tag => _tag;

        public string Furniture => _furniture;

        public int Amount => _amount;

        public NeedFurniture(string tag, int amount, string thing)
        {
            if (!FurnitureGrid.HasTag(tag)) throw new IOException(tag + " - Таких меблів незнайдено!");
            _tag = tag;
            _amount = amount;
            _furniture = thing;
//            Debug.Log(_amount);
        }
    }

    public abstract class Floor : MonoBehaviour
    {
        [SerializeField] protected global::Assets.Scripts.Room.Room[] Rooms;
        [SerializeField] protected Direct Direct;
        [SerializeField] private int _price;
        protected string Path;
        protected List<NeedFurniture> NeedFurnitures = new List<NeedFurniture>();
        protected DateTime? _startTime = null;

        public int Price
        {
            get => _price;
            protected set => _price = value;
        }

        public Tenant Tenant { get; set; } = null;

        public bool ThereIsAllTheFurniture()
        {
            bool flag = true;
            string info = "Невистачає: ";
            foreach (var item in NeedFurnitures)
            {
                var search = from e in AllFurniture() where e.tag == item.Tag select e;

                foreach (var VARIABLE in AllFurniture())
                {
                    Debug.Log(VARIABLE);
                }

                if (search.Count() < item.Amount)
                {
                    Debug.Log(item.Amount.ToString());
                    info += item.Furniture + "(потрібно - " + item.Amount + ", невистачає - " +
                            (item.Amount - search.Count()) + ")" +
                            (NeedFurnitures[NeedFurnitures.Count - 1] == item ? "." : ", ");
                    flag = false;
                    Debug.Log(item.Furniture + " незнайдено");
                }

            }


            if (flag) return true;
            Message.ShowMessage("Здача квартири в оренду", info);
            return false;
        }

        private List<Furniture> AllFurniture()
        {
            List<Furniture> all = new List<Furniture>();
            foreach (var room in Rooms)
            {
                all.AddRange(room.Furnitures);
            }

            return all;
        }

        public string GetPath()
        {
            return Path;
        }

        public int GetPrice()
        {
            return Price;
        }

        public delegate void Buy(Floor floor);

        public static event Buy buy;

        public void BuyFloor()
        {
            Debug.Log("Calls");
            buy?.Invoke(this);
        }

        public abstract void Generation(global::Assets.Scripts.Room.Room temp);
        public abstract void Subscribe();
        public abstract void Unsubscribe();
        public abstract void Load(XElement floor, Room.Room room);
        public abstract XElement GetElement();

        public XAttribute TakePhoto()
        {
            if (Path != null)
            {
                return new XAttribute("Screen", Path);
            }

            Debug.Log("calls");
            GameObject canvas = GameObject.Find("Catalog");
            if (canvas == null) throw new Exception("Canvas not installed");
            canvas.SetActive(false);


            int i = 1;
            string path = Application.persistentDataPath;
            while (true)
            {
                if (!File.Exists(Application.persistentDataPath + "/Floor" + i + ".png"))
                {
                    path = Application.persistentDataPath + "/Floor" + i + ".png";
                    StartCoroutine(TakeSnapshot(canvas, path));
                    break;
                }

                i++;
            }

            XAttribute screen = new XAttribute("Screen", path);


            return screen;
        }

        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        public IEnumerator TakeSnapshot(GameObject canvas, string path)
        {
            yield return frameEnd;

            Camera screen = Camera.main;
            screen.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);
            RenderTexture RT = screen.targetTexture;

            Texture2D texture;

            texture = new Texture2D(RT.width, RT.height, TextureFormat.RGB24, false, false);

            try
            {
                texture.ReadPixels(new Rect(0, 0, RT.width, RT.height), 0, 0);
            }
            catch
            {

            }

            texture.Apply();

            byte[] byteArray = texture.EncodeToPNG();
            File.WriteAllBytes(path, byteArray);
            screen.targetTexture = null;
            canvas.SetActive(true);
        }

        public int GetMoney()
        {
            PossibleDamage temp = new PossibleDamage(Damage.Level1);
            foreach (Room.Room element in Rooms)
            {
                element.GetMoney(temp);
            }

            Debug.Log("Length - " + Rooms.Length);
            return 3000 * Rooms.Length;
        }

        public static void clearListeners()
        {
            buy = null;
        }

        public void SetTenant(Tenant tenant)
        {
            Tenant = tenant;
            _startTime = DateTime.Now;
        }

        public void ShowTimer()
        {
            TimeSpan ts = (TimeSpan) (DateTime.Now - _startTime);
            int totalSeconds = (int) ts.TotalSeconds;//- (4 * 24 * 60);
            for (int i = 0; i < 4; i++)
            {
                totalSeconds -= 24 * 60;
                if (totalSeconds < 0)
                {
                    Clock.StartTimer(Math.Abs(totalSeconds),() =>
                    {
                        
                        Debug.Log("Action");
                    }); 
                    return;
                }
                Debug.Log("Action");
            }

            Clock.StartTimer(0,24, () =>
            {
                Debug.Log("Action");
            });
        }
        
        
    }
}
