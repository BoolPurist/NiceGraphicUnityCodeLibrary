using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicLibrary
{
  public class FixedDateTimeProvider : IDateTimeProvider
  {
    public DateTime FixedTimeStamp { get; set; } = DateTime.UtcNow;

    public DateTime ChangeBySeconds(int seconds) => FixedTimeStamp = FixedTimeStamp.AddSeconds(seconds);

    public DateTime PlusOneSecond() => ChangeBySeconds(1);
    public DateTime MinusOneSecond() => ChangeBySeconds(-1);

    public DateTime GetNowDateTime()
      => FixedTimeStamp;
  }
}