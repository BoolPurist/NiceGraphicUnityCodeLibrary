using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicLibrary
{
  public interface ITakesGameInputProviderProvider
  {
    public void SetKeyButtonProvider(IGameInputProvider newProvider);
  }
}