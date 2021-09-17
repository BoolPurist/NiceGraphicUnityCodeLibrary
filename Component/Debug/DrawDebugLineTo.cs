using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  public class DrawDebugLineTo : MonoBehaviour
  {
    public enum ReferencePoint { Pivot, Center }

    [SerializeField]
    private GameObject _Target;
    [SerializeField]
    private Color _GizmoColor = Color.white;
    [SerializeField]
    private ReferencePoint _PointKind = ReferencePoint.Pivot;


    private void OnDrawGizmosSelected()
    {
      if (_Target != null)
      {
        if (_PointKind == ReferencePoint.Pivot)
        {
          Gizmos.color = _GizmoColor;
          Gizmos.DrawLine(transform.position, _Target.transform.position);
        }
        else
        {
          Vector3 ownCenter = Geometry3DUtility.GetBoundingBoxOfAllMeshes(gameObject).center;
          Vector3 targetCenter = Geometry3DUtility.GetBoundingBoxOfAllMeshes(_Target).center;
          Gizmos.color = _GizmoColor;
          Gizmos.DrawLine(ownCenter, targetCenter);
        }

      }
    }
  } 
}