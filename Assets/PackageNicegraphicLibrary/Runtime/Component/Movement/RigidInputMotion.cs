using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiceGraphicLibrary.Component.Movement
{
  public abstract class RigidInputMotion<TInput> : MonoBehaviour
  {

    [SerializeField]
    protected RigidGeometryMotion _Motion;

    [SerializeField]
    protected TInput MoveLeft;
    [SerializeField]
    protected TInput MoveRight;
    [SerializeField]
    protected TInput MoveUp;
    [SerializeField]
    protected TInput MoveDown;
    [SerializeField]
    protected TInput MoveForward;
    [SerializeField]
    protected TInput MoveBack;


    protected IKeyButtonProvider _inputProvider = new UnityKeyButtonProvider();

    private void Start()
    {
      if (_Motion == null)
      {
        Debug.LogWarning($"In object {name} in the component {nameof(KeyboardRigidMotion)} the property {nameof(_Motion)}");
      }
    }

    public void Update()
    {
      if (_Motion != null)
      {
        float x = ClampInput(MoveRight);
        x -= ClampInput(MoveLeft);

        float y = ClampInput(MoveUp);
        y -= ClampInput(MoveDown);

        float z = ClampInput(MoveForward);
        z -= ClampInput(MoveBack);

        if (_Motion != null)
        {
          _Motion.OnXMotion(x);
          _Motion.OnYMotion(y);
          _Motion.OnZMotion(z);
        }
      }
    }

    private float ClampInput(TInput pressed) => InputChecker(pressed) ? 1f : 0f;

    protected abstract bool InputChecker(TInput input);

    public void SetKeyButtonProvider(IKeyButtonProvider newProvider)
    {
      if (newProvider != null)
      {
        _inputProvider = newProvider;
      }
    }
  }
}