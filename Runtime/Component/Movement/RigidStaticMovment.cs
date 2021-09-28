using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  public class RigidStaticMovment : RigidGeometryMotion
  {

    protected override void ApplyMovement()
    {
      float currentSpeed = Speed * Time.deltaTime;
      Vector3 movement = currentSpeed * _movement;
      Vector3 nextPosition = _rb.position + movement;
      _rb.MovePosition(nextPosition);
    }

    protected override void ProcessAxis()
     => ProcessMovementAxis();
  }
}