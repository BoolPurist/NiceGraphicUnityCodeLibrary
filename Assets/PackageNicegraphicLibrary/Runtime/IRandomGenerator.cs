using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicLibrary
{
  public interface IRandomGenerator
  {
    public float Value { get; }
    public int Range(int minInclusive, int maxInclusive);
  }
}