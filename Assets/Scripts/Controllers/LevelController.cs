using UnityEngine;
using Core;

public class LevelController : MonoSingleton<LevelController>
{
    public void RestartLevel()
    {
        GameManager.Instance.ChangeGameState(GameState.Starting);
    }
}
