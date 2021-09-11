using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace NiceGraphicsLibrary
{
  public class CooldownTimeStampTimer : ICooldownTimer
  {
    // Moment to pass for cool down to wear off.
    private DateTime _endMoment;
    // Moment in time at which the cool down started to wear off.
    private DateTime _startMoment;
    
    // Used to calculates the left time until the cool down wears off.
    private double _endTime;

    /// <param name="_endTime">
    /// Time needs to pass untiles the cool down wears off
    /// Negative value will be converted to a positive one.
    /// </param>
    public CooldownTimeStampTimer(double endTime = 1f)
    {
      _endTime = Math.Abs(endTime);      
      Reset();
    }

    #region Implementation of the interface ICooldownTimer
    public float PassedTimeFactor
    {
      get
      {
        if (DateTime.Now >= _endMoment)
        {
          return 1f;
        }
        else
        {
          double differnceInSeconds = (_endMoment - DateTime.Now).TotalSeconds;
          double differencFactor = 1.0 - Math.Min(1, differnceInSeconds / _endTime);
          return Mathf.Min(1f, Convert.ToSingle(differencFactor));
        }

      }
    }

    public void Reset()
    {
      _startMoment = DateTime.Now;
      _endMoment = DateTime.Now.AddSeconds(_endTime);
    }

    public bool WornOff => PassedTimeFactor == 1f;

    public void SetNewEndTime(float newEndTime)
    {
      double absNewEndTime = Convert.ToDouble(Mathf.Abs(newEndTime));
      _endMoment = _startMoment.AddSeconds(absNewEndTime);
      _endTime = absNewEndTime;      
    }
    #endregion

  }
}
