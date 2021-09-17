using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace NiceGraphicLibrary
{
  [RequireComponent(typeof(Rigidbody))]
  public abstract class PlayerMovement : MonoBehaviour
  {
    protected float _axisX;
    protected float _axisY;
    protected float _axisZ;

    protected Rigidbody _rb;

    [SerializeField]
    private bool X = true;
    [SerializeField]
    private bool Y = true;
    [SerializeField]
    private bool Z = true;

    public void EnableXMovement() => X = true;
    public void EnableYMovement() => Y = true;
    public void EnableZMovement() => Z = true;
    public void DisableXMovement() => X = false;
    public void DisableYMovement() => Y = false;
    public void DisableZMovement() => Z = false;


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

    protected virtual void Start()
    {
      _rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
      _rb.velocity = Vector3.zero;

      if (X) ApplyXMovement();
      if (Y) ApplyYMovement();
      if (Z) ApplyZMovement();

      ResetInput();
    }

    private void ResetInput()
    {
      _axisX = 0f;
      _axisY = 0f;
      _axisZ = 0f;
      
    }

    private void ApplyXMovement() => _rb.velocity += Vector3.right * (ApplyMovement() * _axisX);
    private void ApplyYMovement() => _rb.velocity += Vector3.up * (ApplyMovement() * _axisY);
    private void ApplyZMovement() => _rb.velocity += Vector3.forward * (ApplyMovement() * _axisZ);

    protected abstract float ApplyMovement();

       
    private float ClampInput(float inputAxis)
    { 
      inputAxis = inputAxis < 0 ? -1f : inputAxis;
      inputAxis = inputAxis > 0 ? 1f : inputAxis;
      return inputAxis;
    }
  } 
}