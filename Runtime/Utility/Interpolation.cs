using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Utility
{
  /// <summary>
  /// Provides functions to interpolate values.
  /// </summary>
  public static class Interpolation
  {

    /// <summary>
    /// Coroutine to returns factor from 0 to 1 to a given delegate over a certain duration after each frame 
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
    public static IEnumerator InterpolateOverTime(float duration, Action<float> interpolateReceiver, float startDuration = 0f)
    {
      if (interpolateReceiver == null)
      {
        Debug.LogWarning($"{nameof(interpolateReceiver)} was null. Coroutine can not be executed");
        yield break;
      }
      
      duration = Mathf.Abs(duration);
      float passedTime = Mathf.Clamp(startDuration, 0f, duration);
      
      do {
        passedTime += Time.deltaTime;
        float currentInterpolation = Mathf.Lerp(0f, duration, passedTime / duration);
        interpolateReceiver(currentInterpolation);
        yield return null;
      } while (passedTime < duration);
      
    }

    /// <summary>
    /// Returns plotted value of the smooth step function. Has lower acceleration near 0 and 1 factor.
    /// </summary>
    /// <param name="numberValue">
    /// Value to be plotted
    /// </param>
    /// <param name="factor">
    /// Factor to plot parameter [value] according to the smooth step function
    /// factor will be clamped between 0 and 1. 
    /// </param>
    /// <returns>
    /// Returns a number from 0 to given parameter [value] depending on the parameter [factor]
    /// </returns>
    /// <remarks>
    /// Implementation comes from wiki <seealso href="https://en.wikipedia.org/wiki/Smoothstep"/>
    /// </remarks>
    public static float SmoothStep(float numberValue, float factor)
    {
      factor = Mathf.Clamp(factor, 0f, 1f);
      // 3x^2 - 2x^3
      factor = (factor * factor) * (3 - (2 * factor));
      return factor * numberValue;
    }

    /// <summary>
    /// Smooth step variation suggested by Ken Perlin better approximation but requires more calculation than <see cref="SmoothStep(float, float)"/>
    /// </summary>
    /// <param name="numberValue">
    /// Value to be plotted
    /// </param>
    /// <param name="factor">
    /// Factor to plot parameter [value] according to the smooth step function
    /// factor will be clamped between 0 and 1. 
    /// </param>
    /// <returns>
    /// Returns a number from 0 to the given parameter [value] depending on the parameter [factor]
    /// </returns>
    /// <remarks>
    /// Implementation comes from wiki <seealso href="https://en.wikipedia.org/wiki/Smoothstep"/>
    /// </remarks>
    public static float SmootherStep(float numberValue, float factor)
    {
      factor = Mathf.Clamp(factor, 0f, 1f);
      // 6x^5 - 15x^4 + 10x^3
      factor = factor * factor * factor * ( factor * ( (factor * 6) - 15 ) + 10 );
      return factor * numberValue;
    }

    /// <summary>
    /// Turns the output of smooth step upside down. Can be used for deceleration after acceleration by <see cref="SmoothStep(float, float)"/>
    /// </summary>
    /// <param name="numberValue">
    /// Value to be plotted
    /// </param>
    /// <param name="factor">
    /// Factor to plot parameter [value] according to the smooth step function
    /// factor will be clamped between 0 and 1. 
    /// </param>
    /// <returns>
    /// Returns from parameter [value] to 0 depending on the parameter [factor]
    /// </returns>
    public static float InverseSmoothStep(float numberValue, float factor)
      => (-1f * SmoothStep(numberValue, factor)) + numberValue; // Graph is mirrored on the x-axis and then moved up by the amount of the parameter [value]
    
    /// <summary>
    /// Turns the output of smoother step upside down. Can be used for deceleration after acceleration by <see cref="SmootherStep(float, float)"/>
    /// </summary>
    /// <param name="numberValue">
    /// Value to be plotted
    /// </param>
    /// <param name="factor">
    /// Factor to plot parameter [value] according to the smooth step function
    /// factor will be clamped between 0 and 1. 
    /// </param>
    /// <returns>
    /// Returns from parameter [value] to 0 depending on the parameter [factor]
    /// </returns>
    public static float InverseSmootherStep(float numberValue, float factor)
      => (-1f * SmootherStep(numberValue, factor)) + numberValue; // Graph is mirrored on the x-axis and then moved up by the amount of the parameter [value]

  } 
}