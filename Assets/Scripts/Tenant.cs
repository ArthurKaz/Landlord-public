using System;
using System.Xml.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Tenant : MonoBehaviour
    {
      
        private string _name;
        [SerializeField] private Job _job;
        private int _age;
        private PossibleDamage _possibleDamage;
        [SerializeField]private bool _sex;// => Sex;
       [SerializeField]private int _index;


        public string Name => _name;

        public bool Sex => _sex;

        public string Info => "Робота - " + GetJob(_job) + ", " + _age + MyClass.ToStringAge(_age) + ". " +
                                  _possibleDamage.Info;
        
        public Job Work => _job;

    
        public int Index
        {
            get => _index;
            set => _index = value;
        }
        
        public XElement GetElement()
        {
            XElement index = new XElement("Index", Index);

            XAttribute name = new XAttribute("Name", _name);
            //XAttribute job = new XAttribute("Job", (int) _job);
            XAttribute age = new XAttribute("Age", _age);
            XElement possibleDamge = _possibleDamage.GetElement();
            return new XElement("Tenant", index, name, age, possibleDamge);
        }
        

        //private set => _name = value;
        public Tenant Generate(string name)
        {
            
            _name = name;
            _age = Random.Range(18, 39);
           // _job = (Job) Random.Range(0,  19);
            // _job = Job.Chef;
//            Debug.Log(_job);
            Damage level = (Damage) Random.Range(0, 4);
            _possibleDamage = new PossibleDamage(level);
           
            return this;
            // _possibleDamage = GetPossibleDamage((Damage)  Random.Range(0, 4));
            /*string info = "Робота - "+_possibleWork[Random.Range(0, _possibleWork.Count)]+", ";
          
            info += age + MyClass.ToStringAge(age) + ", ";
            info += "Хоббі - " + _possibleHobby[Random.Range(0, _possibleHobby.Count)]+". ";
            info += GetInfo(level);
              
            Damage level = (Damage) Random.Range(0, 4);

           
            PossibleDamage possibleDamage = GetPossibleDamage(level);*/
        }
      

       
        public enum Job
        {
            Scientist = 0,
            Reporter, 
            RaceDriver,
            Post,
            Police, 
            Plumber, 
            Pilot,
            Paramedic,
            Ninja,
            NavalOfficer,
            Mechanic,
            Fire,
            Farm,
            Explorer,
            Doctor,
            Dentist,
            ConstructionWorker,
            Chef,
            Carpenter  
        }

        private string GetJob(Job job)
        {
            switch (job)
            {
                case Job.Scientist: return "Ноуковець";
                case Job.Reporter: return "Репортер";
                case Job.RaceDriver: return "Гонщик";
                case Job.Post: return "Листоноша";
                case Job.Police: return "Поліцейський";
                case Job.Plumber: return "Сантехнік";
                case Job.Pilot: return "Льотчик";
                case Job.Paramedic: return "Фельдшер";
                case Job.Ninja: return "Ніндзя";
                case Job.NavalOfficer: return "Військово-морський офіцер";
                case Job.Mechanic: return "Механік";
                case Job.Fire: return "Пожежнкик";
                case Job.Farm: return "Фермер";
                case Job.Explorer: return "Дослідник";
                case Job.Doctor: return "Лікар";
                case Job.Dentist: return "Стоматолог";
                case Job.ConstructionWorker: return "Будівельник";
                case Job.Chef: return "Кухар";
                case Job.Carpenter: return "Столяр";
                
            }
            throw new Exception("Іноформація не вибрана");
        }


        public void Load(XElement tenant)
        {
            _name = tenant.Attribute("Name").Value;
            _age = int.Parse(tenant.Attribute("Age").Value);

            _possibleDamage = new PossibleDamage(tenant.Element("PossibleDamage"));
           // Instantiate(this);
        }

        public Tenant Load(Tenant objTenant)
        {
            _name = objTenant._name;
            _age = objTenant._age;
            _possibleDamage = objTenant._possibleDamage;
            return this;
        }
    }
}
