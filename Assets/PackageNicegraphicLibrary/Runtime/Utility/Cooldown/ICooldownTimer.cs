using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceGraphicLibrary.Utility.Cooldown
{
  /// <summary>
  /// Demands a cool down which returns a value between 0 and 1 for passed time within an interval 
  /// </summary>
  interface ICooldownTimer
  {   
    /// <summary>
    /// Deletes passed time as if the cool down just started.
    /// </summary>
    void Reset();
    /// <summary>
    /// Returns a value between 0 and 1. 0 for no time passed and 1 for the cool down has worn off.
    /// </summary>
    float PassedTimeFactor { get; }
    /// <summary>
    /// Returns the passed time between zero and the given end time
    /// </summary>
    float PassedTime { get; }
    /// <summary>
    /// Returns true if the cool down has worn off completely
    /// </summary>
    bool WornOff { get; }

    /// <summary>
    /// Sets time until the cool down wears off the parameter.
    /// </summary>
    /// <param name="newEndTime">
    /// Negative value will be converted to a positive one
    /// </param>
    void SetNewEndTime(float newEndTime);

    
  }
}
