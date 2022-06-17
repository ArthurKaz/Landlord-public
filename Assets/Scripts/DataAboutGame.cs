using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Data", fileName = "Data")]
    public class DataAboutGame : ScriptableObject
    {
        [SerializeField] private int _gameLaunches;

        [SerializeField] private string _lastLaunch ;


        public void AddLastLaunch()
        {
        //    if(_lastLaunch ) _lastLaunch = DateTime.Now;
          //  TimeSpan ts = (TimeSpan) ( DateTime.Now - _lastLaunch);
          Debug.Log(_lastLaunch);
           
           
            _gameLaunches++;
        }
    }
}
