using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FloorTemplate : MonoBehaviour
    {
        [SerializeField] private Image _picture;
        
        [SerializeField] private Button _buyButton;

        private void Start()
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        public void SetImage(string path)
        {
            
            byte[] byteArray = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(byteArray);
                   
            //  GetComponent<Renderer>().material.mainTexture = tex;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
//            Debug.Log(path);
            _picture.sprite = sprite;
        }
        public void AddFunction(UnityAction call)
        {
            _buyButton.onClick.AddListener(call);
//            _picture.gameObject.AddComponent(typeof(Button));
            _picture.GetComponent<Button>().onClick.AddListener(call);
        }
    }
}
