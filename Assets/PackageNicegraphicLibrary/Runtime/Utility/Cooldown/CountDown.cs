using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Utility.Cooldown
{
  /// <summary>
  /// Used to manage a countdown independently from Time.deltaTime.
  /// </summary>
  public class CountDown : ITakesDateTimeProvider
  {
    private IDateTimeProvider _dateTimeProvider = new UtcDateTimeProvider();

    // Moment to pass to reach zero, end of count down.
    private DateTime _timeStamp;
    private int _startSeconds = 1;

    private bool _isStopped = true;
    private int _previousRemainingSeconds = 1;

    /// <summary>
    /// Getter: If true count down is stopped, not counting down further
    /// </summary>
    public bool IsStopped => _isStopped;
    /// <summary>
    /// Getter: If true the count down has counted down completely. 
    /// </summary>
    public bool IsDone => RemainingSeconds == 0;

    /// <summary>
    /// Seconds from which the countdown starts to count down.
    /// </summary>
    /// <value>
    /// Negativ values will be converted to positive values
    /// </value>
    public int StartSeconds
    {
      get => _startSeconds;
      set
      {
        _startSeconds = Mathf.Abs(value);
      }
    }
    /// <summary>
    /// Returns remaining seconds to count down. If zero the count down is done.
    /// </summary>
    public int RemainingSeconds
    {
      get
      {
        if (_isStopped)
        {
          return _previousRemainingSeconds;
        }
        else
        {
          SetCurrentRemainingSeconds();
          return _previousRemainingSeconds;
        }
      }
    }

    /// <summary>
    /// Sets up count down.
    /// </summary>
    /// <param name="seconds">
    /// Seconds to count down.
    /// Negativ values will be converted to positive values
    /// </param>
    /// <param name="startAfterCreation">
    /// If true then the count down starts after creation. If false the count down is stopped until <see cref="Resume"/> is called
    /// </param>
    public CountDown(int seconds, bool startAfterCreation = false)
    {
      StartSeconds = seconds;      
      Reset(startAfterCreation);      
    }

    /// <summary>
    /// Resets the countdown. it starts from <see cref="StartSeconds"/> again
    /// </summary>
    /// <param name="startAfterReset">
    /// If true the count down starts from <see cref="StartSeconds"/> to count down right away.
    /// If false the count down does not start until <see cref="Resume"/> is called
    /// </param>
    public void Reset(bool startAfterReset = false)
    {
      _isStopped = true;
      _previousRemainingSeconds = StartSeconds;
      SetTimeStamp(StartSeconds);

      _isStopped = !startAfterReset;
    }

    /// <remarks>
    /// Resets the count down after this call
    /// </remarks>
    public void SetDateTimeProvider(IDateTimeProvider provider)
    {
      if (provider != null)
      {
        _dateTimeProvider = provider;
        Reset();
      }
    }

    /// <summary>
    /// Stops the count down from counting further. <see cref="RemainingSeconds"/> does not change anymore.
    /// </summary>
    public void Stop()
    {
      if (!_isStopped)
      {
        SetCurrentRemainingSeconds();
        _isStopped = true;
      }
    }

    /// <summary>
    /// Resumes the counting. <see cref="RemainingSeconds"/> changes again. 
    /// Counting starts from the remaining seconds before <see cref="Stop"/> was called.
    /// </summary>
    public void Resume()
    {
      if (_isStopped)
      {
        SetTimeStamp(_previousRemainingSeconds);
        _isStopped = false;
      }
    }


    private void SetCurrentRemainingSeconds()
      => _previousRemainingSeconds = Math.Max(0, (_timeStamp - _dateTimeProvider.GetNowDateTime()).Seconds);

    private void SetTimeStamp(in float seconds)
      => _timeStamp = _dateTimeProvider.GetNowDateTime().AddSeconds(seconds);

  } 
}