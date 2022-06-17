using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Room
{
    public class MiddleRoom : Room
    {
        [SerializeField] private MeshRenderer _wall1;
        [SerializeField] private MeshRenderer _wall2;
        [SerializeField] private MeshRenderer _floor;

        public delegate void Click(Vector3 position);


        public static event Click ClickOnRoom;
        public override void Spawn(Direct direct, Window window, Door door, TypeRoom type)
        {
            TypeRoom = type;
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
                case TypeRoom.Playroom:
                    Walls = prefabs.RandomPlayroomWall();
                    Floor = prefabs.RandomPlayroomFloor();
                    break;
                case TypeRoom.LivingRoom:
                    Walls = prefabs.RandomLivingRoomWall();
                    Floor = prefabs.RandomLivingRoomFloor();
                    break;
            }
            SetTextures();
            bool without=false;
            try
            {
                Window = Instantiate(window, transform.position, transform.rotation);
            }
            catch
            {
                without = true;
            }
            Door = Instantiate(door, transform.position, transform.rotation);
            
            if (direct == Direct.Right)
            {
                Vector3 minPosition;
                if (!without)
                {
                    minPosition = new Vector3(transform.position.x - 1.4f, transform.position.y + 1.5f,
                        transform.position.z + 3);


                    Vector3 maxPosition = new Vector3(transform.position.x + 1.4f, transform.position.y + 1.5f,
                        transform.position.z + 3);
                    Window.Spawn(minPosition, maxPosition, window.transform.rotation);

                    Window.transform.SetParent(transform);
                    Window.transform.SetParent(transform, false);
                }

                minPosition = new Vector3(transform.position.x - 2.1f, transform.position.y + 0.1f,
                    transform.position.z + -2.0f);
                Door.Spawn(minPosition, Quaternion.Euler(0, -90, 0));
                Door.transform.SetParent(transform);
                Door.transform.SetParent(transform, false);
            }
            else
            {
                Vector3 minPosition;
                if (!without)
                {
                    minPosition = new Vector3(transform.position.x - 2.5f, transform.position.y + 1.5f,
                        transform.position.z - 1.4f);
                    Vector3 maxPosition = new Vector3(transform.position.x - 2.5f, transform.position.y + 1.5f,
                        transform.position.z + 1.4f);
                    Window.Spawn(minPosition, maxPosition, Quaternion.Euler(0, -90, 0));

                    Window.transform.SetParent(transform);
                    Window.transform.SetParent(transform, false);
                }


                minPosition = new Vector3(transform.position.x - 2.0f, transform.position.y + 0.1f,
                    transform.position.z + 2.1f+0.5f);
                Door.Spawn(minPosition, door.transform.rotation);
                Door.transform.SetParent(transform);
                Door.transform.SetParent(transform, false);
                
                
            }
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
                case TypeRoom.Playroom:
                    _wall1.material = prefabs.GetPlayroomWall(Walls);
                    _wall2.material = prefabs.GetPlayroomWall(Walls);
                    _floor.material = prefabs.GetPlayroomFloor(Floor);
                    break;
                case TypeRoom.LivingRoom:
                    _wall1.material = prefabs.GetLivingRoomWall(Walls);
                    _wall2.material = prefabs.GetLivingRoomWall(Walls);
                    _floor.material = prefabs.GetLivingRoomFloor(Floor);
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

            //Door.transform.localPosition = position;
            Door.transform.SetParent(transform);
            Door.transform.SetParent(transform, false);
            XElement window;
            XElement windowPosition;
            try
            {
                window = room.Element("window");
                windowPosition = window.Element("position");


                index = int.Parse(window.Attribute("index").Value);
                position.x = MyClass.ConvertToFloat(windowPosition.Attribute("x").Value);
                position.y = MyClass.ConvertToFloat(windowPosition.Attribute("y").Value);
                position.z = MyClass.ConvertToFloat(windowPosition.Attribute("z").Value);

                rotation = MyClass.ConvertToFloat(window.Attribute("rotation").Value);
                if (rotation < 0)
                {
                    Window = Instantiate(prefabs.GetWindow(index), position, Quaternion.Euler(0, -90, 0));
                }
                else
                {


                    Window = Instantiate(prefabs.GetWindow(index), position, Quaternion.identity);
                }

               // Window.transform.localPosition = position;
                Window.transform.SetParent(transform);
                Window.transform.SetParent(transform, false);
            }
            catch
            {
                //ignored
            }
            
            
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
                        Furniture[Furniture.Count-1].Wrap();
                    }
                    Furniture[Furniture.Count-1].transform.SetParent(transform);
                    Furniture[Furniture.Count-1].transform.SetParent(transform,true);
                    //if(furniture.Attribute("condition") != null)
                    Furniture[Furniture.Count-1].SetCondition(int.Parse(furniture.Attribute("condition").Value));
                }
            }
            SetTextures();
            

            
        }

        public override void AddFurniture(Furniture newFurniture)
        {
            Furniture.Add(newFurniture);
        }

        public override int[,] GetGrid()
        {
            int[,] temp = new int[(int)transform.localScale.z,(int)transform.localScale.x];
            Vector3 start = new Vector3(transform.position.x  - 2 , transform.position.y, transform.position.z - 3);

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

        public override void SetChildren()
        {
            throw new System.NotImplementedException();
        }

        public override XElement GetElement()
        {
            XElement furnitures = new XElement("furnitures");
            
//            Debug.Log(this);
            foreach (Furniture furniture in Furniture)
            {
                furnitures.Add(furniture.GetElement());
            }

            if (Furniture.Count == 0) furnitures = null;
            XAttribute walls = new XAttribute("walls", Walls);
            XAttribute floor = new XAttribute("floor", Floor);
            XAttribute typeRoom = new XAttribute("typeRoom", (int) TypeRoom);
            XElement door = null;
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
            else window = Window.GetElement();

            XElement element = new XElement("room", walls, floor, typeRoom, furnitures, window, door, "middleRoom");
            return element;
        }

        public override TypeRoom GetTypeRoom()
        {
            return TypeRoom;
        }
        public void OnMouseDown()
        {
//            Debug.Log("Clicked");
            OnClickOnRoom(transform.position);
        }


        private static void OnClickOnRoom(Vector3 position)
        {
            ClickOnRoom?.Invoke(position);
        }
    }
}
