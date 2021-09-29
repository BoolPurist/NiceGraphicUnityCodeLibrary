using System;
using UnityEngine;

namespace NiceGraphicLibrary
{
  /// <summary>
  /// 
  /// </summary>
  public class ReadOnlyFieldAttribute : PropertyAttribute 
  {
    private readonly string _differentName = null;
    public string DifferentName => _differentName;

    public bool IsEmpty => string.IsNullOrWhiteSpace(DifferentName);
    public ReadOnlyFieldAttribute(string differentName = null) => _differentName = differentName; 
  }
}