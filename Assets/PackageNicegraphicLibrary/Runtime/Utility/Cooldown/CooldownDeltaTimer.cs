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
  /// That cool down does not stop even if Time.timeScale== 0 because the inner timer works the DateTime API
  /// </summary>
  public class CooldownDeltaTimer : ICooldownTimer, ITakesDeltaTimeProvider
  {
    // Time to be passed until cool down wears off.
    private float _endTime;
    // Time already passed
    private float _passedTime;

    private IDeltaTimeProvider _deltaTimeProvider = new UnityDeltaTimeProvider();

    /// <param name="_endTime">
    /// Time needs to pass untiles the cool down wears off
    /// Negative value will be converted to a positive one.
    /// </param>
    public CooldownDeltaTimer(float _endTime = 1f)
    {
      this._endTime = Mathf.Abs(_endTime);
      _passedTime = 0f;
    }

    /// <summary>
    /// Adds the seconds passed since the last frame
    /// </summary>
    public void Update() => _passedTime += _deltaTimeProvider.GetDelatTime();

    /// <summary>
    /// Adds given value to the passed time for the cool down.
    /// </summary>
    public void Update(float time) => _passedTime += Mathf.Abs(time);

    #region Implementation of the interface ICooldownTimer
    public float PassedTimeFactor => PassedTime / _endTime;

    public void Reset() => _passedTime = 0f;

    public bool WornOff => _passedTime >= _endTime;

    public float PassedTime => Mathf.Clamp(_passedTime, 0f, _endTime);

    public void SetNewEndTime(float newEndTime) => _endTime = Mathf.Abs(newEndTime);

    #endregion

    public void SetDeltaTimeProvider(IDeltaTimeProvider provider)
    {
      if (provider != null)
      {
        _deltaTimeProvider = provider;
      }
    }

  }
}
