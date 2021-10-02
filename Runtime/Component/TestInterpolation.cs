using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceGraphicLibrary;
using NiceGraphicLibrary.Utility;

[ExecuteInEditMode]
public class TestInterpolation : MonoBehaviour
{
  [SerializeField]
  private Color _StartColor = Color.white;
  [SerializeField]
  private Color _EndColor = Color.red;

  private Color _currentColor;

  private Coroutine _currentCoroutine;

  private const string TEXT_TEMPLATE = "Current Color";
  private string _currentMessage = "";

  private void Awake()
  {
    _currentColor = _StartColor;
    _currentMessage = TEXT_TEMPLATE.WithColor(_StartColor);
  }

  [ContextMenu("Interpolation")]
  public void StartInterpolation()
  {
    if (_currentCoroutine != null)
    {
      StopCoroutine(_currentCoroutine);
    }
    _currentCoroutine = StartCoroutine(Interpolation.InterpolateColorOverTime(5f, _StartColor, _EndColor, InterpolationLogic));

  }

  private void InterpolationLogic(Color newColor)
  {
    _currentColor = newColor;
    _currentMessage = TEXT_TEMPLATE.WithColor(_currentColor);
  }

  private void Update()
  {
    Debug.Log(_currentMessage);
  }
}
