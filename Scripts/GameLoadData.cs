using UnityEngine;

public class GameLoadData : MonoBehaviour
{

    public void SaveCurrentState()
    {
        SaveSystem.SavePlayerState(LoadSceneLogic.player);
    }
}
