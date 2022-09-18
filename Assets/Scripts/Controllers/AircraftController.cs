using System;
using Core;
using UnityEngine;

public class AircraftController : MonoSingleton<AircraftController>
{
    #region Components

    private Rigidbody _body;
    private Rigidbody body => _body ??= GetComponent<Rigidbody>();

    #endregion

    #region Variables

    private Aircraft aircraft;

    private float activePitch;
    private float activeRoll;
    private float targetSpeed = 0;
    private float engineMinPower;
    private bool isWarningGiven = false;

    #endregion

    #region Actions

    public event Action Crashed;
    public event Action<bool> WarningSituationChanged;

    #endregion

    #region Unity Methods

    private void FixedUpdate() 
    {
        SetRotation();
        SetMovement();
        CheckDistanceToPath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectable>(out ICollectable collectable))
        {
            collectable.Collect();
        }
    }

    #endregion

    #region Methods

    public void Setup(Aircraft aircraft)
    {
        this.aircraft = aircraft;
        GameManager.AfterStateChanged += OnAfterStateChanged;
    }

    private void CheckDistanceToPath()
    {
        var distance = Vector3.Distance(PathController.Instance.PathCreator.path.GetClosestPointOnPath(transform.position), transform.position);
        if(distance > 200)
        {
            Crashed?.Invoke();
        }
        else if (distance > 100)
        {
            if (!isWarningGiven)
            {
                isWarningGiven = true;
                WarningSituationChanged?.Invoke(true);
            }
        }
        else
        {
            if (isWarningGiven)
            {
                isWarningGiven = false;
                WarningSituationChanged?.Invoke(false);
            }
        }
    }

    private void SetMovement()
    {
        if (transform.position.y > .3f)
        {
            if (UIController.Instance.EnginePowerSlider.value < .2f)
            {
                engineMinPower = .2f;
                SetVelocity((aircraft.EnginePower - engineMinPower) * .3f);
                transform.RotateAround(transform.position, transform.right, .1f);
            }
            else
            {
                engineMinPower = UIController.Instance.EnginePowerSlider.value;
                SetVelocity((aircraft.EnginePower - engineMinPower) * .3f);
            }
        }
        else
        {
            if (transform.position.z > PathController.Instance.FinalPointPosition.z && engineMinPower < .01f)
            {
                GameManager.Instance.ChangeGameState(GameState.Win); 
            }

            engineMinPower = UIController.Instance.EnginePowerSlider.value;
            SetVelocity(aircraft.EnginePower - engineMinPower + 2);
        }
    }

    private void SetRotation()
    {
        activeRoll = InputController.Instance.Joystick.Horizontal * aircraft.RollPower * Time.deltaTime;
        activePitch = InputController.Instance.Joystick.Vertical * aircraft.PitchPower * Time.deltaTime;

        transform.Rotate
        (
            activePitch * aircraft.PitchPower * engineMinPower, 
            activeRoll * aircraft.RollPower * .1f * engineMinPower, 
            -activeRoll * aircraft.RollPower * engineMinPower
        );
    }

    private void SetVelocity(float gravityForce)
    {
        targetSpeed = Mathf.Lerp(targetSpeed, aircraft.EnginePower * engineMinPower, .01f);
        body.velocity = targetSpeed * transform.forward;
        body.AddForce(Physics.gravity * gravityForce, ForceMode.Acceleration);
    }

    private void RestartAircraft()
    {
        body.isKinematic = false;
        gameObject.SetActive(true);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        body.velocity = Vector3.zero;
        engineMinPower = 0;
        targetSpeed = 0;
    }

    private void OnCrashed()
    {
        body.isKinematic = true;
        gameObject.SetActive(false);
    }

    #endregion

    #region Callbacks

    private void OnCollisionEnter(Collision other) 
    {
        if (body.velocity.magnitude > 13f || transform.position.y > .5f)
        {
            Crashed?.Invoke();
        }
    }

    private void OnAfterStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                RestartAircraft();
                break;
            
            case GameState.Lose:
                OnCrashed();
                break;
        }
    }

    #endregion
}
