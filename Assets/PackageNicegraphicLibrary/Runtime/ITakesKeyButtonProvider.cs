using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicLibrary
{
  public interface ITakesKeyButtonProvider
  {
    public void SetKeyButtonProvider(IKeyButtonProvider newProvider);
  }
}