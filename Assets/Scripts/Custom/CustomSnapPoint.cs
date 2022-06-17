using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class CustomSnapPoint : MonoBehaviour
{

    
    [SerializeField] private ConnectionType _type;
    [SerializeField] private ConnectionType[] _canConnection;
     
    public ConnectionType Type => _type;
    
    
    private void OnDrawGizmos()
    {
        const float RADIUS = 0.1f;
        switch (_type)
        {
            case ConnectionType.Deck:
                Gizmos.color = Color.green;
                break;
            case ConnectionType.Wall:
                Gizmos.color = Color.gray;
                break;
            case ConnectionType.PartForDoor:
                Gizmos.color = Color.red;
                break;
            case ConnectionType.PartForWindow:
                Gizmos.color = Color.blue;
                break;
            case ConnectionType.WallSide:
                Gizmos.color = new Color(1, 1, 1);
                break;
            
        }
        
        Gizmos.DrawSphere(transform.position, RADIUS);
    }
    
    public enum ConnectionType
    {
        Deck = 0,
        Wall = 1 ,
        PartForWindow,
        PartForDoor,
        WallSide
    }

    public bool CanConnect(ConnectionType type)
    {
        foreach (ConnectionType t in _canConnection)
        {
            if (type == t) return false;
        }

        return true;
    }
}
