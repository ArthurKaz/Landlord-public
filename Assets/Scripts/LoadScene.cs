using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Floor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
   
    
    [SerializeField]private AsyncOperation _asyncOperation;

    [SerializeField] private Image _loadBar;

    [SerializeField] private Text _textBar;

    [SerializeField] private int SceneID;

    private void Start()
    {
       // StartCoroutine(LoadSceneCor());
    }

    public  void OpenScene()
    {
        SceneTransition.SwitchToScene(SceneID);
    }

 
    
  /*  IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        _asyncOperation = SceneManager.LoadSceneAsync(SceneID);
        while (!_asyncOperation.isDone)
        {
            float progres = _asyncOperation.progress / 0.9f;
            _loadBar.fillAmount = progres;
            _textBar.text = "Loading  " + Math.Round(progres * 100f);
            yield return 0;
        }
    }*/
   
}
