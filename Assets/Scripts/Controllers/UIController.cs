using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Core;
using System;

public class UIController : MonoSingleton<UIController>
{
    #region SerializeFields

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI warningText;

    [SerializeField]
    private Slider enginePowerSlider;

    [SerializeField]
    private GameObject winPanelGO;
    
    [SerializeField]
    private GameObject crashEffectPanelGO;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private List<GameObject> stars; 

    #endregion

    #region Props

    public Slider EnginePowerSlider => enginePowerSlider;

    #endregion

    #region Unity Methods

    private void Awake() 
    {
        ScoreController.Instance.ScoreIncreased += OnScoreIncreased;
        GameManager.AfterStateChanged += OnAfterStateChanged;
        AircraftController.Instance.WarningSituationChanged += OnWarningGiven;

        restartButton.onClick.AddListener(()=> LevelController.Instance.RestartLevel());
    }

    private void OnWarningGiven(bool isGiven)
    {
        warningText.gameObject.SetActive(isGiven);
    }

    #endregion

    #region Methods

    private void SetupUI()
    {
        scoreText.text = PathController.Instance.PointCount.ToString() + " / 0";
        enginePowerSlider.value = 0;
        warningText.gameObject.SetActive(false);
        crashEffectPanelGO.SetActive(false);
        winPanelGO.SetActive(false);

        foreach (var star in stars)
        {
            star.SetActive(false);
        }
    }

    private void SetupWinUI()
    {
        winPanelGO.SetActive(true);
        var earnedStarCount = ScoreController.Instance.CurrentScore / (PathController.Instance.PointCount / 3);
        for (int i = 0; i < earnedStarCount; i++)
        {
            stars[i].SetActive(true);
        }
    }

    private void PlayCrashEffect()
    {
        crashEffectPanelGO.SetActive(true);
        crashEffectPanelGO.transform.localScale = Vector3.one * .01f;
        LeanTween.scale(crashEffectPanelGO, Vector3.one, .3f).setEaseOutExpo().setLoopPingPong(1).setOnComplete(() => crashEffectPanelGO.SetActive(false));
    }

    #endregion

    #region Callbacks

    private void OnAfterStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                SetupUI();
                break;

            case GameState.Win:
                SetupWinUI();
                break;

            case GameState.Lose:
                PlayCrashEffect();
                break;
        }
    }

    private void OnScoreIncreased()
    {
        scoreText.text = PathController.Instance.PointCount.ToString() + " / " +ScoreController.Instance.CurrentScore.ToString();
    }

    #endregion
}
