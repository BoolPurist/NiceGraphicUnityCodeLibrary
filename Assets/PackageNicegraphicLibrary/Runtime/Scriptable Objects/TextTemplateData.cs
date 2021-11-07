using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using NiceGraphicLibrary.Utility;

namespace NiceGraphicLibrary.ScriptableObjects
{
  [CreateAssetMenu(fileName = "NewTextTemplateData", menuName = "NiceGraphicLibrary/TextTemplateData")]
  public class TextTemplateData : ScriptableObject
  {
    private const string DEFAULT_LEFT_SEPARATOR = "{";
    private const string DEFAULT_RIGHT_SEPARATOR = "}";

    [System.Serializable]
    public class ValueEntry
    {
      public string Name = "";    
      public string DefaultValue = "";
    }

    [SerializeField]
    private string _leftSeparator = DEFAULT_LEFT_SEPARATOR;
    [SerializeField]
    private string _rightSeparator = DEFAULT_RIGHT_SEPARATOR;

    [SerializeField, TextArea]
    private string TextTemplate = "";

    [SerializeField, TextArea]
    private string TextPreview = "";

    [SerializeField]
    private List<ValueEntry> ValueInText;

    public string Text => TextPreview;

    private Dictionary<string, string> _valueTable;

    private ListToDictionaryConverter<string, string, ValueEntry> _converter =
      new ListToDictionaryConverter<string, string, ValueEntry>("Name", "DefaultValue");

    private void OnValidate()
    {
      _valueTable = _converter.CreateDictionaryFrom(ValueInText);      
      
      UpdateText();
    }

    private void UpdateText()
    {
      TextPreview = TextTemplate;

      var newTextBuilder = new StringBuilder(TextTemplate);

      foreach (KeyValuePair<string, string> valueEntry in _valueTable)
      {
        newTextBuilder.Replace($"{_leftSeparator}{valueEntry.Key}{_rightSeparator}", valueEntry.Value);
      }

      TextPreview = newTextBuilder.ToString();
    }

    public void InsertValueAt(string Name, object newValue)
    {
      if (_valueTable.ContainsKey(Name))
      {
        _valueTable[Name] = newValue == null ? "null" : newValue.ToString();
        UpdateText();
      }
      else
      {
        Debug.LogWarning($"{nameof(Name)} [{Name}] does not exit in this text template");
      }
    }

  }
}
