using System;
using Core;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    #region SerializeFields

    [SerializeField]
    private AircraftConfig aircraftConfig;

    #endregion

    #region Props

	public GameState State { get; private set; }

    #endregion

    #region Actions

    public static event Action<GameState> BeforeStateChanged;
	public static event Action<GameState> AfterStateChanged;

    #endregion

    #region Unity Methods

    private void Start() 
    {
        Initialize();
		LeanTween.init(100);
		ChangeGameState(GameState.Starting);
    }

    #endregion

    #region Methods

    private void Initialize()
    {
        var aircraft = new Aircraft(aircraftConfig);
        AircraftController.Instance.Setup(aircraft);
        
        AircraftController.Instance.Crashed += OnCrashed;
    }

    public void ChangeGameState(GameState newState)
    {
        BeforeStateChanged?.Invoke(State);
		State = newState;
		AfterStateChanged?.Invoke(State);
    }

    #endregion

    #region Callbacks

    private void OnCrashed()
    {
        ChangeGameState(GameState.Lose);

        LeanTween.delayedCall(2, ()=> ChangeGameState(GameState.Starting));
    }

    #endregion
}
