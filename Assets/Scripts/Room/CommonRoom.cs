

using System;
using System.Xml.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Assets.Scripts.Room
{
    public class CommonRoom : Room
    {
        [SerializeField] private MeshRenderer _wall1;
        [SerializeField] private MeshRenderer _wall2;
        [SerializeField] private MeshRenderer _floor;
        public delegate void Click(Vector3 position);
        public static event Click ClickOnRoom;
        public override void Spawn(Direct direct, Window window, Door door, TypeRoom type)
        {

            Window = Instantiate(window, transform.position, transform.rotation);
            Door = Instantiate(door, transform.position, transform.rotation);
            TypeRoom = type;


            if (direct == Direct.Right)
            {
                Vector3 minPosition = new Vector3(transform.position.x - 1.4f, transform.position.y + 1.5f,
                    transform.position.z + 2.5f);
                Vector3 maxPosition = new Vector3(transform.position.x + 1.4f, transform.position.y + 1.5f,
                    transform.position.z + 2.5f);
                Window.Spawn(minPosition, maxPosition, window.transform.rotation);

                Window.transform.SetParent(transform);
                Window.transform.SetParent(transform, false);
                minPosition = new Vector3(transform.position.x - 2.1f, transform.position.y + 0.1f,
                    transform.position.z + -2.0f);

                Door.Spawn(minPosition, Quaternion.Euler(0, -90, 0));
                Door.transform.SetParent(transform);
                Door.transform.SetParent(transform, false);
            }
            else
            {
                Vector3 minPosition = new Vector3(transform.position.x - 2.5f, transform.position.y + 1.5f,
                    transform.position.z - 1.4f);
                Vector3 maxPosition = new Vector3(transform.position.x - 2.5f, transform.position.y + 1.5f,
                    transform.position.z + 1.4f);
                Window.Spawn(minPosition, maxPosition, Quaternion.Euler(0, -90, 0));

                Window.transform.SetParent(transform);
                Window.transform.SetParent(transform, false);
                minPosition = new Vector3(transform.position.x - 2.0f, transform.position.y + 0.1f,
                    transform.position.z + 2.1f);


                Door.Spawn(minPosition, door.transform.rotation);
                Door.transform.SetParent(transform);
                Door.transform.SetParent(transform, false);
            }
            Prefabs prefabs = GameObject.Find("Prefabs").GetComponent<Prefabs>();
            switch (TypeRoom)
            {
                case TypeRoom.Bathroom:
                    Walls = prefabs.RandomBathroomWall();
                    Floor = prefabs.RandomBathroomFloor();
                    break;
                case TypeRoom.Bedroom:
                    Walls = prefabs.RandomBedroomWall();
                    Floor = prefabs.RandomBedroomFloor();
                    break;
                case TypeRoom.Kitchen:
                    Walls = prefabs.RandomKitchenWall();
                    Floor = prefabs.RandomKitchenFloor();
                    break;
            }
            SetTextures();
        }

        private void SetTextures()
        {
            Prefabs prefabs = GameObject.Find("Prefabs").GetComponent<Prefabs>();
            switch (TypeRoom)
            {
                case TypeRoom.Bathroom:
                    _wall1.material = prefabs.GetBathroomWall(Walls);
                    _wall2.material = prefabs.GetBathroomWall(Walls);
                    _floor.material = prefabs.GetBathroomFloor(Floor);
                    break;
                case TypeRoom.Bedroom:
                    _wall1.material = prefabs.GetBedroomWall(Walls);
                    _wall2.material = prefabs.GetBedroomWall(Walls);
                    _floor.material = prefabs.GetBedroomFloor(Floor);
                    break;
                case TypeRoom.Kitchen:
                    _wall1.material = prefabs.GetKitchenWall(Walls);
                    _wall2.material = prefabs.GetKitchenWall(Walls);
                    _floor.material = prefabs.GetKitchenFloor(Floor);
                    break;
            }
        }

        public override void Spawn(XElement room)
        {
            TypeRoom = (TypeRoom) int.Parse(room.Attribute("typeRoom").Value);
            Walls = int.Parse(room.Attribute("walls").Value);
            Floor = int.Parse(room.Attribute("floor").Value);
            Prefabs prefabs = GameObject.Find("Prefabs").GetComponent<Prefabs>();

            XElement door = room.Element("door");
            int index = int.Parse(door.Attribute("index").Value);

            Vector3 position = new Vector3();
            XElement doorPosition = door.Element("position");
            position.x = MyClass.ConvertToFloat(doorPosition.Attribute("x").Value);
            position.y = MyClass.ConvertToFloat(doorPosition.Attribute("y").Value);
            position.z = MyClass.ConvertToFloat(doorPosition.Attribute("z").Value);

            float rotation = MyClass.ConvertToFloat(door.Attribute("rotation").Value);
            if (rotation < 0)
            {
                Door = Instantiate(prefabs.GetDoor(index), position, Quaternion.Euler(0, -90, 0));
                Door.ChangeSize();

            }
            else Door = Instantiate(prefabs.GetDoor(index), position, Quaternion.identity);

            // Door.transform.localPosition = position;


            Door.transform.SetParent(transform);
            Door.transform.SetParent(transform, false);

            XElement window = room.Element("window");
            XElement windowPosition = window.Element("position");
            index = int.Parse(window.Attribute("index").Value);
            position.x = MyClass.ConvertToFloat(windowPosition.Attribute("x").Value);
            position.y = MyClass.ConvertToFloat(windowPosition.Attribute("y").Value);
            position.z = MyClass.ConvertToFloat(windowPosition.Attribute("z").Value);
//            Debug.Log("Xml loc - " + position);
            rotation = MyClass.ConvertToFloat(window.Attribute("rotation").Value);
            if (rotation < 0)
            {
                Window = Instantiate(prefabs.GetWindow(index), position, Quaternion.Euler(0, -90, 0));
            }
            else
            {


                Window = Instantiate(prefabs.GetWindow(index), position, Quaternion.identity);
            }

            Window.transform.SetParent(transform);
            Window.transform.SetParent(transform, false);
            //    Window.transform.localPosition = position;

            SetTextures();
            FurnitureGrid furnitureGrid = GameObject.Find("FurnitureGrid").GetComponent<FurnitureGrid>();
            XElement furnitures = room.Element("furnitures");
            if (furnitures != null)
            {
                foreach (XElement furniture in furnitures.Elements("furniture"))
                {
                    XElement pos = furniture.Element("position");
                    position.x = MyClass.ConvertToFloat(pos.Attribute("x").Value);
                    position.y = MyClass.ConvertToFloat(pos.Attribute("y").Value);
                    position.z = MyClass.ConvertToFloat(pos.Attribute("z").Value);
                    Furniture.Add(Instantiate(
                        furnitureGrid.GetFurniture(Int32.Parse(furniture.Attribute("index").Value)), position,
                        Quaternion.identity));
                    for (int i = 0; i < int.Parse(furniture.Attribute("turns").Value); i++)
                    {
                        Furniture[Furniture.Count - 1].Wrap();
                    }

                    Furniture[Furniture.Count - 1].SetCondition(int.Parse(furniture.Attribute("condition").Value));
                    Furniture[Furniture.Count - 1].transform.SetParent(transform);
                    Furniture[Furniture.Count - 1].transform.SetParent(transform, true);
                }
            }

        }



        public override TypeRoom GetTypeRoom()
        {
            return TypeRoom;
        }

        public override int[,] GetGrid()
        {
            
            int[,] temp = new int[(int)transform.localScale.x,(int)transform.localScale.z];
            Vector3 start = new Vector3(transform.position.x  - 2 , transform.position.y, transform.position.z - 2);

            Door.SetGrid(temp,transform.position);
            
            if (Furniture != null)
            {
                for (int i = 0; i < Furniture.Count; i++)
                {
                    if (Furniture[i] != null)
                    {
                        Vector3 position = Furniture[i].transform.position;
                        float y = position.z - start.z;
                        float x = position.x - start.x;

                        for (int k = 0; k < Furniture[i].Size.y; k++)
                        {
                            for (int j = 0; j < Furniture[i].Size.x; j++)
                            {
                                temp[(int) y + k,(int)x + j] = 1;
                            }
                        }
                    }
                }
            }
            

            return temp;
        }

        public void OnMouseDown()
        {
            OnClickOnRoom(transform.position);
        }


        private static void OnClickOnRoom(Vector3 position)
        {
            ClickOnRoom?.Invoke(position);
            
        }

        public override void AddFurniture(Furniture newFurniture)
        {
            Furniture.Add(newFurniture);
           foreach (Furniture furniture in Furniture)
           {
           }
            
        }

        public override void SetChildren()
        {
            Door =  gameObject.GetComponentsInChildren<Door>()[0];
            Window =  gameObject.GetComponentsInChildren<Window>()[0];
        }

        public override XElement GetElement()
        {

            XElement furnitures = new XElement("furnitures");
            foreach (Furniture furniture in Furniture)
            {
                furnitures.Add(furniture.GetElement());
            }

            if (Furniture.Count == 0) furnitures = null;
            
            XAttribute walls = new XAttribute("walls", Walls);
            XAttribute floor = new XAttribute("floor", Floor);
            XAttribute typeRoom = new XAttribute("typeRoom", (int) TypeRoom);

            XElement door =null;
            if (Door != null)
            {
                try
                {

                    door = Door.GetElement();
                }
                catch
                {
                    door = null;
                }
            }

            XElement window = null;
            if (Door != null)
            {
                try
                {
                    window = Window.GetElement();
                }
                catch
                {
                    window = null;
                }
            }

            Window.GetElement();
            
            XElement element = new XElement("room",walls,floor,typeRoom,furnitures,window,door,"commonRoom");
            return element;
        }
    }
    
}
