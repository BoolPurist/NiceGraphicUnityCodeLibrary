using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Tests.Editor.Stubs
{
  public partial class FakeKeyButtonProvider
  {
    private class InputState
    {
      private bool IsPressed = false;
      private bool IsPressedDown = false;
      private bool IsRelease = false;

      public bool GetInputStateActive(InpuType type)
      {
        switch (type)
        {
          case InpuType.Pressed:
            return IsPressed;
          case InpuType.Down:
            return IsPressedDown;
          case InpuType.Released:
            return IsRelease;
          default:
            Debug.LogError(ErrorMessages.NotAccountedEnumValue(type));
            return false;
        }
      }

      public void SetInputStateActive(InpuType type, bool newInputState)
      {
        switch (type)
        {
          case InpuType.Pressed:
            IsPressed = newInputState;
            return;
          case InpuType.Down:
            IsPressedDown = newInputState;
            return;
          case InpuType.Released:
            IsRelease = newInputState;
            return;
          default:
            Debug.LogError(ErrorMessages.NotAccountedEnumValue(type));
            return;
        }
      }

      public void ResetInputState()
      {
        IsPressed = false;
        IsPressedDown = false;
        IsRelease = false;
      }
    }

  }
}