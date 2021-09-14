using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  public static class Interpolation
  {

    /// <summary>
    /// Coroutine to returns factor from 0 to 1 over a certain duration after each frame 
    /// until the duration has passed    
    /// </summary>
    /// <param name="duration">
    /// Time until the coroutine ends. Negative values will be converted to absolute/positive values.
    /// </param>
    /// <param name="interpolateReceiver">
    /// Is called after each frame. Its argument is from 0 to 1 for the amount of passed duration.
    /// Should not be null.
    /// </param>
    /// <return>
    /// IEnumerator as argument for the StartCoroutine function
    /// </return>
    public static IEnumerator InterpolateOverTime(float duration, Action<float> interpolateReceiver)
    {
      if (interpolateReceiver == null)
      {
        Debug.LogWarning($"{nameof(interpolateReceiver)} was null. Coroutine can not be executed");
        yield break;
      }
      
      float passedTime = 0f;
      duration = Mathf.Abs(duration);
      
      do {
        passedTime += Time.deltaTime;
        float currentInterpolation = Mathf.Lerp(0f, duration, passedTime / duration);
        interpolateReceiver(currentInterpolation);
        yield return null;
      } while (passedTime < duration);
      
    }

    
  } 
}