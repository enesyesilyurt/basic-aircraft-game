using UnityEngine;
using Core;

public class InputController : MonoSingleton<InputController>
{
    #region SerializeFields

    [SerializeField]
    private FloatingJoystick joystick;

    #endregion

    #region Props

    public FloatingJoystick Joystick => joystick;

    #endregion
}
