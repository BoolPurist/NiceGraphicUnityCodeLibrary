using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary.Component
{
  /// <summary>
  /// Moves an object with rigid-body along the global or local axis.
  /// </summary>
  [RequireComponent(typeof(Rigidbody))]
  public abstract class PlayerMovement : MonoBehaviour
  {
    /// <summary>
    /// Determines if the object is moved along global axis or its local axis, orientation influenced by rotation.
    /// </summary>
    public enum MovementAxisLevel { Global, Local }

    // movement direction from -1 to 1.
    protected float _axisX;
    protected float _axisY;
    protected float _axisZ;

    protected Rigidbody _rb;

    [SerializeField]
    [Tooltip("Global: is moved  interdependently on its rotation. Local: is moved dependently on its rotation.")]
    private MovementAxisLevel _CurrentAxisLevel = MovementAxisLevel.Global;

    [SerializeField]
    [Tooltip("Object is moved along x direction.")]
    private bool X = true;
    [SerializeField]
    [Tooltip("Object is moved along y direction.")]
    private bool Y = true;
    [SerializeField]
    [Tooltip("Object is moved along z direction.")]
    private bool Z = true;

    #region Setters for freezing certain axis.
    public void EnableXMovement() => X = true;
    public void EnableYMovement() => Y = true;
    public void EnableZMovement() => Z = true;
    public void DisableXMovement() => X = false;
    public void DisableYMovement() => Y = false;
    public void DisableZMovement() => Z = false;
    #endregion


    #region Public api to trigger movement in certain direction
    
    public void OnMoveX(float inputXAxis)
    {
      if (X)
      {
        _axisX = ClampInput(inputXAxis);
      }
    }

    public void OnMoveY(float inputYAxis)
    {
      if (Y)
      {
        _axisY = ClampInput(inputYAxis);
      }
    }

    public void OnMoveZ(float inputZAxis)
    {
      if (Z)
      {
        _axisZ = ClampInput(inputZAxis);
      }
    }

    #endregion


    protected virtual void Start()
    {
      _rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
      _rb.velocity = Vector3.zero;

      if (_CurrentAxisLevel == MovementAxisLevel.Global)
      {
        if (X) ApplyXMovement();
        if (Y) ApplyYMovement();
        if (Z) ApplyZMovement();
      }
      else
      {
        if (X) ApplyLocalXMovement();
        if (Y) ApplyLocalYMovement();
        if (Z) ApplyLocalZMovement();
      }

      ResetInput();
    }

    // Used to make sure that to the next fixed update the inputs are zero if no inputs are given.
    private void ResetInput()
    {
      _axisX = 0f;
      _axisY = 0f;
      _axisZ = 0f;      
    }

    #region 
    private void ApplyXMovement() => _rb.velocity += Vector3.right * (ApplyMovement() * _axisX);
    private void ApplyYMovement() => _rb.velocity += Vector3.up * (ApplyMovement() * _axisY);
    private void ApplyZMovement() => _rb.velocity += Vector3.forward * (ApplyMovement() * _axisZ);

    private void ApplyLocalXMovement() => _rb.velocity += transform.right * (ApplyMovement() * _axisX);
    private void ApplyLocalYMovement() => _rb.velocity += transform.up * (ApplyMovement() * _axisY);
    private void ApplyLocalZMovement() => _rb.velocity += transform.forward * (ApplyMovement() * _axisZ);
    #endregion

    protected abstract float ApplyMovement();

       
    private float ClampInput(float inputAxis)
    { 
      inputAxis = inputAxis < 0 ? -1f : inputAxis;
      inputAxis = inputAxis > 0 ? 1f : inputAxis;
      return inputAxis;
    }
  } 
}