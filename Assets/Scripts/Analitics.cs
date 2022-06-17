using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Analitics : MonoBehaviour
{
    [SerializeField] private DataAboutGame _dataAboutGame; 
    private void Start()
    {
        _dataAboutGame.AddLastLaunch();
    }
}
