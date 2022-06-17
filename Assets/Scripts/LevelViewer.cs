using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelViewer : MonoBehaviour
{
    
    
    [SerializeField] private Text _level;
    [SerializeField] private Image _experience;
    public void Max()
    {
        _level.text = "Max";
        _experience.fillAmount = 1.0f;
    }

    public void Show(int Level,int Experience, int currentExperience)
    {
        _level.text = Level.ToString();
        _experience.fillAmount = (float) currentExperience / Experience;
    }
}
