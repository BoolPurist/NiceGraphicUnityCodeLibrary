using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NiceGraphicLibrary.Utility
{
  public class NoRepeatedRandomPicker<TElement>
  {
    private List<TElement> _innerCollection = new List<TElement>();
    private int _currentIndex = -1;

    public NoRepeatedRandomPicker(IEnumerable<TElement> collectionToPickFrom)
    {
      SetANew(collectionToPickFrom);
    }

    public TElement Next
    {
      get
      {
        if (_innerCollection.Count == 0)
        {
          return default(TElement);
        }
        else
        {          
          if (_currentIndex == _innerCollection.Count)
          {
            Reset();
          }

          _currentIndex++;
          TElement nextElement = _innerCollection[_currentIndex];

          return nextElement;
        }
      }
    }

    public void Reset()
    {
      RandomUtility.Shuffle(_innerCollection);
      _currentIndex = 0;
    }

    public void SetANew(IEnumerable<TElement> collectionToPickFrom)
    {
      if (collectionToPickFrom != null)
      {
        _innerCollection = new List<TElement>(collectionToPickFrom);
        Reset();
      }
    }
  } 
}