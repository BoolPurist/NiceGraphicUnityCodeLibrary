using System;
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
    /// Returns the bounding box of the all meshes under the object.
    /// Returns a bounding box with a vector (0,0,0) for center, min, max, extends and size  
    /// if the object itself and none of its children has a MeshRenderer Component
    /// </return>
    /// <remarks>
    /// To work, object with a mesh needs the component MeshRenderer. 
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// If rootObject is null
    /// </exception>
    public static Bounds GetBoundingBoxOfAllMeshes(GameObject rootObject)
    {      
      if (rootObject == null)
      {
        throw new ArgumentNullException($"{nameof(rootObject)} must not be null");
      }

      var resultBoundingBox = new Bounds();

      float minX = float.MaxValue;
      float maxX = float.MinValue;
      float minY = float.MaxValue;
      float maxY = float.MinValue;
      float minZ = float.MaxValue;
      float maxZ = float.MinValue;

      bool hasMeshRenderer = false;

      CompareOneMesh(rootObject);
      
      if (hasMeshRenderer)
      {
        resultBoundingBox.max = new Vector3(maxX, maxY, maxZ);
        resultBoundingBox.min = new Vector3(minX, minY, minZ);        
      }

      return resultBoundingBox;

      void CompareOneMesh(GameObject oneObject)
      {
        var meshRenderer = oneObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
          hasMeshRenderer = true;
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