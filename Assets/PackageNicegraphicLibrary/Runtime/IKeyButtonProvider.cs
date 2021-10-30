using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicLibrary
{
  public interface IKeyButtonProvider
  {
    public bool GetKey(string keyName);
    public bool GetKey(KeyCode keyCode);
    public bool GetKeyDown(string keyName);
    public bool GetKeyDown(KeyCode keyCode);
    public bool GetKeyUp(string keyName);
    public bool GetKeyUp(KeyCode keyCode);

    bool GetButton(string buttonName);
    bool GetButtonDown(string buttonName);
    bool GetButtonUp(string buttonName);

    float GetAxis(string axisName);
  }
}