using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using NiceGraphicLibrary.ScriptableObjects;
using NiceGraphicLibrary.Utility;

namespace NiceGraphicLibrary.Component.GUI
{
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class TextTemplateInsertion : MonoBehaviour
  {
    [SerializeField]
    private TextTemplateData _templateDate;

    private TextMeshProUGUI _textComponent;

    private void OnValidate()
    {
      Start();
    }

    private void Start()
    {
      _textComponent = ComponentUtility.EnsureComponentOn<TextMeshProUGUI>(gameObject);
      if (_textComponent != null)
      {
        _textComponent.text = _templateDate == null ? "No data asset applied !": _templateDate.Text;
      }
      
    }

    public void InsertValueWithName(string name, string insertedValue)
    {
      _templateDate.InsertValueAt(name, insertedValue);
      _textComponent.text = _templateDate.Text;
    }
  } 
}