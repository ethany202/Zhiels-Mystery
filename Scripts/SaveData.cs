using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public float[] playerPosition; // [0] = x, [1] = y, [2] = z
    public float playerHealth;

    public bool holdingPistol;
    public bool holdingKnife;

    public string objectName;

    public string character;

    public int examPhase;

    public SaveData(CharacterManager player)
    {
        playerPosition = new float[3];
        playerPosition[0] = player.body.transform.position.x;
        playerPosition[1] = player.body.transform.position.y;
        playerPosition[2] = player.body.transform.position.z;

        playerHealth = player.Health;

        //if (player.IsGrabbingObject())
        //{
        //    if(player.GetObjectBody().gameObject.layer == LayerMask.NameToLayer("Knife"))
        //    {
        //        holdingKnife = true;
        //        holdingPistol = false;
        //    }
        //    else
        //    {
        //        holdingPistol = true;
        //        holdingKnife = false;
        //    }
        //    objectName = player.GetObjectBody().name;
        //}

        character = "FPS Character";//CustomizedData.GetCharacterName();
        examPhase = SceneManager.GetActiveScene().buildIndex;
    }
}
