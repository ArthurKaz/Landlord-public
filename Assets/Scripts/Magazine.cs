using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Magazine : MonoBehaviour
    {
        [SerializeField] private List<string> _possibleManName;
        [SerializeField] private List<string> _possibleWomanName;
        [SerializeField] private List<Tenant> _prefabs;

        private static Magazine _object;
        private Tenant _tenant = null;


        public static Tenant GetTenant(int index) => _object._prefabs[index];
        public void Start()
        {
            _object = this;
          //  foreach (int i = 0 );// tenant in _prefabs)
          for (int i = 0; i < _prefabs.Count; i++)
          {

              _prefabs[i].Index = i;
          }
        }
      //  [SerializeField] private List<string> _possibleInfoPart2;
        

        //[SerializeField] private List<PossibleDamage> _possibleDamages;

        public void GenerateTenants()
        {

            try
            {

                Destroy(_tenant.gameObject);
            }
            catch
            {
                // ignored
            }



            _tenant = Instantiate(_prefabs[Random.Range(0, _prefabs.Count)], new Vector3(100, 0, 0),
                new Quaternion(0, 180, 0, 0));
            if (_tenant.Sex)
            {
                string name = _possibleWomanName[Random.Range(0, _possibleWomanName.Count)];
                _tenant.Generate(name);
            }
            else
            {
                string name = _possibleManName[Random.Range(0, _possibleManName.Count)];
                _tenant.Generate(name);
            }

            TenantTemplate.ShowInfo(_tenant,true);
        }





    }
}
