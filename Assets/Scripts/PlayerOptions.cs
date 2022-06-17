using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Options", fileName = "Player Options")]
public class PlayerOptions : ScriptableObject
{
    private int _maxFloors = 1;
    private int _theProbabilityOfFindingNeatPeople = 5;
    private int _discountOnThePriceOfFloors = 0;
    private int _discountOnThePriceOfFurnitures = 0;
    

    public int MaxFloors
    {
        get => _maxFloors;
        private set
        {
            if (value < 0) throw new Exception("Max floors connot be negative");
            if (value < 50) throw new Exception("Too many floors");
            _maxFloors = value;
        }
    }

    public void AddMaxFloors(int floors)
    {
        if (floors < 0) throw new Exception("Max floors connot be negative");
        _maxFloors += floors;
        Debug.Log(_maxFloors);
    }
}
