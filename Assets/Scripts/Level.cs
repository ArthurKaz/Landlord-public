using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Levels", fileName = "new level")]
public class Level : ScriptableObject
{
    public int Value => _value;
    public int Experience => _experience;
    public Level NextLevel => _nextLevel;
    
    [SerializeField] private  int _value;
    [SerializeField] private int _experience;

    [SerializeField]
    private UnityEvent _reachTheLevel;
    
    [SerializeField] private Level _nextLevel = null;

    public Level(int value, int experience)
    {
        if (value < 0) throw new Exception("Рівень не може бути від'ємним");
        if (experience < 0) throw new Exception("Досвід не можебути від'ємним");
        _value = value;
        _experience = experience;
    }


    public void OnReachTheLevel()
    {
        _reachTheLevel?.Invoke();
    }
}
