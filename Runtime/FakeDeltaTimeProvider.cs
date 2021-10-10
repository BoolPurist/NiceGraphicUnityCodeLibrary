using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NiceGraphicLibrary
{
  public class FakeDeltaTimeProvider : IDeltaTimeProvider
  {
    private float _deltaTimeFactor = 1f;

    public float DeltaTimeFactor 
    { 
      get => _deltaTimeFactor;
      set
      {
        _deltaTimeFactor = Mathf.Abs(value);
      }
    }

    public FakeDeltaTimeProvider() : this(1f) { }
    public FakeDeltaTimeProvider(float deltaTimeFactor)
    {
      DeltaTimeFactor = deltaTimeFactor;
    }

    public float GetDelatTime()
    {
      return DeltaTimeFactor;
    }
  }
}
