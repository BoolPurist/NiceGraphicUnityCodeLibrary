using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  public abstract class RigidInterpolatedMotion : RigidGeometryMotion
  {
    [SerializeField]
    [Min(0)]
    protected float _Duration = 1f;
    [SerializeField]
    [Min(0)]
    protected float _CounterDuration = 0.5f;
    [SerializeField]
    [Min(0)]
    protected float _SlowingDuration = 1f;

    protected float _currentDurationX = 0f;
    protected float _currentSlowingDurationX = 0f;
    protected float _currentCounterDurationX = 0f;
    protected float _currentSpeedX = 0f;
    protected float _currentDurationY = 0f;
    protected float _currentSlowingDurationY = 0f;
    protected float _currentCounterDurationY = 0f;
    protected float _currentSpeedY = 0f;
    protected float _currentDurationZ = 0f;
    protected float _currentSlowingDurationZ = 0f;
    protected float _currentCounterDurationZ = 0f;
    protected float _currentSpeedZ = 0f;
    
    protected MovingState _currentMovingStateX = MovingState.Standing;
    protected MovingState _currentMovingStateY = MovingState.Standing;
    protected MovingState _currentMovingStateZ = MovingState.Standing;

    public MovingState CurrentMovingStateX => _currentMovingStateX;
    public MovingState CurrentMovingStateY => _currentMovingStateY;
    public MovingState CurrentMovingStateZ => _currentMovingStateZ;

    protected void CalculateMotionCycle(
        ref MovingState movingState,
        in float axisDirection,
        ref float duration,
        ref float slowingDuration,
        ref float counteDuration,
        ref float speed,
        Func<float, float, float> interpolation,
        Func<float, float, float> inverseInterpolation
      )
    {
      bool stateHasChanged;

      do
      {
        
        stateHasChanged = false;

        if (movingState == MovingState.Standing)
        {
          // Possible change to other state
          if (IsControlledToRight(axisDirection))
          {            
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SpeedingUpFront);
          }
          else if (IsControlledToLeft(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SpeedingUpBack);
          }
          else
          {            
            duration = 0f;
            slowingDuration = 0f;
            counteDuration = 0f;
            speed = 0f;
          }
        }
        else if (movingState == MovingState.SpeedingUpFront)
        {
          if (IsNotControlled(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SlowingDownFront);
            ConvertDurationInverse(ref slowingDuration, duration, _Duration, _SlowingDuration);
          }
          else if (IsControlledToLeft(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.CounterSpeedingUpBack);
            ConvertDurationInverse(ref counteDuration, duration, _Duration, _CounterDuration);
          }
          else
          {
            counteDuration = 0f;
            slowingDuration = 0f;
            duration = Mathf.Min(duration + Time.deltaTime, _Duration);
            speed = interpolation(Speed, duration / _Duration);
          }
        }
        else if (movingState == MovingState.SpeedingUpBack)
        {
          if (IsNotControlled(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SlowingDownBack);
            ConvertDurationInverse(ref slowingDuration, duration, _Duration, _SlowingDuration);
          }
          else if (IsControlledToRight(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.CounterSpeedingUpFront);
            ConvertDurationInverse(ref counteDuration, duration, _Duration, _CounterDuration);
          }
          else
          {
            counteDuration = 0f;
            slowingDuration = 0f;
            duration = Mathf.Min(duration + Time.deltaTime, _Duration);
            speed = -interpolation(Speed, duration / _Duration);
          }
        }
        else if (movingState == MovingState.SlowingDownFront)
        {
          if (IsNotMoving(axisDirection, speed))
          {
            
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.Standing);
          }
          else if (IsControlledToRight(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SpeedingUpFront);
            ConvertDurationInverse(ref duration, slowingDuration, _SlowingDuration, _Duration);
          }
          else if (IsControlledToLeft(axisDirection))
          {            
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.CounterSpeedingUpBack);
            ConvertDuration(ref counteDuration, slowingDuration, _SlowingDuration, _CounterDuration);
          }
          else
          {
            duration = 0f;
            counteDuration = 0f;
            slowingDuration = Mathf.Min(slowingDuration + Time.deltaTime, _SlowingDuration);
            speed = inverseInterpolation(Speed, slowingDuration / _SlowingDuration);            
          }
        }
        else if (movingState == MovingState.SlowingDownBack)
        {
          if (IsNotMoving(axisDirection, speed))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.Standing);
          }
          else if (IsControlledToLeft(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SpeedingUpFront);
            ConvertDurationInverse(ref duration, slowingDuration, _SlowingDuration, _Duration);
          }
          else if (IsControlledToRight(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.CounterSpeedingUpFront);
            ConvertDuration(ref counteDuration, slowingDuration, _SlowingDuration, _CounterDuration);
          }
          else
          {
            duration = 0f;
            counteDuration = 0f;
            slowingDuration = Mathf.Min(slowingDuration + Time.deltaTime, _SlowingDuration);
            speed = -inverseInterpolation(Speed, slowingDuration / _SlowingDuration);
          }
        }
        else if (movingState == MovingState.CounterSpeedingUpFront)
        {
          if (HasNoSpeed(speed))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.Standing);
          }
          else if (IsControlledToLeft(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SpeedingUpBack);
            ConvertDurationInverse(ref duration, counteDuration, _CounterDuration, _Duration);
          }
          else if (IsNotControlled(axisDirection) && !HasNoSpeed(speed))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SlowingDownBack);
          }
          else
          {
            duration = 0f;
            slowingDuration = 0f;
            counteDuration = Mathf.Min(counteDuration + Time.deltaTime, _CounterDuration);
            speed = -inverseInterpolation(Speed, counteDuration / _CounterDuration);
          }
        }
        else if (movingState == MovingState.CounterSpeedingUpBack)
        {
          // MovingState.CounterSpeedingUpBack
          if (HasNoSpeed(speed))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.Standing);
          }
          else if (IsControlledToRight(axisDirection))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SpeedingUpFront);
            ConvertDurationInverse(ref duration, counteDuration, _CounterDuration, _Duration);
          }
          else if (IsNotControlled(axisDirection) && !HasNoSpeed(speed))
          {
            ChangeToAnotherState(ref stateHasChanged, ref movingState, MovingState.SlowingDownFront);
          }
          else
          {
            duration = 0f;
            slowingDuration = 0f;
            counteDuration = Mathf.Min(counteDuration + Time.deltaTime, _CounterDuration);
            speed = inverseInterpolation(Speed, counteDuration / _CounterDuration);
          }
        }

      } while (stateHasChanged);

      bool IsControlledToRight(in float checkedAxis) => checkedAxis > 0f;

      bool IsControlledToLeft(in float checkedAxis) => checkedAxis < 0f;

      bool IsNotControlled(in float chechedAxis) => chechedAxis == 0f;

      bool HasNoSpeed(in float checkedSpeed) => checkedSpeed == 0f;

      bool IsNotMoving(in float checkedAxis, in float checkedSpeed) => IsNotControlled(checkedAxis) && HasNoSpeed(checkedSpeed);

      void ConvertDurationInverse(
        ref float durationToBeConverted, 
        in float durationToConvertFrom, 
        in float referenceDurationFrom, 
        in float referenceDurationTo
        ) => durationToBeConverted = (1f - (durationToConvertFrom / referenceDurationFrom)) * referenceDurationTo;

      void ConvertDuration(
        ref float durationToBeConverted,
        in float durationToConvertFrom,
        in float referenceDurationFrom,
        in float referenceDurationTo
        ) => durationToBeConverted = (durationToConvertFrom / referenceDurationFrom) * referenceDurationTo;

      void ChangeToAnotherState(
        ref bool stateHasChanged,
        ref MovingState oldMovingState,
        in MovingState newMovingState
        )
      {
        stateHasChanged = true;
        oldMovingState = newMovingState;
      }

    }

    
  } 
}