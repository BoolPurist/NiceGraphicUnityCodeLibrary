using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Utility
{
  /// <summary>
  /// Used to start a countdown from number of seconds to zero.
  /// </summary>
  public class CountDown 
  {
    // Moment to pass to reach zero, end of count down.
    private DateTime _timeStamp;
    private float _startSeconds = 1f;

    /// <summary>
    /// Sets the count down and starts it.
    /// </summary>
    /// <param name="sconds">
    /// Seconds to count down.
    /// Negativ values will be converted to positive values
    /// </param>
    public CountDown(int seconds)
    {
      StartSeconds = seconds;
      ResetAndStart();
    }

    /// <summary>
    /// Resets the countdown and starts it
    /// </summary>
    public void ResetAndStart()
    {      
      _timeStamp = DateTime.Now.AddSeconds(StartSeconds);  
    }

    /// <summary>
    /// Returns remaining seconds of the count down
    /// </summary>
    public int RemainingSeconds => Math.Max(-1, (_timeStamp - DateTime.Now).Seconds) + 1;

    /// <summary>
    /// Seconds from which the countdown starts to count down.
    /// </summary>
    /// <value>
    /// Negativ values will be converted to positive values
    /// </value>
    public float StartSeconds
    {
      get => _startSeconds;
      set
      {
        _startSeconds = Mathf.Abs(value);
      }
      
    }
  } 
}