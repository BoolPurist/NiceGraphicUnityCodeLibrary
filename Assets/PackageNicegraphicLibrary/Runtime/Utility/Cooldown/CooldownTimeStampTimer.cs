using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace NiceGraphicLibrary.Utility.Cooldown
{
  /// <summary>
  /// Object to return a value for the passed time within an interval.
  /// Inner timer stops if Time.timeScale == 0.
  /// </summary>
  public class CooldownTimeStampTimer : ICooldownTimer, ITakesDateTimeProvider
  {

    // Moment in time at which the cool down started to wear off.
    private DateTime _startMoment;
    
    // Used to calculates the left time until the cool down wears off.
    private double _miliSecondsToPass;

    private IDateTimeProvider _dateTimeProvider = new UtcDateTimeProvider();

    /// <param name="_endTime">
    /// Time needs to pass untiles the cool down wears off
    /// Negative value will be converted to a positive one.
    /// </param>
    public CooldownTimeStampTimer(float endTime = 1f)
    {
      SetNewEndTime(endTime);
      Reset();
    }

    #region Implementation of the interface ICooldownTimer
    public float PassedTimeFactor
    {
      get
      {
        DateTime currentMoment = _dateTimeProvider.GetNowDateTime();
        TimeSpan difference = currentMoment - _startMoment;
        double clampedDifference = Math.Min(difference.TotalMilliseconds, _miliSecondsToPass) / _miliSecondsToPass;
        return Convert.ToSingle(clampedDifference);
      }
    }

    public void Reset()
    {      
      _startMoment = _dateTimeProvider.GetNowDateTime();
    }

    public bool WornOff => PassedTimeFactor == 1f;

    public float PassedTime => (PassedTimeFactor * Convert.ToSingle(_miliSecondsToPass)) / 1000f;
    

    public void SetNewEndTime(float newEndTime)
    {
      _miliSecondsToPass = Convert.ToDouble(Mathf.Abs(newEndTime) * 1000f);
    }


    #endregion

    public void SetDateTimeProvider(IDateTimeProvider provider)
    {
      if (provider != null)
      {
        _dateTimeProvider = provider;
      }
    }

  }
}
