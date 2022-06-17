using System.IO;
using Assets.Scripts.Floor;
using Assets.Scripts.Room;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GenerateFloors : MonoBehaviour
    {
        [SerializeField] private Room.CommonRoom _commonRoomPrefab;
        [SerializeField] private Room.MiddleRoom _middleRoomPrefab;
        
        [SerializeField] public GameObject _buyButton;
        [SerializeField] private Text _price;
        [SerializeField] public  GameObject _myList;
        [SerializeField] public  GameObject _catalog;

        
        private Floor.Floor _tempFloor = null;

        private void Start()
        {
            RandomGeneration();
           Floor.Floor.clearListeners();
           Floor.Floor.buy += Player.AddFloor;
        }

        public void RandomGeneration()
        {
            _buyButton.SetActive(true);
            if (_tempFloor != null)
            {
                Destroy(_tempFloor/*.GetComponent<GameObject>()*/.gameObject);
                _tempFloor = null;
            }

            switch (Random.Range(0,2))
            {
                case 0:
                    _tempFloor = new GameObject().AddComponent<MiddleFloor>();
                    _tempFloor.Generation(_middleRoomPrefab);
                    _tempFloor.name = "MiddleFloor";
                    break;
                case 1:
                    _tempFloor = new GameObject().AddComponent<CommonFloor>();
                    _tempFloor.Generation(_commonRoomPrefab);
                    _tempFloor.name = "CommonFloor";
                    break;
                
            }

            _price.text = "Ціна - " + _tempFloor.Price;
            
        }

        public void BuyFloor()
        {
            Debug.Log("Call");
            _buyButton.SetActive(false);
            _tempFloor.BuyFloor();
            
        }
        
    }
}
