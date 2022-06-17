using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TenantTemplate : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _button;

        [SerializeField] private GameObject _showInfo;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Text _name;
        [SerializeField] private Text _info;

        [SerializeField] private GameObject _male;
        [SerializeField] private GameObject _female;

        [SerializeField]private Button _toRentButton;
        private Floor.Floor _floor;
        private Tenant _tenant = null;
        private static TenantTemplate _object;

        public static GameObject Button => _object._button;

        public static Floor.Floor Floor
        {
            get => _object._floor;
            set => _object._floor = value;
        }

        private void Start()
        {
            _object = this;
            _object._panel.SetActive(false);
            
        }

        public static void HideButton()
        {
            _object._button.SetActive(false);
        }
        public static void ShowButton()
        {
            //_object._floor?.Unsubscribe();
            _object._button.SetActive(true);

        }
        public  void Hide()
        {
           Destroy(_tenant.gameObject);
       // _tenant.gameObject.SetActive(false);
            _object._floor.Subscribe();
            _object._panel.SetActive(false);
            _object._canvas.gameObject.SetActive(true);
        }

        public static void ShowInfo(Tenant tenant,bool toRentButton)
        {
            
            if(!_object._floor.ThereIsAllTheFurniture()) return;
            
            _object._canvas.gameObject.SetActive(false);
                
            if (_object._tenant != null)
            {
                Destroy(_object._tenant.gameObject);
                _object._tenant = null;
            }
            
            if(!toRentButton){tenant = Instantiate(tenant, new Vector3(100, 0, 0),
                new Quaternion(0, 180, 0, 0)).Load(tenant);}
            
            tenant.gameObject.SetActive(true);
            _object._tenant = tenant;
            _object._floor.Unsubscribe();
            _object._panel.SetActive(true);
            _object._name.text = tenant.Name;
       //     Debug.Log(tenant.Name);
            _object._info.text = tenant.Info;
            if (tenant.Sex)
            {
                _object._male.SetActive(false);
                _object._female.SetActive(true);
            }
            else
            {
                _object._male.SetActive(true);
                _object._female.SetActive(false);
            }
            _object._toRentButton.gameObject.SetActive(toRentButton);
        }

      /*  public static void ShowInfo(Tenant tenant)
        {
            _object._tenant = tenant;
            _object._floor.Unsubscribe();
            _object._panel.SetActive(true);
            _object._name.text = tenant.Name;
            _object._info.text = tenant.Info;
            if (tenant.Sex)
            {
                _object._male.SetActive(false);
                _object._female.SetActive(true);
            }
            else
            {
                
                _object._male.SetActive(true);
                _object._female.SetActive(false);
            }
        }*/

        public void ToRent()
        {
            _button.SetActive(false);
            _floor.SetTenant(_object._tenant);
            
            Debug.Log(_floor.Tenant);
            Hide();
            Player.SaveAll();
        }
        

    }
}
