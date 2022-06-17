using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Net creator", typeof(CustomDeck))]
public class NetCreator : EditorTool
{
    private CustomDeck _customSnap;
    [SerializeField] private Texture2D _toolIcon;
    
    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = _toolIcon,
                text = "Custom Snap Move Tool",
                tooltip = "Custom Snap Move Tool - best tool ever"
            };
        }
    }
    
    private void OnEnable()
    {
        _customSnap = (CustomDeck ) target;
      
    }

    public override void OnToolGUI(EditorWindow window)
    {
        const float MIN_SIZE = 1.0f;
        const float MAX_SIZE = 10.0f;
        Vector3 scale =  Handles.ScaleHandle(_customSnap.GizmosScale, _customSnap.transform.position, Quaternion.identity,1);
        if (scale.x < MIN_SIZE) scale.x = MIN_SIZE;
        if (scale.z < MIN_SIZE) scale.z = MIN_SIZE;
        
        if (scale.x > MAX_SIZE) scale.x = MAX_SIZE;
        if (scale.z > MAX_SIZE) scale.z = MAX_SIZE;
         _customSnap.LoadNet((int)scale.x,(int) scale.z);
    }
    
    
}
