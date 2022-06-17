using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
   public class Window : MonoBehaviour
   {
      [SerializeField] private int _index;
      private Vector3 _position;
      private Quaternion _rotation;

      public void SetIndex(int value)
      {
         _index = value;
      }

      public void Load(Vector3 position, Quaternion rotation)
      {
         _position = position;
         _rotation = rotation;
         Instantiate(this, _position, _rotation);
      }

      public void Spawn(Vector3 minPosition,Vector3 maxPosition,Quaternion rotation)
      {
         _rotation = rotation;
         _position = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z));
         transform.position = _position;
         transform.rotation = _rotation;
      }

      public XElement GetElement()
      {
         XAttribute x = new XAttribute("x",transform.position.x);
         XAttribute y = new XAttribute("y",transform.position.y);
         XAttribute z = new XAttribute("z",transform.position.z);

         XElement position = new XElement("position",x,y,z);
            
         XAttribute rotation = new XAttribute("rotation", transform.rotation.y);
         XAttribute index = new XAttribute("index", _index);
            
         XElement element = new XElement("window",position,rotation,index);
         return element;
      }
   }
}
