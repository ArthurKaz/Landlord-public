using System;
using Assets.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public  class Pocket : MonoBehaviour
    {
        [SerializeField] private Text _balanceText;
        [SerializeField] private LevelController _levelControler;
        [SerializeField] private LevelViewer _levelViewer;
      
        private Pocket _object;

        private void Start()
        {
            if (_balanceText==null) throw new Exception("Balance: не встановлено силку на текст");
            if(_levelControler == null )throw new Exception("LevelController: не встановлено силку на контролер");
            _object = this;
            _object._balanceText.text = Money.Balance.ToString();
            _levelControler.LevelViewer = _levelViewer;
            _levelControler.Update();
        }
        
        
    }

   
}
