using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Task", fileName = "new Task")]
    public class Task : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private string _revard;
    
    }
}
