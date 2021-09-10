using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicsLibrary
{
  public static class Geometry3DUtility
  {
    /// <summary>
    /// Gets the center, size etc from a whole mesh of an object. Whole mesh can made up of several meshes
    /// residing in the object itself and its children
    /// </summary>
    /// <param name="rootObject">
    /// Object to get the bounding box from
    /// </param>
    /// <return>
    /// Returns the bounding box of the whole mesh
    /// </return>
    /// <remarks>
    /// To work, object with a mesh needs the component MeshRenderer
    /// </remarks>
    public static Bounds GetVolumeBoundingBox(GameObject rootObject)
    {
      float minX = float.MaxValue;
      float maxX = float.MinValue;
      float minY = float.MaxValue;
      float maxY = float.MinValue;
      float minZ = float.MaxValue;
      float maxZ = float.MinValue;

      CompareOneMesh(rootObject);

      var bounding = new Bounds()
      {
        max = new Vector3(maxX, maxY, maxZ),
        min = new Vector3(minX, minY, minZ)
      };

      return bounding;

      void CompareOneMesh(GameObject oneObject)
      {
        var meshRenderer = oneObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
          Bounds boundingBox = meshRenderer.bounds;
          minX = Mathf.Min(minX, boundingBox.min.x);
          maxX = Mathf.Max(maxX, boundingBox.max.x);
          minY = Mathf.Min(minY, boundingBox.min.y);
          maxY = Mathf.Max(maxY, boundingBox.max.y);
          minZ = Mathf.Min(minZ, boundingBox.min.z);
          maxZ = Mathf.Max(maxZ, boundingBox.max.z);
        }

        for (int i = 0; i < oneObject.transform.childCount; i++)
        {
          CompareOneMesh(oneObject.transform.GetChild(i).gameObject);
        }
      }
    }
  }

}