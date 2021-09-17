using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


namespace NiceGraphicLibrary.Component
{
  /// <summary>
  /// Provides input management for keyboard inputs to control a component inheriting from type <see cref="PlayerMovement"/> 
  /// </summary>
  [RequireComponent(typeof(PlayerMovement))]
  public class PlayerKeyboardMovementInput : MonoBehaviour
  {
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
    
    private PlayerMovement _movementComponent;

    private void Start()
    {
      _movementComponent = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
      float x = ClampInput(MoveRight);
      x -= ClampInput(MoveLeft);

      float y = ClampInput(MoveUp);
      y -= ClampInput(MoveDown);

      float z = ClampInput(MoveForward);
      z -= ClampInput(MoveBack);

      _movementComponent.OnMoveX(x);
      _movementComponent.OnMoveY(y);
      _movementComponent.OnMoveZ(z);
    }

    private float ClampInput(KeyCode pressed) => Input.GetKey(pressed) ? 1f : 0f;
  } 
}