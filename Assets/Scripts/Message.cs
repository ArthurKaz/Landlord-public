using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;

        [SerializeField] private Text _title;
        [SerializeField] private Text _info;
        [SerializeField] private Button _button;
        
        
        private static Message _message;

        private void Start()
        {
            _message = this;
            Hide();
        }

        public static void ShowMessage(string title,string info)
        {
            _message._panel.SetActive(true);
            _message._title.text = title;
            _message._info.text = info;
        }
        
        public void Hide()
        {
            _message._panel.SetActive(false);
        }
    }
}
