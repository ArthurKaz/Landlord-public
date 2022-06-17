using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public abstract class Room : MonoBehaviour
    {
        [SerializeField] protected List<Furniture> Furniture = new List<Furniture>();
        [SerializeField] protected  Window  Window;
        [SerializeField] protected Door Door;
        [SerializeField] protected int Walls;
        [SerializeField] protected int Floor;
        [SerializeField] protected TypeRoom TypeRoom;
        
        public Furniture[] Furnitures => Furniture.ToArray();

        public abstract void Spawn(Direct direct,Window window,Door door,TypeRoom type);
        public abstract TypeRoom GetTypeRoom();

        public abstract int[,] GetGrid();
        
        public abstract void SetChildren();
        public abstract void AddFurniture(Furniture newFurniture);
        public abstract void Spawn(XElement room);

        public abstract XElement GetElement();

        public void GetMoney(PossibleDamage possibleDamage)
        {
            foreach (Furniture furniture in Furniture)
            {
                furniture.ReceiveDamage(possibleDamage.ReceiveDamage());
            }
        }
    }
}


