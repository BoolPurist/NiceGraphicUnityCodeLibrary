using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceGraphicLibrary.Component;

namespace NiceGraphicLibrary.Component
{
  public class RigidInterpolatedMovement : RigidInterpolatedMotion
  {
    private DebugValueScreenViewer _viewer;

    void Start()
    {      
      _viewer = GetComponent<DebugValueScreenViewer>();
    }
    protected override void ApplyMovement()
    {

      CalculateMotionCycle(
        ref base._currentMovingStateX,
        base._movement.x,
        ref base._currentDurationX,
        ref base._currentSlowingDurationX,        
        ref base._currentCounterDurationX,
        ref base._currentSpeedX,
        LinearSpeedInterpopulation,
        InverseLinearSpeedInterpopulation
        );

      CalculateMotionCycle(
        ref base._currentMovingStateY,
        base._movement.y,
        ref base._currentDurationY,
        ref base._currentSlowingDurationY,        
        ref base._currentCounterDurationY,
        ref base._currentSpeedY,
        LinearSpeedInterpopulation,
        InverseLinearSpeedInterpopulation
        );

      CalculateMotionCycle(
        ref base._currentMovingStateZ,
        base._movement.z,
        ref base._currentDurationZ,
        ref base._currentSlowingDurationZ,
        ref base._currentCounterDurationZ,
        ref base._currentSpeedZ,
        LinearSpeedInterpopulation,
        InverseLinearSpeedInterpopulation
        );

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



      _rb.MovePosition(_rb.position + nextMove);

      _viewer.UpdateValue("X moving State", _currentMovingStateX);
      _viewer.UpdateValue("_currentSlowingDurationX", _currentSlowingDurationX);
      _viewer.UpdateValue("_currentDurationX", _currentDurationX);
      _viewer.UpdateValue("_currentCounterDurationX", _currentCounterDurationX);
      _viewer.UpdateValue("base._movement.x", base._movement.x);
      _viewer.UpdateValue("_currentSpeedX", _currentSpeedX);

      _viewer.UpdateValue("Y moving State", _currentMovingStateY);
      _viewer.UpdateValue("_currentSlowingDurationY", _currentSlowingDurationY);
      _viewer.UpdateValue("base._movement.y", base._movement.y);
      _viewer.UpdateValue("_currentSpeedY", _currentSpeedY);
      
      _viewer.UpdateValue("Z moving State", _currentMovingStateZ);
      _viewer.UpdateValue("_currentSlowingDurationZ", _currentSlowingDurationZ);
      _viewer.UpdateValue("base._movement.z", base._movement.z);
      _viewer.UpdateValue("_currentSpeedZ", _currentSpeedZ);
    }

    

    private float LinearSpeedInterpopulation(float speed, float durationRatio)
      => Mathf.Lerp(0f, speed, durationRatio);

    private float InverseLinearSpeedInterpopulation(float speed, float durationRatio)
      => Mathf.Lerp(0f, speed, 1f - durationRatio);

    protected override void ProcessAxis()
      => ProcessGlobalAxis();
  }
}