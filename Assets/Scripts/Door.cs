using System.Xml.Linq;
using Assets.Prefabs.Doors;
using UnityEngine;

namespace Assets.Scripts
{
    public class Door : MonoBehaviour
    {
        private Vector3 _position;
        [SerializeField] private int _index;
        [SerializeField] private Quaternion _rotation;
        [SerializeField]
        private GameObject camera;
        [SerializeField]
        private Teleporter[] Teleporter = new Teleporter[2];
        
        [SerializeField ]private Vector2Int _size = Vector2Int.one;

        public void SetIndex(int value)
        {
            _index = value;
        }

        public void ChangeSize()
        {
            int temp = _size.x;
            _size.x = 1;
            _size.y = 2;
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y,
                camera.transform.position.z - 10);
            foreach (var element in Teleporter)
            {
                element.Wrap();
            }
        }
        
        public void Spawn(Vector3 minPosition,Quaternion rotation)
        {
            _rotation = rotation;
            _position = new Vector3(minPosition.x+ Random.Range(0, 4), minPosition.y, minPosition.z);
           /* _position = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z));*/
           
            if (_rotation.y < 0)
            {
                _position = new Vector3(minPosition.x, minPosition.y, minPosition.z+Random.Range(0,3));
               ChangeSize();
                
            }
            transform.position = _position;
            transform.rotation = _rotation;
            //  if(_rotation)
        }

        public int[,] SetGrid(int[,] grid,Vector3 position)
        {
           
            
            
            if (transform.rotation.y < 0)
            { 
                Vector3 startPosition =new Vector3(position.x - 2.1f, position.y + 0.1f,
                    position.z + - 2.0f);
//                Debug.Log(startPosition);
  //              Debug.Log("Df - " +( transform.position.z - startPosition.z));
                float x = transform.position.z - startPosition.z;
                grid[(int)x,0] = 1;
                grid[(int)x+1,0] = 1;
               // Debug.Log("Enterd");
            }
            else
            {
                Vector3 startPosition = new Vector3(position.x - 2.0f , position.y +0.1f,
                    position.z +2.1f);
//               Debug.Log(startPosition);
  //             Debug.Log("Df - " +( transform.position.x - startPosition.x));
               float y = transform.position.x - startPosition.x;
               grid[grid.GetLength(0)-1, (int)y] = 1;
               grid[grid.GetLength(0) -1, (int)y +1 ] = 1;
//               Debug.Log("Enterd");
            }
            return grid;
        }
        private void OnDrawGizmos()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Gizmos.color = new UnityEngine.Color(0f,1f,0f,0.3f);
                    Gizmos.DrawCube(transform.position + new Vector3(x,0,y),new Vector3(1,.1f,1));
                }
            }
        }

        
        public XElement GetElement()
        {
            XAttribute x = new XAttribute("x",transform.position.x);
            XAttribute y = new XAttribute("y",transform.position.y);
            XAttribute z = new XAttribute("z",transform.position.z);

            XElement position = new XElement("position",x,y,z);
            
            XAttribute rotation = new XAttribute("rotation", transform.rotation.y);
            XAttribute index = new XAttribute("index", _index);
            
            XElement element = new XElement("door",position,rotation,index);
            return element;
        }
    }
}
