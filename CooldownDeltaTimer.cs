using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace NiceGraphicsLibrary
{
  public class CooldownDeltaTimer : ICooldownTimer
  {
    // Time to be passed until cool down wears off.
    private float endTime;
    // Time already passed
    private float passedTime;

    /// <param name="_endTime">
    /// Time needs to pass untiles the cool down wears off
    /// Negative value will be converted to a positive one.
    /// </param>
    public CooldownDeltaTimer(float _endTime = 1f)
    {
      endTime = Mathf.Abs(_endTime);
      passedTime = 0f;
    }

    /// <summary>
    /// Adds the seconds passed since the last frame
    /// </summary>
    public void Update() => passedTime += Time.deltaTime;

    /// <summary>
    /// Adds given value to the passed time for the cool down.
    /// </summary>
    public void Update(float time) => passedTime += Mathf.Abs(time);

    #region Implementation of the interface ICooldownTimer
    public float PassedTimeFactor => Mathf.Clamp(passedTime, 0f, endTime) / endTime;

    public void Reset() => passedTime = 0f;

    public bool WornOff => passedTime >= endTime;

    public void SetNewEndTime(float newEndTime) => endTime = Mathf.Abs(newEndTime);
    #endregion
  }
}
