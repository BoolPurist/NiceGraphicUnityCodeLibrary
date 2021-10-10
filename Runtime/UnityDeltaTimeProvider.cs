using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  public class UnityDeltaTimeProvider : IDeltaTimeProvider
  {
    public float GetDelatTime()
    {
      return Time.deltaTime;
    }
  }
}