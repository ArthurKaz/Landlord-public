using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
   public class Furniture : MonoBehaviour
   {
      [SerializeField] private int _index;
      
      [SerializeField] private int _turns;

      [SerializeField] private Sprite _sprite;

      public TypeRoom TypeRoom;
      public Vector2Int Size = Vector2Int.one;
      public GameObject Rotation;

      [SerializeField] private int _condition = 100;
      [SerializeField] private int _price;

      public int Price => _price;

      private bool _canOpenShowInfo = true;

      public void Start()
      {
         FurnitureInfo.LockFurniture += LockOpenSHowInfo;
         FurnitureInfo.UnLockFurniture += UnLockOpenSHowInfo;
      }

      private void LockOpenSHowInfo(object sender, EventArgs e)
      { 
         _canOpenShowInfo = false;
      }
      private void UnLockOpenSHowInfo(object sender, EventArgs e)
      { 
         _canOpenShowInfo = true;
      }
      
      
     // [SerializeField] private int _point;


     public void SetCondition(int value)
     {
        
        _condition = value;
     }
      public void SetIndex(int value)
      {
         _index = value;
      }

      public Sprite GetImage()
      {
         return _sprite;
      }
      public XElement GetElement()
      {

         XAttribute index = new XAttribute("index", _index);
         XAttribute turns = new XAttribute("turns", _turns);
         XAttribute condition = new XAttribute("condition", _condition);
  //       Debug.Log(condition);

         XAttribute x = new XAttribute("x", transform.position.x);
         XAttribute y = new XAttribute("y", transform.position.y);
         XAttribute z = new XAttribute("z", transform.position.z);

         XElement position = new XElement("position", x, y, z);

         XElement element = new XElement("furniture", index, turns, position,condition);
//         Debug.Log(element);
         return element;
      }

      public void Wrap()
      {
         int tmp = Size.x;
         Size.x = Size.y;
         Size.y = tmp;

         Rotation.transform.rotation *=
            Quaternion.Euler(Rotation.transform.rotation.x, 90, Rotation.transform.rotation.x);

         _turns++;
         if (_turns >= 4) _turns = 0;
         float x = Rotation.transform.localPosition.z;
         float y = Rotation.transform.localPosition.x;
         Rotation.transform.localPosition = new Vector3(x, Rotation.transform.localPosition.y, y);
      }

      private void OnDrawGizmos()
      {
         for (int x = 0; x < Size.x; x++)
         {
            for (int y = 0; y < Size.y; y++)
            {
               Gizmos.color = new UnityEngine.Color(0f, 1f, 0f, 0.3f);
               Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
         }
      }

      public void ReceiveDamage(int damage)
      {
         _condition -= damage;
      }
      
      public void OnMouseDown()
      {
//         Debug.Log("this - " + this);
         if(_canOpenShowInfo) FurnitureInfo.ShowInfo(this);
      }

      public void ShowInfo(Text condition, Text price, Image sprite,Button repairForAdvertising,Button repairForMoney)
      {
         condition.text =_condition.ToString();
         
         double temp = -1*(Math.Pow(_condition, 1.268f)) + Math.Pow(100, 1.268f);
         
         

         price.text =  Math.Round(_price * (temp/100)).ToString();
         sprite.sprite = _sprite;


         if (_condition < 100)
         {
            repairForMoney.gameObject.SetActive(true);
            repairForAdvertising.gameObject.SetActive(true);
            int priceForRepair = int.Parse(Math.Round(_price * (temp / 100)).ToString());
            repairForMoney.onClick.AddListener(delegate { Repair(priceForRepair,price,condition);});
            repairForMoney.onClick.AddListener(Player.SaveAll);
         }
         else
         {
            repairForMoney.gameObject.SetActive(false);
            repairForAdvertising.gameObject.SetActive(false);
         }
      }

      public void Repair(int price,Text priceText,Text condition)
      {
         if (Player.SpendMoney(price))
         {
            _condition = 100;
            priceText.text = "0";
            condition.text = _condition.ToString();
         }
         else
         {
            Debug.Log("No money");
         }
      }
      

   }
}
