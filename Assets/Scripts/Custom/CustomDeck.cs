using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDeck : Custom
{
    [SerializeField] private CustomSnapPoint _pointPrefab;
    
    
    [SerializeField] private Vector3 _gizmosScale = new Vector3(3,0.05f,3);

    [SerializeField]private List<CustomSnapPoint> _customSnapPoints = new List<CustomSnapPoint>();
    public Vector3 GizmosScale => _gizmosScale;
    public void LoadNet(int x, int z)
    {
        if ((int) _gizmosScale.x != x || (int) _gizmosScale.z != z)
        {
            GenerateNet(x, z);
            _gizmosScale = new Vector3(x ,0.05f,z);
        }
    }

    private void GenerateNet(int x, int z)
    {
        ClearCustomSnapPoints();
        const float TOP = 0.025f;
        const float COEFFICIENT = 0.6f;
        const float CELL_SIZE = 1.2f;
        
        Vector3 rightPointPosition = transform.position;
        rightPointPosition.y = TOP;
        rightPointPosition.z += (z * CELL_SIZE / 2) + 0.1f;
        rightPointPosition.x -= x * COEFFICIENT;
        
        Vector3 leftPointPosition = transform.position;
        leftPointPosition.y = TOP;
        leftPointPosition.z -= (z * CELL_SIZE / 2) + 0.1f;
        leftPointPosition.x -= x * COEFFICIENT;
        
        
        for (int i = 0; i <= x; i++)
        {
            _customSnapPoints.Add(Instantiate(_pointPrefab, rightPointPosition, Quaternion.identity));
            _customSnapPoints.Add(Instantiate(_pointPrefab, leftPointPosition, Quaternion.identity));
            rightPointPosition.x += 1.2f;
            leftPointPosition.x += 1.2f;
        }
        
        
        Vector3 behindPointPosition = transform.position;
        behindPointPosition.y = TOP;
        behindPointPosition.x += (x * CELL_SIZE / 2) + 0.1f;
        behindPointPosition.z += z * COEFFICIENT;
        
        Vector3 aheadPointPosition = transform.position;
        aheadPointPosition.y = TOP;
        aheadPointPosition.x -= (x * CELL_SIZE / 2) + 0.1f;
        aheadPointPosition.z += z * COEFFICIENT;

        for (int i = 0; i <= z; i++)
        {
            
            _customSnapPoints.Add(Instantiate(_pointPrefab, behindPointPosition, Quaternion.identity));
            _customSnapPoints.Add(Instantiate(_pointPrefab, aheadPointPosition, Quaternion.identity));
            behindPointPosition.z -= CELL_SIZE;
            aheadPointPosition.z -= CELL_SIZE;
        }

        foreach (var customSnapPoint in _customSnapPoints)
        {
            customSnapPoint.transform.SetParent(transform);
        }
    }

    private void Start()
    {
        ClearCustomSnapPoints();
    }

    private void ClearCustomSnapPoints()
    {
        Debug.Log(_customSnapPoints.Count);
        foreach (CustomSnapPoint snapPoint in _customSnapPoints)
        {
            if (snapPoint == null)
            {
                _customSnapPoints.Remove(snapPoint);
            }
            GameObject.DestroyImmediate(snapPoint.gameObject);
           
        }
        _customSnapPoints.Clear();
    }

    private void OnDrawGizmos()
    {
        /*_gizmosScale = new Vector3(6,0.2f,4.8f);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position,_gizmosScale);*/
        Gizmos.color = new Color(2.55f, 2.35f, 0.45f);//Color.green;
        Vector3 gizmosPosition = transform.position;
        gizmosPosition.y = gizmosPosition.y+0.05f;
        Gizmos.DrawCube(gizmosPosition,new Vector3(_gizmosScale.x * 1.2f, _gizmosScale.y,_gizmosScale.z* 1.2f));
        //transform.localScale = _gizmosScale;
    }
}
