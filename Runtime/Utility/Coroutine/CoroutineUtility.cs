using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Utility
{
  /// <summary>
  /// Functions to start coroutines delayed, in certain interval or in a loop
  /// </summary>
  public static class CoroutineUtility
  {
    /// <summary>
    /// Stops a coroutine and starts it again
    /// </summary>
    /// <param name="component">
    /// Component on which the coroutine should be reset. Should not be null.
    /// </param>
    /// <param name="coroutineToReset">
    /// Coroutine to reset. Should not be null.
    /// </param>
    /// <returns>
    /// Reference of reset coroutine
    /// </returns>
    public static Coroutine ResetCoroutine(MonoBehaviour component, IEnumerator coroutineToReset)
    {

      if (component != null && coroutineToReset != null)
      {
        component.StopCoroutine(coroutineToReset);
        return component.StartCoroutine(coroutineToReset);
      }
      else
      {
        return null;
      }

    }

    /// <summary>
    /// Starts a coroutine after a certain delay
    /// </summary>
    /// <param name="component">
    /// Component on which the coroutine should be reset. Should not be null.
    /// </param>
    /// <param name="coroutineToStartDelayed">
    /// coroutine to be stated delayed. Should not be null.
    /// </param>
    /// <param name="delayInSeconds">
    /// Seconds to pass for start of the coroutine
    /// </param>
    /// <returns>
    /// Object to prevent the start of the coroutine if not started yet or stopping it if started already
    /// </returns>
    public static StopableCoroutines StartCoroutineDelayed(MonoBehaviour component, Func<IEnumerator> coroutineToStartDelayed, float delayInSeconds = 1f)
    {
      
      return InitializeMainCoroutine(component, coroutineToStartDelayed, delayInSeconds, Delay);

      IEnumerator Delay(float delay, StopableCoroutines coroutines, Func<IEnumerator> newCoroutineConstructor)
      {
        yield return new WaitForSeconds(delay);
        coroutines.AddCreatedCoroutine(component.StartCoroutine(newCoroutineConstructor()));
      }
    }

    /// <inheritdoc cref="CoroutineUtility.StartCoroutineDelayed(MonoBehaviour, Func{IEnumerator}, float)"/>
    public static StopableCoroutines StartActionDelayed(MonoBehaviour component, Action coroutineToStartDelayed, float delayInSeconds = 1f)
    {
      if (coroutineToStartDelayed != null)
      {
        return StartCoroutineDelayed(component, () => StartAction(coroutineToStartDelayed), delayInSeconds);
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Starts coroutine and starts it again an again in a interval with given time.
    /// </summary>
    /// <param name="component">
    /// Component on which the coroutine should be reset. Should not be null.
    /// </param>
    /// <param name="coroutineToStartInInterval">
    /// Coroutine to start in a given interval again and again
    /// </param>
    /// <param name="intervalInSeconds">
    /// Time to pass to start a coroutine again.
    /// </param>
    /// <returns>
    /// Object to stop the starting of coroutines  in the given interval and stops all coroutines started by this interval call.
    /// </returns>
    public static StopableCoroutines StarCoroutineInInterval(MonoBehaviour component, Func<IEnumerator> coroutineToStartInInterval, float intervalInSeconds = 1f)
    {
      return InitializeMainCoroutine(component, coroutineToStartInInterval, intervalInSeconds, Interval);

      IEnumerator Interval(float interval, StopableCoroutines coroutines, Func<IEnumerator> coroutineConstructor)
      {
        while (true)
        {          
          coroutines.AddCreatedCoroutine(component.StartCoroutine(coroutineConstructor()));
          yield return new WaitForSeconds(interval);
        }
      }
    }

    /// <inheritdoc cref="CoroutineUtility.StarCoroutineInInterval" />
    public static StopableCoroutines StartActionInInterval(MonoBehaviour component, Action actionToStarInInterval, float intervalInSeconds = 1f)
    {
      if (actionToStarInInterval != null)
      {
        return StarCoroutineInInterval(component, () => StartAction(actionToStarInInterval), intervalInSeconds);
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Starts a coroutine and starts again after the coroutine is finished
    /// </summary>
    /// <param name="component">
    /// Component on game object to start coroutine in a loop. Should not be null.
    /// </param>
    /// <param name="coroutineToStarInInterval">
    /// Coroutine to start in a loop.
    /// </param>
    /// <returns>
    /// Object to stop the starting of new coroutines and stops current started coroutine within the loop.
    /// </returns>
    public static StopableCoroutines StartCoroutineInLoop(MonoBehaviour component, Func<IEnumerator> coroutineToStarInInterval)
    {
      return InitializeMainCoroutine(component, coroutineToStarInInterval, 0f, Loop);

      IEnumerator Loop(float delay, StopableCoroutines coroutines, Func<IEnumerator> coroutineConstructor)
      {
        Coroutine lastStartedCoroutine;

        while (true)
        {     
          lastStartedCoroutine = component.StartCoroutine(coroutineConstructor());
          coroutines.AddCreatedCoroutine(lastStartedCoroutine);
          yield return lastStartedCoroutine;
          yield return new WaitForEndOfFrame();
          coroutines.RemoveDoneCoroutine(lastStartedCoroutine);          
        }
      }
    }

    /// <inheritdoc cref="CoroutineUtility.StartCoroutineInLoop" />
    public static StopableCoroutines StartActionInLoop(MonoBehaviour component, Action actionToStarInLoop)
    {
      if (actionToStarInLoop != null)
      {
        return StartCoroutineInLoop(component, () => StartAction(actionToStarInLoop));
      }
      else
      {
        return null;
      }
    }

    #region Routines
    private static IEnumerator StartAction(Action action)
    {
      action();
      yield break;
    }

    private static StopableCoroutines InitializeMainCoroutine(
      MonoBehaviour component, 
      Func<IEnumerator> coroutineConstructor, 
      float seconds, 
      Func<float, StopableCoroutines, Func<IEnumerator>, IEnumerator> subCoroutinesLogic
      )
    {      
      if (component != null && coroutineConstructor != null)
      {
        StopableCoroutines coroutineContainer = new StopableCoroutines(component);
        seconds = Mathf.Abs(seconds);
        return coroutineContainer.AddCreatedCoroutine(component.StartCoroutine(subCoroutinesLogic(seconds, coroutineContainer, coroutineConstructor)));
      }
      else
      {
        return null;
      }
    }



    #endregion

  }
}