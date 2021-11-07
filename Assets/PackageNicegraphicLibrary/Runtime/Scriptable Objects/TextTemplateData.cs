using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public string GetPreviewText() => TextPreview;

    public Dictionary<string, string> DefaultValuesCopy => new Dictionary<string, string>(_defaultValueTable);

    [SerializeField, TextArea]
    private string TextPreview = "";

    [SerializeField]
    private List<ValueEntry> ValueInText;

    private Dictionary<string, string> _defaultValueTable = new Dictionary<string, string>();

    private ListToDictionaryConverter<string, string, ValueEntry> _converter =
      new ListToDictionaryConverter<string, string, ValueEntry>("Name", "DefaultValue");

    private void OnValidate()
    {
      _defaultValueTable = _converter.CreateDictionaryFrom(ValueInText);
      TextPreview = GetUpdatedText(_defaultValueTable);
    }

       

    private string GetUpdatedText(Dictionary<string, string> values)
    {     
      var newTextBuilder = new StringBuilder(TextTemplate);

      foreach (KeyValuePair<string, string> valueEntry in values)
      {
        newTextBuilder.Replace($"{_leftSeparator}{valueEntry.Key}{_rightSeparator}", valueEntry.Value);
      }

      return newTextBuilder.ToString();
    }

    public string GetTextWithInsertedValue(
      in string Name, 
      in object newValue, 
      Dictionary<string, string> currentValues
      )
    {
      if (currentValues.ContainsKey(Name))
      {
        currentValues[Name] = newValue == null ? "null" : newValue.ToString();        
      }
      else
      {
        Debug.LogWarning($"{nameof(Name)} [{Name}] does not exit in this text template.");
      }

      return GetUpdatedText(currentValues);
    }

    

   

  }
}
