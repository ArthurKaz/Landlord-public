using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Floor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject _minuteHand;
    [SerializeField] private GameObject _secondHand;
    [SerializeField] private Text _indicator;

     private float _minute;
     private float _second;

     private static Clock _object;
     private static Animator _componentAnimator;

     private static UnityEvent _endTimerEvent = new UnityEvent();


    private float Minute
    {
        get => _minute;
        set
        {
            float rotationDegreePerMinute = 15;
            _minuteHand.transform.eulerAngles = new Vector3(0, 0, value * rotationDegreePerMinute);
          //  Debug.Log(_minuteHand.transform.eulerAngles.z +"min - " + rotationDegreePerMinute);
            _minute = value;
        }
    }

    private float Second
    {
        get => _second;
        set
        {
            float rotationDegreePerSecond = 6;
            _secondHand.transform.eulerAngles = new Vector3(0, 0, value * rotationDegreePerSecond);
            _indicator.text = (_minute < 10? "0" + _minute : _minute.ToString()) + ":" + (value < 10? "0" + value : value.ToString());
            _second = value;
        }
    }

    private void Start()
    {
        //_object_secondHand
        if (_minuteHand == null) throw new UnassignedReferenceException("Префаб вказівника на годинник не встановлено");
        if (_secondHand == null) throw new UnassignedReferenceException("Префаб вказівника на секунду не встановлено");
        if (_indicator == null) throw new UnassignedReferenceException("Префаб вказівника на показник не встановлено");
        _object = this;
        _componentAnimator = _object.GetComponent<Animator>();
    }

    public static void StartTimer(int second, int minute,UnityAction call)
    {
        if (second < 0 || second > 60) throw new IOException("Некоректно введено секунди");
        if (minute< 0 || minute > 24) throw new IOException("Некоректно введено хвилини");
        
        _object.Minute = minute;
        _object.Second = second;
        
        StartTimer();
        _endTimerEvent.AddListener(call);
    }
    public static void StartTimer(int seconds, UnityAction call)
    {
        if (seconds< 0 ) throw new IOException("Некоректно введено секунди");
        _object._minute = seconds / 60;
        _object._second = seconds - _object._minute * 60;
        
        StartTimer();
        _endTimerEvent.AddListener(call);
    }
    private static void StartTimer()
    {
        _object.StopAllCoroutines();
        _object.StartCoroutine(ExecuteAfterTime());
        _componentAnimator.SetBool("show",true);
    }

    private static IEnumerator ExecuteAfterTime()
    {
        while (_object.Minute > 0 || _object.Second > 0 )
        {

            if (_object.Second <= 0)
            {
                _object.Second = 60;
                _object.Minute--;
            }
            
            _object.Second--;
            yield return new WaitForSeconds(1);
        }
        _endTimerEvent?.Invoke();
    }

    public void Show()
    {
        _componentAnimator.SetBool("show",true);
    }


    public static void Hide()
    {
        _componentAnimator.SetBool("show",false);
    }
}
