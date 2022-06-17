using System;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FurnitureInfo : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _price;
        [SerializeField] private Text _condition;

        [SerializeField] private Button _repairForAdvertising;
        [SerializeField] private Button _repairForMoney;
        
        private static FurnitureInfo _object;

        public static event EventHandler LockFurniture;
        public static event EventHandler UnLockFurniture;
        //public event EventHandle LockFurnitures;


        public void Back()
        {
            //    _repairForMoney.onClick.RemoveAllListeners();
            OnUnLockFurniture();
            _object.gameObject.SetActive(false);
            _object._repairForAdvertising.gameObject.SetActive(false);
            _object._repairForMoney.gameObject.SetActive(false);
        }

        public void Awake()
        {
            _object = this;
            _object.gameObject.SetActive(false);
            _object._repairForAdvertising.gameObject.SetActive(false);
            _object._repairForMoney.gameObject.SetActive(false);
        }
        public static void ShowInfo(Furniture furniture)
        {
            OnLockFurniture();
            _object._repairForMoney.onClick.RemoveAllListeners();
            _object._image.sprite = furniture.GetImage();
            furniture.ShowInfo(_object._condition,_object._price,_object._image,_object._repairForAdvertising,_object._repairForMoney);
            _object.gameObject.SetActive(true);
        }

        public static void AddFunction(UnityAction action)
        {
            _object._repairForMoney.onClick.AddListener(action);
        }

        public static void OnLockFurniture()
        {
            LockFurniture?.Invoke(_object, EventArgs.Empty);
        }

        public static void OnUnLockFurniture()
        {
            UnLockFurniture?.Invoke(null, EventArgs.Empty);
        }
    }
}
