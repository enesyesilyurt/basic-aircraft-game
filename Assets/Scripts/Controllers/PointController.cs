using System;
using UnityEngine;

public class PointController : MonoBehaviour, ICollectable
{
    private const float AnimTime = .3f;
    #region Methods

    public void Setup()
    {
        gameObject.SetActive(true);

        LeanTween.cancel(gameObject);
        LeanTween.rotateAround(gameObject, transform.forward, 360, 1f).setLoopClamp();
    }

    public void Collect()
    {
        ScoreController.Instance.CurrentScore++;
        
        DespawnAnim();
    }

    private void DespawnAnim()
    {
        LeanTween.scaleX(gameObject, 0, AnimTime).setEaseInCubic().setLoopPingPong(1);
        LeanTween.scaleY(gameObject, 0, AnimTime).setEaseInCubic().setLoopPingPong(1);
        LeanTween.alpha(gameObject, 0, AnimTime).setLoopPingPong(1);

        LeanTween.delayedCall(AnimTime, ()=> SimplePool.Despawn(gameObject));
    }

    #endregion
}
