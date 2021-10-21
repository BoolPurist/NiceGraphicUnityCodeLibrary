using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceGraphicLibrary.Component;

namespace NiceGraphicLibrary.Component
{
  public class RigidInterpolatedMovement : RigidInterpolatedMotion
  {
    private DebugValueScreenViewer _viewer;

    protected override void ApplyMovement()
    {

      base.CalculateMotionCycleX(LinearSpeedInterpopulation, InverseLinearSpeedInterpopulation);
      base.CalculateMotionCycleY(LinearSpeedInterpopulation, InverseLinearSpeedInterpopulation);
      base.CalculateMotionCycleZ(LinearSpeedInterpopulation, InverseLinearSpeedInterpopulation);

      Vector3 nextMove = Vector3.zero;

      if (_AxisLevel == MovementAxisLevel.Global)
      {
        nextMove = new Vector3(
          _currentSpeedX,
          _currentSpeedY,
          _currentSpeedZ
          );
      }
      else
      {
        nextMove += base._currentSpeedX * transform.TransformDirection(Vector3.right);
        nextMove += base._currentSpeedY * transform.TransformDirection(Vector3.up);
        nextMove += base._currentSpeedZ * transform.TransformDirection(Vector3.forward);
      }

      if (IsSetForUnitTest)
      {
        transform.position += nextMove;
      }
      else
      {
        _rb.MovePosition(_rb.position + nextMove);
      }

    }




    private float LinearSpeedInterpopulation(float speed, float durationRatio)
      => Mathf.Lerp(0f, speed, durationRatio);

    private float InverseLinearSpeedInterpopulation(float speed, float durationRatio)
      => Mathf.Lerp(0f, speed, 1f - durationRatio);

    protected override void ProcessAxis()
      => ProcessGlobalAxis();
  }
}