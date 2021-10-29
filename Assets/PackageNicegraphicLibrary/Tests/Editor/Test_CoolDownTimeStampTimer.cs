using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NiceGraphicLibrary.Utility.Cooldown;

namespace NiceGraphicLibrary.Tests.Editor
{
  [TestFixture]
  public class Test_CoolDownTimeStampTimer
  {
    private const float TIME_TO_PASS = 5f;
    private const float FAKE_DELTA_TIME_FACTOR = 1f;

    private CooldownTimeStampTimer _timer;
    private FixedDateTimeProvider _fakeDateTimeProvider = new FixedDateTimeProvider();

    [SetUp]
    public void SetUp()
    {
      _timer = new CooldownTimeStampTimer();
      _fakeDateTimeProvider.FixedTimeStamp = new DateTime(0);
      _timer.SetDateTimeProvider(_fakeDateTimeProvider);
      _timer.SetNewEndTime(TIME_TO_PASS);
      _timer.Reset();
    }

    [Test]
    public void Test_PassedTime()
    {
      Assert.AreEqual(0f, _timer.PassedTimeFactor, $"{nameof(_timer.PassedTimeFactor)} should be zero.");
      Assert.IsFalse(_timer.WornOff, $"Timer should not have worn off yet !");

      float previousPassedTimeFactor = 0f;

      for (
        float currentPassedTime = 0f, endTime = TIME_TO_PASS - FAKE_DELTA_TIME_FACTOR;
        currentPassedTime < endTime;
        currentPassedTime += FAKE_DELTA_TIME_FACTOR
        )
      {
        Assert.AreEqual(currentPassedTime, _timer.PassedTime, $"Passed time from time is not correct.");

        _fakeDateTimeProvider.PlusOneSecond();

        Assert.Greater(
          _timer.PassedTimeFactor,
          previousPassedTimeFactor,
          $"{nameof(_timer.PassedTimeFactor)} should have been greater than the previous passed time factor."
          );

        previousPassedTimeFactor = _timer.PassedTimeFactor;

        Assert.IsFalse(_timer.WornOff, $"Timer should not have worn off yet !");
      }

      // Last Update leading to end moment
      _fakeDateTimeProvider.PlusOneSecond();
      Assert.AreEqual(1f, _timer.PassedTimeFactor, $"{nameof(_timer.PassedTimeFactor)} should be one.");
      Assert.IsTrue(_timer.WornOff, $"Timer should have worn off !");

      _fakeDateTimeProvider.PlusOneSecond();
      Assert.AreEqual(1f, _timer.PassedTimeFactor, $"{nameof(_timer.PassedTimeFactor)} should be one still.");
    }
  }
}