using UnityEngine;

namespace Assets.Scripts
{
    public class MoveTo : MonoBehaviour
    {
        private Vector3 _srartPosition;
        private Vector3 _endPosition;
        private float _step = 0.35f;
        private float _progress;

        void Start()
        {
           // if (Application.platform == RuntimePlatform.Android) _step = 0.0045f;
            _srartPosition = transform.position;
            _endPosition = transform.position;
        }
        
        public void Move(Vector3 position)
        {
            _srartPosition = transform.position;
            _endPosition = position;
            _progress = 0;
        }
        
        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(_srartPosition, _endPosition, _progress);//* Time.deltaTime;
            _progress += _step;
        }
    }
    
}
