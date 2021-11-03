using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Tests.Editor.Stubs
{
  public class FakeRandomGenerator : IRandomGenerator
  {
    public float FakeValue { get; set; } = 0.5f;

    public float Value => FakeValue;

    public int FakeRangeReturnValue { get; set; } = 0;

    public int Range(int minInclusive, int maxInclusive)
      => FakeRangeReturnValue;
  }
}