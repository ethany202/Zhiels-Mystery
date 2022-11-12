using UnityEngine;

public class GameLoadData : MonoBehaviour
{

    //public GameObject districtText;
    public GameObject healthText;
    public Transform spawnPos;

    void Start()
    {
        //LoadSceneLogic.SetDistrictText(districtText);
        LoadSceneLogic.SetHealthBarUI(healthText);
        LoadSceneLogic.spawnPos = spawnPos;
    }

    public void SaveCurrentState()
    {
        SaveSystem.SavePlayerState(LoadSceneLogic.player);
    }
}
