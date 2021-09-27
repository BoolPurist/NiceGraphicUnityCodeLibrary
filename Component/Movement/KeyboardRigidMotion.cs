using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


namespace NiceGraphicLibrary.Component
{
  /// <summary>
  /// Provides input management for keyboard inputs to control a component inheriting from type <see cref="RigidBodyMovement"/> 
  /// </summary>
  
  public class KeyboardRigidMotion : MonoBehaviour
  {
    [SerializeField]
    private RigidGeometryMotion _Motion;

    [SerializeField]
    private KeyCode MoveLeft = KeyCode.A;
    [SerializeField]
    private KeyCode MoveRight = KeyCode.D;
    [SerializeField]
    private KeyCode MoveUp = KeyCode.W;
    [SerializeField]
    private KeyCode MoveDown = KeyCode.S;
    [SerializeField]
    private KeyCode MoveForward = KeyCode.Space;
    [SerializeField]
    private KeyCode MoveBack = KeyCode.C;
    
    private void Start()
    {
      if (_Motion == null)
      {
        Debug.LogWarning($"In object {name} in the component {nameof(KeyboardRigidMotion)} the property {nameof(_Motion)}");
      }
    }

    private void Update()
    {
      if (_Motion != null)
      {
        float x = ClampInput(MoveRight);
        x -= ClampInput(MoveLeft);

        float y = ClampInput(MoveUp);
        y -= ClampInput(MoveDown);

        float z = ClampInput(MoveForward);
        z -= ClampInput(MoveBack);

        _Motion.OnXMotion(x);
        _Motion.OnYMotion(y);
        _Motion.OnZMotion(z);

      }
    }

    private float ClampInput(KeyCode pressed) => Input.GetKey(pressed) ? 1f : 0f;
  } 
}