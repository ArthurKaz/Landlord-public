using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Assets.Scripts.Room;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Floor
{
    public class CommonFloor : Floor
    { 
        [SerializeField] private GameObject _buyButton;
       [SerializeField] private GameObject _myList;
       [SerializeField] private GameObject _catalog;
       
       
       
       private void Start()
       {
           NeedFurnitures.Add(new NeedFurniture("Bed",1,"Ліжко"));
           NeedFurnitures.Add(new NeedFurniture("Kitchen",1,"Кухонний уголок"));
           NeedFurnitures.Add(new NeedFurniture("ToiletBowl",1,"Туалет"));
           NeedFurnitures.Add(new NeedFurniture("Fridge",1,"Холодильник"));
           NeedFurnitures.Add(new NeedFurniture("Shower",1,"Душ"));
       }
       


        public override void Subscribe()
        {
            CommonRoom.ClickOnRoom += Click;
            FurnitureGrid.back += Back;
        }

        public override void Unsubscribe()
        {
            CommonRoom.ClickOnRoom -= Click;
            FurnitureGrid.back -= Back;
        }

        private void Back()
        {
            CommonRoom.ClickOnRoom += Click;
            
        }

        private void Click(Vector3 position)
        {
            
            for (int i = 0; i < 3; i++)
            {
                if (Rooms[i] != null)
                {
                    if (Rooms[i].transform.position == position)
                    {
                        FurnitureGrid grid = GameObject.Find("FurnitureGrid").gameObject.GetComponent<FurnitureGrid>();
                        grid.ShowGrid(new Vector3(position.x - 2, position.y + 0.035f, position.z - 2), Rooms[i]);
                     //   Rooms[i].GetComponent<BoxCollider>().gameObject.SetActive(false);
                     Rooms[i].GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
            CommonRoom.ClickOnRoom -= Click;
            Camera.main.GetComponent<MoveTo>().Move(new Vector3(position.x + 4, position.y+4,position.z -5));
            Clock.Hide();
        }
        
        public override void Generation(Room.Room temp)
        {
          
            Direct = (Direct) Random.Range(0, 2);
            Price = Random.Range(10000,20000);
            Queue<TypeRoom> types = new Queue<TypeRoom>();
            if (Direct == Direct.Left)
            {
                types.Enqueue(TypeRoom.Bathroom);
                types.Enqueue(TypeRoom.Kitchen);
                types.Enqueue(TypeRoom.Bedroom);
            }
            else
            {
                types.Enqueue(TypeRoom.Bedroom);
                types.Enqueue(TypeRoom.Kitchen);
                types.Enqueue(TypeRoom.Bathroom);
            }

            Window window = GameObject.Find("Prefabs").GetComponent<Prefabs>().RandomWindow();
            Door door = GameObject.Find("Prefabs").GetComponent<Prefabs>().RandomDoor();
            
            Rooms = new CommonRoom[3] as Room.Room[];
            Vector3 position = new Vector3(0, 0, 0);
            for (int i = 0; i < 3; i++)
            {
                
                Rooms[i] = Instantiate(temp, position, Quaternion.identity);
                position = new Vector3(position.x + 5.1f, position.y, position.z + 5.1f);
                
                Rooms[i].Spawn(Direct,window,door,types.Dequeue());
                Rooms[i].transform.SetParent(transform);
                Rooms[i].transform.SetParent(transform,true);
                Rooms[i].name = ""+i;
            }
        }

        public override void Load(XElement floor,Room.Room room)
        {
            
            Rooms = new CommonRoom[3] as Room.Room[];
            Direct = (Direct) int.Parse(floor.Attribute("direct").Value);
            Price =  int.Parse(floor.Attribute("price").Value);
           
            XElement rooms = floor.Element("rooms");
            Path = floor.Attribute("Screen").Value;
            Vector3 position =  new Vector3(0, 0, 0);

            int i = 0;
            foreach (XElement myRoom in rooms.Elements("room"))
            {
                
                Rooms[i] = Instantiate(room, position, Quaternion.identity);
                Rooms[i].Spawn(myRoom);
                Rooms[i].transform.SetParent(transform);
                Rooms[i].transform.SetParent(transform,true);
                Rooms[i].name = ""+i;
                position = new Vector3(position.x + 5.1f, position.y, position.z + 5.1f);
                i++;
            }

            try
            {
                XElement tenant = floor.Element("Tenant");
                int index = int.Parse(tenant.Element("Index").Value);
                Tenant = Magazine.GetTenant(index);
                Tenant.Load(tenant);
               
               _startTime = DateTime.Parse(floor.Element("lastStart").Value);
            }
            catch
            {
                // ignored
            }
            
            
        }
        
        public override XElement GetElement()
        {
            XElement rooms = new XElement("rooms");
            foreach (Room.Room room in Rooms)
            {
                Debug.Log(this);
                rooms.Add(room.GetElement());
            }
            XAttribute price = new XAttribute("price", Price);
            XAttribute direct = new XAttribute("direct", (int) Direct);
            XAttribute name = new XAttribute("name", "commonFloor");

            //XElement tenant = Tenant.GetElement();

            XElement element = new XElement("floor", rooms,price, direct,TakePhoto(),name);
            if (Tenant != null)
            {
                XElement tenant = Tenant.GetElement();
                element.Add(tenant);
            }
            
            element.Add(new XElement("lastStart",_startTime.ToString()) );
            return element;
        }
    }
}
