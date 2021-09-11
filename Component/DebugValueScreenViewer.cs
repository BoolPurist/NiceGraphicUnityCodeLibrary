using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiceGraphicLibrary
{
  /// <summary>
  /// Other components can use the API of this one 
  /// To show values to the screen.
  /// </summary>
  public class DebugValueScreenViewer : MonoBehaviour
  {
    [SerializeField]
    [Min(0)]
    [Tooltip(" Size of the margin of the whole viewer ")]
    private int Margin = 10;
    [SerializeField]
    [Min(0)]
    [Tooltip(" Space between 2 text lines in vertical direction ")]
    private int Spacing = 10;
    [SerializeField]
    [Tooltip(" Where to place the text lines on screen, top left, bottom right etc ")]
    private TextAnchor ContentAlignment = TextAnchor.UpperLeft;

    [SerializeField]
    private Color FontColor = Color.black;
    [SerializeField]
    [Min(0)]
    private int FontSize = 20;

    private Dictionary<string, Text> _textLines;
    private GameObject _canvaseContainer;
    private VerticalLayoutGroup _layout;

    // If true, the viewer and text lines can be changed in respect of appearance
    // in the inspector after the start of the game.
    private bool _isSetUp = false;

    /// <summary>
    /// Either creates a new text line to show an arbitrary value to the screen during the game or
    /// updates the value displayed to the screen.
    /// </summary>
    /// <param name="valueName">
    /// Lable assigned to the value. Is displayed to the left of the shown value
    /// If this value was not added to this component then a new text line will be displayed.
    /// Should not be null or empty
    /// </param>
    /// <param name="value">
    /// Value to be shown right to the lable
    /// </param>    
    public void UpdateValue(string valueName, object value)
    {
      CheckValueName(valueName);

      string newValue = value == null ? "null" : value.ToString();

      if (_textLines.ContainsKey(valueName))
      {
        var textComp = _textLines[valueName].GetComponent<Text>();
        textComp.text = CreateContentForTextLine(valueName, newValue);
      }
      else
      {
        CreateOneTextLine(valueName, newValue);
      }
    }

    /// <summary>
    /// Removes one text line for given lable if it exits
    /// </summary>
    /// <param name="valueName">
    /// Name  of the lable. Used to determine which text line needs to be deleted.
    /// </param>
    /// <return>
    /// Returns true if a text line was found to be deleted.
    /// </return>
    public bool RemoveValue(string valueName)
    {
      CheckValueName(valueName);

      if (_textLines.ContainsKey(valueName))
      {
        _textLines.Remove(valueName);
        return true;
      }

      return false;
    }

    private void CheckValueName(string valueName)
    {
      if (valueName == null)
      {
        Debug.LogError($"{nameof(valueName)} must not be null !");
      }
      else if (string.IsNullOrWhiteSpace(valueName))
      {
        Debug.LogError($"{nameof(valueName)} is empty !");
      }
    }

    #region Update layout and appearance

    private void OnValidate()
    {
      if (_isSetUp)
      {
        ApplyingLayout();
        UpdateTextFieldStyles();
      }
    }

    private void ApplyingLayout()
    {
      _layout.padding.left = Margin;
      _layout.padding.right = Margin;
      _layout.padding.top = Margin;
      _layout.padding.bottom = Margin;
      _layout.spacing = Spacing;
      _layout.childAlignment = ContentAlignment;
    }

    private void UpdateTextFieldStyles()
    {
      foreach (Text text in _textLines.Values)
      {
        if (text != null)
        {
          text.color = FontColor;
          text.fontSize = FontSize;
        }        
      }
    }

    #endregion

    #region Creation of view

    private void Awake()
    {
      _textLines = new Dictionary<string, Text>();
      CreateCanvas();
      _isSetUp = true;
    }

    private void CreateCanvas()
    {
      
      _canvaseContainer = new GameObject("Screen Value Viewer");

      // Constructing adding components
      _canvaseContainer.AddComponent<RectTransform>();
      _canvaseContainer.AddComponent<Canvas>();
      _canvaseContainer.AddComponent<CanvasScaler>();
      _canvaseContainer.AddComponent<GraphicRaycaster>();
      _canvaseContainer.AddComponent<VerticalLayoutGroup>();

      // Adjusting canvas
      var canvas = _canvaseContainer.GetComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      _canvaseContainer.transform.SetParent(transform);

      // Setting up layout for later text lines
      _layout = _canvaseContainer.GetComponent<VerticalLayoutGroup>();
      _layout.childForceExpandHeight = false;
      _layout.childForceExpandWidth = false;
      ApplyingLayout();      
    }

    private void CreateOneTextLine(string valueName, string value)
    {
      // Constructing text line
      var root = new GameObject() { name = valueName };      
      root.AddComponent<RectTransform>();
      root.AddComponent<CanvasRenderer>();
      root.AddComponent<Text>();      
      root.transform.SetParent(_canvaseContainer.transform);
      
      // Adjusting text line
      var newText = root.GetComponent<Text>();
      newText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
      newText.color = FontColor;
      newText.fontSize = FontSize;
      newText.text = CreateContentForTextLine(valueName, value);
      
      _textLines.Add(valueName, newText);      
    }

    #endregion

    private string CreateContentForTextLine(string valueName, string value) => $"{valueName}: {value}";
  } 
}