using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Levels
{
    [CreateAssetMenu(menuName = "LevelController", fileName = " Controller")]
    public class LevelController : ScriptableObject
    {
        [SerializeField] private Level _currentLevel;
        [SerializeField] private int _currentExperience;

        [SerializeField] private LevelViewer _levelViewer;

        public LevelViewer LevelViewer
        {
            set => _levelViewer = value;
        }

        
        

        public void AddExperience(int experience)
        {
            if (experience < 0) throw new Exception("Досвід не може бути від'ємним");
            if (_currentLevel.NextLevel == null)
            {
                _levelViewer.Max();
                return;
            }
            _currentExperience += experience;
            
            if (_currentExperience > _currentLevel.Experience)
            {
                NextLevel();
            }
            _levelViewer.Show(_currentLevel.Value,_currentLevel.Experience,_currentExperience);
        }

        private void NextLevel()
        {
            _currentLevel.OnReachTheLevel();
            _currentExperience = _currentExperience - _currentLevel.Experience;
            _currentLevel = _currentLevel.NextLevel;
        }

        public void Update()
        {
            if (_currentLevel.NextLevel == null)
                {
                    _levelViewer.Max();
                
                }
                else _levelViewer.Show(_currentLevel.Value,_currentLevel.Experience,_currentExperience);
        }
    }
}