using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using NiceGraphicLibrary.ScriptableObjects;
using NiceGraphicLibrary.Utility;

namespace NiceGraphicLibrary.Component.GUI
{
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class TextTemplate : MonoBehaviour
  {
    [SerializeField]
    private TextTemplateData _templateDate;

    private TextMeshProUGUI _textComponent;

    private Dictionary<string, string> _valueTable = new Dictionary<string, string>();

    private void OnValidate()
    {
      Start();
    }

    public void Start()
    {      
      _textComponent = ComponentUtility.EnsureComponentOn<TextMeshProUGUI>(gameObject);
      if (_textComponent != null)
      {
        
        if (_templateDate == null)
        {
          _textComponent.text = "No data asset applied !";
        }
        else
        {
          _textComponent.text = _templateDate.GetPreviewText();
          _valueTable = _templateDate.DefaultValuesCopy;
        }
      }

    
    }

    public void InsertValueWithName(in string name, in string insertedValue)
    {

      _textComponent.text = _templateDate.GetTextWithInsertedValue(name, insertedValue, _valueTable);
    }
  } 
}