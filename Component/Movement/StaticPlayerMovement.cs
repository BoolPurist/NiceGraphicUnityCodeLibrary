using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  public class StaticPlayerMovement : PlayerMovement
  {
    [SerializeField]
    [Min(0)]
    private float Speed = 1f;

    public void SetSpeed(float newSpeed) => Speed = Mathf.Abs(newSpeed);

    protected override float ApplyMovement() => Speed * Time.deltaTime;
  }
}