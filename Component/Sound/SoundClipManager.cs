using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  /// <summary>
  /// Component to swap different audio clips for an audio source during play time
  /// </summary>
  [RequireComponent(typeof(AudioSource))]
  public class SoundClipManager : MonoBehaviour
  {
    /// <summary>
    /// Class to type an audio clip with a certain name to find it later during play time
    /// </summary>
    [System.Serializable]
    public class ClipEntry
    {
      /// <summary>
      /// Name as parameter in the method <see cref="ChangeToAudioClip"/> to search for the specific audio clip in 
      /// </summary>     
      public string _Name;
      /// <summary>
      /// A clip which can be changed to during the play time with the method <see cref="ChangeToAudioClip"/> 
      /// </summary>
      public AudioClip _Clip;
    }

    [SerializeField]
    [Tooltip("List of clips which can be changed to during play time. The name is key to provide as argument for the public function  ChangeToAudioClip")]
    private List<ClipEntry> _ClipEntries = new List<ClipEntry>();
    
    private readonly Dictionary<string, AudioClip> _Entries = new Dictionary<string, AudioClip>();        
    private AudioSource _MusicSource;

    /// <summary>
    /// Changes the clip audio of attached source audio the a clip with the name equal to the given name exits.
    /// </summary>
    /// <param name="nameOfAudioClip">
    /// Name to use to search for the clip to swap in
    /// </param>
    /// <param name="playOnLoop">
    /// If true sound will be played in a loop
    /// </param>
    /// <remarks>
    /// If nameOfAudioClip is null, empty or no clip was stored to this key, nothing happens
    /// </remarks>
    public void ChangeToAudioClip(string nameOfAudioClip, bool playOnLoop = false)
    {

      if (_Entries.ContainsKey(nameOfAudioClip))
      {
        ChangeClip(_Entries[nameOfAudioClip], playOnLoop);
      }
      else
      {
        Debug.LogWarning($"For {nameof(nameOfAudioClip)} = {nameOfAudioClip} was no entry found !");
      }

    }

    private void ChangeClip(AudioClip newClip, bool playOnLoop)
    {
      _MusicSource.Stop();
      _MusicSource.loop = playOnLoop;
      _MusicSource.clip = newClip;
      _MusicSource.Play();
    }

    private void Awake()
    {
      _MusicSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
      _Entries.Clear();

      for (int i = 0; i < _ClipEntries.Count; i++)
      {
        string name = _ClipEntries[i]._Name;
        AudioClip clip = _ClipEntries[i]._Clip;

        if (!string.IsNullOrEmpty(name) && !_Entries.ContainsKey(name) && clip != null)
        {
          _Entries.Add(name, clip);
        }
        
      }      
    }

  } 
}