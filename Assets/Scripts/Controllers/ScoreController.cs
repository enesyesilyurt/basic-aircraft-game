using System;
using Core;

public class ScoreController : MonoSingleton<ScoreController>
{
    #region Variables

    private int currentScore = 0;

    #endregion

    #region Props

    public int CurrentScore 
    { 
        get 
        {
            return currentScore;
        } 
        set 
        { 
            currentScore = value;
            ScoreIncreased?.Invoke();
        } 
    }

    #endregion

    #region Actions

    public event Action ScoreIncreased;

    #endregion

    #region Unity Methods

    private void Awake() 
    {
        GameManager.AfterStateChanged += OnAfterStateChanged;
    }

    #endregion

    #region Callbacks

    private void OnAfterStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                currentScore = 0;
                break;
        }
    }

    #endregion
}
