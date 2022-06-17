using System;
using System.Collections.Generic;
using Assets.Scripts.Room;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Prefabs : MonoBehaviour
    {
        [SerializeField] private Window[] _windows;

        [SerializeField] private Door[] _doors;

        [SerializeField] private Material[] _bedroomWalls;
        [SerializeField] private Material[] _bedroomFloors;

        [SerializeField] private Material[] _kitchenWalls;
        [SerializeField] private Material[] _kitchenFloors;

        [SerializeField] private Material[] _bathroomWalls;
        [SerializeField] private Material[] _bathroomFloors;
        
        [SerializeField] private Material[] _playroomWalls;
        [SerializeField] private Material[] _playroomFloors;

        [SerializeField] private Material[]   _livingRoomWalls;
        [SerializeField] private Material[]   _livingRoomFloors;
        
        
        
        
        
        
      
        private void Start()
        {
            for (int i = 0; i < _doors.Length; i++)
            {
                _doors[i].SetIndex(i);
            }

            for (int i = 0; i < _windows.Length; i++)
            {
                _windows[i].SetIndex(i);
            }
        }

     

        public Window RandomWindow()
        {
            return _windows[Random.Range(0, _windows.Length)];
        }

        public Door GetDoor(int value)
        {
            // throw new ArgumentException("Такого елемента немає");
//            Debug.Log("Index - " +value+"All - "+_doors.Length);
            if (value == _doors.Length)return _doors[value - 1];
            return _doors[value ];
        }
        public Window GetWindow(int value)
        {
            return _windows[value];
        }
        public Door RandomDoor()
        {
            return _doors[Random.Range(0, _doors.Length)];
        }

        public int RandomBedroomWall()
        {
            return (int) Random.Range(0, _bedroomWalls.Length);
        }

        public int RandomBedroomFloor()
        {
            return Random.Range(0, _bedroomFloors.Length);
        }

        public int RandomKitchenWall()
        {
            return Random.Range(0, _kitchenFloors.Length);
        }
        public int RandomKitchenFloor()
        {
            return Random.Range(0, _kitchenFloors.Length);
        }

        public int RandomBathroomWall()
        {
            return Random.Range(0, _bathroomWalls.Length);
        }
        public int RandomBathroomFloor()
        {
            return Random.Range(0, _bathroomFloors.Length);
        }

        public int RandomPlayroomWall()
        {
            return Random.Range(0, _playroomWalls.Length);
        }
        
        public int RandomPlayroomFloor()
        {
            return Random.Range(0, _playroomFloors.Length);
        }
        
        public int RandomLivingRoomWall()
        {
            return Random.Range(0, _livingRoomWalls.Length);
        }
        
        public int RandomLivingRoomFloor()
        {
            return Random.Range(0, _livingRoomFloors.Length);
        }

        public Material GetBathroomFloor(int index)
        {
            return _bathroomFloors[index];
        }

        public Material GetBathroomWall(int index)
        {
            return _bathroomWalls[index];
        }
        
        public Material GetBedroomFloor(int index)
        {
            return _bedroomFloors[index];
        }

        public Material GetBedroomWall(int index)
        {
            return _bedroomWalls[index];
        }
        
        public Material GetKitchenFloor(int index)
        {
            return _kitchenFloors[index];
        }

        public Material GetKitchenWall(int index)
        {
            //Debug.Log(index);
            return _kitchenWalls[index];
        }
        
        public Material GetPlayroomWall(int index)
        {
            return _playroomWalls[index];
        }
        public Material GetPlayroomFloor(int index)
        {
            return _playroomFloors[index];
        }
        
        public Material GetLivingRoomWall(int index)
        {
            return _livingRoomWalls[index];
        }
        public Material GetLivingRoomFloor(int index)
        {
            return _livingRoomFloors[index];
        }
    }
}
