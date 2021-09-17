using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NiceGraphicLibrary.Utility;

namespace NiceGraphicLibrary.Component
{
  /// <summary>
  /// Draws a bounding box which contains all meshes under one game object
  /// while selected even in editor mode.
  /// </summary>
  public class DrawBoundingBox : MonoBehaviour
  {

    [System.Serializable]
    public class ElementColors
    {
      public Color Center = Color.red;
      public Color Min = Color.blue;
      public Color Max = Color.green;
      public Color WireFrame = Color.grey;
      public Color Diagonal = Color.cyan;
    }

    [SerializeField]
    [Min(0)]
    [Tooltip("Radius of a sphere which represents a point")]
    private float RadiusSpherePoint = 0.1f;

    [SerializeField]
    private ElementColors Colors;

    private void OnDrawGizmosSelected()
    {      
      Bounds boundingBox = Geometry3DUtility.GetBoundingBoxOfAllMeshes(gameObject);
      boundingBox.Encapsulate(this.transform.TransformPoint(Vector3.zero));
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(boundingBox.center, RadiusSpherePoint);
      Gizmos.color = Colors.Min;
      Gizmos.DrawSphere(boundingBox.min, RadiusSpherePoint);
      Gizmos.color = Colors.Max;
      Gizmos.DrawSphere(boundingBox.max, RadiusSpherePoint);
      Gizmos.color = Colors.WireFrame;
      Gizmos.DrawWireCube(boundingBox.center, boundingBox.size);
      Gizmos.color = Colors.Diagonal;
      Gizmos.DrawLine(boundingBox.min, boundingBox.max);
     
    }

    
  }

}
