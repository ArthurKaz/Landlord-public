using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FurnitureTemplate : MonoBehaviour
    {

        [SerializeField] private Text _price;
        [SerializeField] private Image _picture;
        
        [SerializeField] private Button _buyButton;
        
        

        public void SetImage(Sprite sprite)
        {
            _picture.sprite = sprite;
        }
        public void AddFunction(UnityAction call)
        {
            _buyButton.onClick.AddListener(call);
        }


        public void SetPrice(int price)
        {
            _price.text = price.ToString();
        }
    }
}
