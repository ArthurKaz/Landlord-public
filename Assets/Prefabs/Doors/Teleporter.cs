using Assets.Scripts;
using Assets.Scripts.Floor;
using UnityEngine;

namespace Assets.Prefabs.Doors
{
    public class Teleporter : MonoBehaviour
    {
        public Teleporter Other;

        public bool Teleported = false;
        public Vector2 vect;
        
        private Collider _last;

        public void Wrap()
        {
            float temp = vect.x * -1; 
            vect.x = vect.y * -1;
            vect.y = temp;
        }

       
        private void  OnTriggerStay(Collider other)
        {
            if (_last != null && _last != other) Teleported = false;
        
            if (!Teleported)
            {
            
                Teleported = true;
                Other.Teleported = false;
                Teleport(other.transform);
                _last = other;

            }
        }

        private void Teleport(Transform obj)
        {
            Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint3x4(obj.position);
            localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);
            Vector3 newPosition =  Other.transform.localToWorldMatrix.MultiplyPoint3x4(localPos);
            obj.position = new Vector3(newPosition.x +vect.x,newPosition.y,newPosition.z+vect.y);

            Quaternion difference = Other.transform.rotation * Quaternion.Inverse(transform.rotation * Quaternion.Euler(0, 180, 0));
            obj.rotation = difference * obj.rotation;
        }
    }
}
