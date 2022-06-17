using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Assets.Scripts.Room;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Floor
{
    public class MiddleFloor : Floor
    {
        // public override event Buy buy;

        private void Start()
        {
            NeedFurnitures.Add(new NeedFurniture("Bed",1,"Ліжко"));
            NeedFurnitures.Add(new NeedFurniture("Kitchen",1,"Кухонний уголок"));
            NeedFurnitures.Add(new NeedFurniture("ToiletBowl",1,"Туалет"));
            NeedFurnitures.Add(new NeedFurniture("Fridge",1,"Холодильник"));
            NeedFurnitures.Add(new NeedFurniture("Shower",1,"Душ"));
        }

        public override void Generation(Room.Room temp)
        {
            Direct = (Direct) Random.Range(0, 2);
            Price = Random.Range(25000, 40000);
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

            Rooms = new MiddleRoom[Random.Range(3, 6)] as Room.Room[];

            Vector3 position = new Vector3(-3, -3, 1);
            for (int i = 0; i < 3; i++)
            {
                Rooms[i] = Instantiate(temp, position, Quaternion.identity);
                position = new Vector3(position.x + 5.1f, position.y, position.z + 6.1f);
                Rooms[i].transform.SetParent(transform);
                Rooms[i].transform.SetParent(transform, true);
                if (Rooms.Length <= 3)
                {
                    Rooms[i].Spawn(Direct, window, door, types.Dequeue());

                }
                else
                {
                    if (i != 0 && i != 2) Rooms[i].Spawn(Direct.Left, null, door, types.Dequeue());
                    else if (i == 2) Rooms[i].Spawn(Direct.Right, window, door, types.Dequeue());
                    else Rooms[i].Spawn(Direct.Left, window, door, types.Dequeue());
                }
            }

            position = new Vector3(-3, -3 + 3.4f, 1 + 6.1f);
            for (int i = 3; i < Rooms.Length; i++)
            {
                types.Enqueue((TypeRoom) Random.Range(3, 5));
                Rooms[i] = Instantiate(temp, position, Quaternion.identity);
                position = new Vector3(position.x + 5.1f, position.y, position.z + 6.1f);
                Rooms[i].Spawn(Direct, window, door, types.Dequeue());
                Rooms[i].transform.SetParent(transform);
                Rooms[i].transform.SetParent(transform, true);
            }
        }
        

        public override XElement GetElement()
        {
            
            XElement rooms = new XElement("rooms");
            foreach (Room.Room room in Rooms)
            {
                rooms.Add(room.GetElement());
            }

            XAttribute price = new XAttribute("price", Price);
            XAttribute direct = new XAttribute("direct", (int) Direct);
            XAttribute name = new XAttribute("name", "middleFloor");
            
            XElement element = new XElement("floor", rooms, price, direct,TakePhoto(), name);
            if (Tenant != null)
            {
                XElement tenant = Tenant.GetElement();
                element.Add(tenant);
            }
            element.Add(new XElement("lastStart",_startTime.ToString()) );
            return element;
        }

        public override void Load(XElement floor, Room.Room room)
        {
            
            Direct = (Direct) int.Parse(floor.Attribute("direct").Value);

            Price = int.Parse(floor.Attribute("price").Value);
            XElement rooms = floor.Element("rooms");
            Path = floor.Attribute("Screen").Value;
            int amount = 0;
            foreach (XElement myRoom in rooms.Elements("room"))
            {
                amount++;
            }

            Rooms = new MiddleRoom[amount] as Room.Room[];

            Vector3 position = new Vector3(-3, -3, 1);
            for (int i = 0; i < 3; i++)
            {
                Rooms[i] = Instantiate(room, position, Quaternion.identity);
                position = new Vector3(position.x + 5.1f, position.y, position.z + 6.1f);
                Rooms[i].transform.SetParent(transform);
                Rooms[i].transform.SetParent(transform, true);

            }

            position = new Vector3(-3, -3 + 3.4f, 1 + 6.1f);
            for (int i = 3; i < Rooms.Length; i++)
            {

                Rooms[i] = Instantiate(room, position, Quaternion.identity);
                position = new Vector3(position.x + 5.1f, position.y, position.z + 6.1f);
                Rooms[i].transform.SetParent(transform);
                Rooms[i].transform.SetParent(transform, true);
            }

            int j = 0;

            foreach (XElement myRoom in rooms.Elements("room"))
            {
                Rooms[j].name = "" + j;
                Rooms[j].Spawn(myRoom);
                j++;
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
//            Debug.Log(floor.Element("lastStart"));

        }

        public override void Subscribe()
        {
            MiddleRoom.ClickOnRoom += Click;
            FurnitureGrid.back += Back;
        }

        public override void Unsubscribe()
        {
            MiddleRoom.ClickOnRoom -= Click;
            FurnitureGrid.back -= Back;
        }
        
        private void Click(Vector3 position)
        {
            MiddleRoom.ClickOnRoom -= Click;
            Camera.main.GetComponent<MoveTo>().Move(new Vector3(position.x + 4, position.y+4,position.z -5));
            for (int i = 0; i < Rooms.Length; i++)
            {
                if (Rooms[i] != null)
                {
                    if (Rooms[i].transform.position == position)
                    {

                        FurnitureGrid grid = GameObject.Find("FurnitureGrid").gameObject.GetComponent<FurnitureGrid>();
                        grid.ShowGrid(new Vector3(position.x - 2, position.y + 0.035f, position.z - 2.5f), Rooms[i]);

                        Rooms[i].GetComponent<BoxCollider>().enabled = false;
                        //  Rooms[i].GetComponent<BoxCollider>().gameObject.SetActive(false);
                    }
                }
            }
            Clock.Hide();
        }
        private void Back()
        {
            MiddleRoom.ClickOnRoom += Click;
        }
    }
}
