using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceGraphicLibrary.Component;

namespace NiceGraphicLibrary
{
  public class RigidForceMovement : RigidGeometryMotion
  {
    [SerializeField]
#pragma warning disable IDE0044 // Add readonly modifier
    private ForceMode _ForceMode = ForceMode.VelocityChange;
#pragma warning restore IDE0044 // Add readonly modifier

    protected override void ApplyMovement()
    {
      float currentSpeed = Speed * Time.deltaTime;
      Vector3 currentMovement = _movement * currentSpeed;
      _rb.AddForce(currentMovement, _ForceMode);
    }

    protected override void ProcessAxis()
      => ProcessMovementAxis();
  }
}