using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using CustomSnapPoint_1 = CustomSnapPoint;

[EditorTool("Custom Snap Move", typeof(Custom))]
public class CustomSnapPointTool : EditorTool
{
   [SerializeField] private Texture2D _toolIcon;

   private Transform _oldTarget;
   private CustomSnapPoint[] _allPoints;
   private CustomSnapPoint[] _targetPoints;

   private void OnEnable()
   {
      Debug.Log("Enabled");
      
   }

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

   public override void OnToolGUI(EditorWindow window)
   {
      Transform targetTransform = ((Custom) target).transform;

      if (targetTransform != _oldTarget)
      {
       _allPoints = FindObjectsOfType<CustomSnapPoint>();
      _targetPoints = targetTransform.GetComponentsInChildren<CustomSnapPoint>();
      _oldTarget = targetTransform;
      }

      EditorGUI.BeginChangeCheck();
      Vector3 newPosition = Handles.PositionHandle(targetTransform.position, Quaternion.identity);

     // Vector3 test = Handles.ScaleHandle(targetTransform.localScale, targetTransform.position, Quaternion.identity, 1);
    //  Vector3 newScale = new Vector3(int.Parse(test.y.ToString()), int.Parse(test.x.ToString()),
    //     int.Parse(test.x.ToString()));
    /*int x = (int) test.x;
      Debug.Log(x);*/

      if (EditorGUI.EndChangeCheck())
      {
         Undo.RecordObject(targetTransform, "Move with snap tool");
         newPosition.y = 0.0f;
         MoveWithSnapping(targetTransform,newPosition);
      }
      /*Transform targetTransform = ((CustomSnap) target).transform;

      if (targetTransform != _oldTarget)
      {
         PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(targetTransform.gameObject);

         if (prefabStage != null)
            _allPoints = prefabStage.prefabContentsRoot.GetComponentsInChildren<CustomSnapPoint>();
         else
            _allPoints = FindObjectsOfType<CustomSnapPoint>();
            
         _targetPoints = targetTransform.GetComponentsInChildren<CustomSnapPoint>();

         _oldTarget = targetTransform;
      }

      EditorGUI.BeginChangeCheck();
      Vector3 newPosition = Handles.PositionHandle(targetTransform.position, Quaternion.identity);

      if (EditorGUI.EndChangeCheck())
      {
         Undo.RecordObject(targetTransform, "Move with custom snap tool");

        // if (((CustomSnap) target).IsGrounded) newPosition.y = 0;
            
         MoveWithSnapping(targetTransform, newPosition);
      }*/
   }

   private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
   {
  
      
      
      Vector3 bestPosition = newPosition;
      float closesDistance = float.PositiveInfinity;


     
      foreach (CustomSnapPoint point in _allPoints)
      {
         if (point.transform.parent == targetTransform) continue;
         
         foreach (CustomSnapPoint ownPoint in _targetPoints)
         {
            if(point.CanConnect(ownPoint.Type)) continue;
            Vector3 targetPos = point.transform.position - (ownPoint.transform.position - targetTransform.position);
          //  targetPos.y = 0.0f;
            
          //Vector3 targetPos = ownPoint.transform.position - ( point.transform.position- targetTransform.position);
          //Vector3 targetPos = point.transform.position - (ownPoint.transform.position - point.transform.position);
        
            float distance = Vector3.Distance(targetPos, newPosition);
            
            if (distance < closesDistance)
            {
               closesDistance = distance;
               bestPosition = targetPos;
            }
         }
      }
      if (closesDistance < 3f)
      {
        
         targetTransform.position = bestPosition;
      }
      else
      {
         targetTransform.position = newPosition;
      }
     
   }
}