using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class SetColor : MonoBehaviour
{
   [SerializeField] private Color _color;
   [SerializeField] private Color _previousColor;

   [SerializeField] private Image _image;

   private void Start()
   {
      _image.color = _previousColor;
   }
   public void ChangeColor()
   {
      _image.color = _color;
   }

   public void BackColor()
   {
      _image.color = _previousColor;
   }
   
}
