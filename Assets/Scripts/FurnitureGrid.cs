using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts
{

    public class FurnitureGrid : MonoBehaviour
    {
      

        
        [SerializeField] private GameObject Panel;
        
        [SerializeField] private GameObject _scrollView;
         private GameObject _shop;
        [SerializeField] private GameObject Move;
        [SerializeField] private FurnitureTemplate _template;
        
        [SerializeField] private Furniture[] prefabs;

        
        private  List<GameObject> center = new List<GameObject>();
        
        private Furniture _flyingFurniture;
        
        private Vector3 _startPosition;
        private Room.Room _room;
        [SerializeField] private GameObject _myList;

        private int[,] _grid;
        private int _x, _y;
        public delegate void ToList();
        public static event ToList back;
        
        public delegate void Save();
        public static event Save save;

        private static FurnitureGrid _obj;
        
        
     
        private void Start()
        {
            _obj = this;
            for (int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i].SetIndex(i);
            }
        }

        public static bool HasTag(string tag)
        {
            try
            {
                foreach (var item in _obj.prefabs)
                {
                    if (item.tag == tag) return true;
                }
            }
            catch (NullReferenceException e)
            {
                return true;
            }

            return false;
        }


        public Furniture GetFurniture(int value)
        {
            return prefabs[value];
        }
        public void ShowGrid(Vector3 position,Room.Room room)
        {
           Panel.SetActive(true);
           _myList.SetActive(false);
           //_catalog = GameObject.Find("Catalog");
//           _catalog.SetActive(false);
           _startPosition = position;
           _room = room;
           _grid = room.GetGrid();
        }

        public void SelectFurniture()
        {
            FurnitureInfo.OnLockFurniture();
            _scrollView.SetActive(true);
            
            _shop = GameObject.Find("Shop");
            
            bool flag = false;
           
          //  center.Add(new GameObject("GameObject",typeof(RectTransform)));
          //  center[center.Count - 1].transform.SetParent(_shop.transform);
            int j = 0;
            for (int i = 0; i < prefabs.Length; i++)
            {
                if ((int)prefabs[i].TypeRoom == (int)_room.GetTypeRoom())
                {
                    flag = true;
                  /*  GameObject newButton =
                        new GameObject("New button " + i, typeof(Image), typeof(Button), typeof(LayoutElement));
                    newButton.transform.localScale = new Vector3(1.6f, 0.3f, 1f);
                    newButton.GetComponent<RectTransform>().rect.Set(0, 0, 160, 30);
                    newButton.tag = "Furniture";
                    newButton.GetComponent<LayoutElement>().minHeight = 35;
                    newButton.transform.SetParent(_shop.transform);
                    newButton.name = "" + i;

                    GameObject newText = new GameObject("New text", typeof(Text));
                    newText.transform.SetParent(newButton.transform);
                    newText.GetComponent<Text>().text = "Buy";
                    newText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                    newText.GetComponent<Text>().color = Color.black;
                    newText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                   
                    newButton.GetComponent<Button>().onClick.AddListener(delegate { StartPlacingFurniture(prefabs[n]); });*/

                  int n = i;
                  
                  if (j == 1)
                  {
                      j = 0;
                      FurnitureTemplate temp = Instantiate(_template);
                      Vector3 position = temp.transform.position;
                      temp.transform.localScale = new Vector3(1, 1, 1);
                      temp.transform.position = new Vector3(position.x+300, position.y, position.z);
                      temp.transform.SetParent(center[center.Count-1].transform);
                      
                      
                      temp.AddFunction(delegate { StartPlacingFurniture(prefabs[n]); });
                      
                      center[center.Count - 1].transform.SetParent(_shop.transform);
                  //    center[center.Count - 1].transform.localScale = new Vector3(1, 1, 1);


                      temp.SetImage(prefabs[n].GetImage());
                      temp.SetPrice(prefabs[n].Price);
                      
                  }
                  else
                  {
                      center.Add(new GameObject("GameObject",typeof(RectTransform)));
                      center[center.Count - 1].transform.SetParent(_shop.transform);
                   //   center[center.Count - 1].transform.localScale = new Vector3(1, 1, 1);
                      
                      FurnitureTemplate temp = Instantiate(_template);
                      temp.transform.SetParent(center[center.Count-1].transform);
                      temp.AddFunction(delegate { StartPlacingFurniture(prefabs[n]); });
                      temp.SetImage(prefabs[n].GetImage());
                      temp.SetPrice(prefabs[n].Price);
                      j++;
                  }
                }
            }

            foreach (var item in center)
            {
                item.transform.localScale = new Vector3(1, 1, 1);
            }
            if(!flag) _scrollView.SetActive(false);
        }

        public void StartPlacingFurniture(Furniture furniture)
        {
            
            Move.SetActive(true);
            _x = 0;
            _y = 0;
            if (_flyingFurniture != null) Destroy(_flyingFurniture.gameObject);
            _flyingFurniture = Instantiate(furniture,_startPosition,Quaternion.identity);
            if (IsPlaceTaken(_x, _y))
            {
//                Debug.Log("Full");
            }
            Close();
           /* Vector3 zeroPosition = _flyingFurniture.transform.position;
            for (; _x < _grid.GetLength(0); _x++)
            {
                for (; _y < _grid.GetLength(1); _y++)
                {
                    if(!IsPlaceTaken(_x,_y)) return;
                    Vector3 newPosition = _flyingFurniture.transform.position;
                    _flyingFurniture.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z + 1);
                }
                zeroPosition = new Vector3(zeroPosition.x - 1, zeroPosition.y, zeroPosition.z);
                _flyingFurniture.transform.position = zeroPosition;
            }
            */
            Vector3 zeroPosition = _flyingFurniture.transform.position;
            for (; _y < _grid.GetLength(0); _y++)
            {

                // Debug.Log(zeroPosition);
                for (_x = 0; _x < _grid.GetLength(1); _x++)
                {
                   
                    if (!IsPlaceTaken(_x, _y)) return;
                    Vector3 newPosition = _flyingFurniture.transform.position;
                    _flyingFurniture.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z + 1);
                }

//                Debug.Log(zeroPosition);
                zeroPosition = new Vector3(zeroPosition.x + 1, zeroPosition.y, zeroPosition.z);
                _flyingFurniture.transform.position = zeroPosition;

            }
            Cancel();
        }

        private void Close()
        {
            foreach (GameObject obj in center)
            {
                
                Destroy(obj.gameObject);
               
            }

            center.Clear();
          /*  GameObject[] buttons = GameObject.FindGameObjectsWithTag("Furniture");
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }*/
          FurnitureInfo.OnUnLockFurniture();
            _scrollView.SetActive(false);
            
        }

        public void Up()
        {
//           Debug.Log(_grid.GetLength(0)+"y = "+_grid.GetLength(1));
           if (IsPlaceTaken(_x + 1, _y)) return;
       //    if (_x+1+_flyingFurniture.Size.y > _grid.GetLength(0)) return;
           _x++;
           
           Vector3 position = new Vector3(_flyingFurniture.transform.position.x, _flyingFurniture.transform.position.y,
               _flyingFurniture.transform.position.z + 1);
           _flyingFurniture.transform.position = position;
           
           
        }

        public void Down()
        {
            if (IsPlaceTaken(_x - 1, _y)) return;
         //   if(_x - 1 < 0) return;
            _x--;
            Vector3 position = new Vector3(_flyingFurniture.transform.position.x, _flyingFurniture.transform.position.y,
                _flyingFurniture.transform.position.z - 1);
            _flyingFurniture.transform.position = position;
        }

        public void Right()
        {
            if (IsPlaceTaken(_x ,1 + _y)) return;

        //    if (_y + 1 + _flyingFurniture.Size.x > _grid.GetLength(1)) return;
            _y++;
            Vector3 position = new Vector3(_flyingFurniture.transform.position.x + 1,
                _flyingFurniture.transform.position.y, _flyingFurniture.transform.position.z);
            _flyingFurniture.transform.position = position;
        }

        public void Left()
        {
            if (IsPlaceTaken(_x, _y - 1)) return;

           // if (_y - 1 < 0) return;
            _y--;
            Vector3 position = new Vector3(_flyingFurniture.transform.position.x - 1,
                _flyingFurniture.transform.position.y, _flyingFurniture.transform.position.z);
            _flyingFurniture.transform.position = position;

        }

        public void Build()
        {
           
            for (int i = 0; i < _flyingFurniture.Size.x; i++)
            {
                for (int j = 0; j < _flyingFurniture.Size.y; j++)
                {
                    _grid[_x + j, _y + i] = 1;
                }
            }
            _flyingFurniture.transform.SetParent(_room.transform);
            _flyingFurniture.transform.SetParent(_room.transform,false);
            _room.AddFurniture(_flyingFurniture);
           
            _flyingFurniture = null;
            Move.SetActive(false);
            FurnitureInfo.OnUnLockFurniture();
            save?.Invoke();
        }

        public void Cancel()
        {
            FurnitureInfo.OnUnLockFurniture();
            Destroy(_flyingFurniture.gameObject) ;
            Move.SetActive(false);
            
        }

        public void Wrap()
        {
            _flyingFurniture.Wrap();
            if (IsPlaceTaken(_x, _y))
            {
                _flyingFurniture.Wrap();
            }
        }

        private bool IsPlaceTaken(int x,int y)
        {
            if (x < 0 || y < 0) return true;
            for (int i = 0; i < _flyingFurniture.Size.x; i++)
            {
                for (int j = 0; j < _flyingFurniture.Size.y; j++)
                {
                    if (x + j >=  _grid.GetLength(0)  ||  y + i >= _grid.GetLength(1) ) return true;
                    if (_grid[x + j, y + i] == 1) return true;
                }
            }

            return false;
        }
        
        public void Back()
        {
           // catalog.SetActive(true);
            //Destroy(panel.gameObject);
            if (_flyingFurniture != null)
            {
                Move.SetActive(false);
                Destroy(_flyingFurniture.gameObject);
            }
            _room.GetComponent<BoxCollider>().enabled = true;
            Panel.SetActive(false);
            _myList.SetActive(true);
            Camera.main.GetComponent<MoveTo>().Move(new Vector3(12, 6, -2));
            back?.Invoke();
        }
    }
}
