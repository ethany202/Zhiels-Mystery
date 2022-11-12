using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseAdventurer : MonoBehaviour
{

    public Transform parentObj;
    public bool selectedCharacter = false;
    //public GameObject defaultSkin;

    public GameObject selectButton, lockedIn;

    public void SetCurrentCharacter(UnityEngine.Object obj)
    {
        if (this.selectedCharacter)
            return;

        GameObject selectedCharacter = GameObject.FindWithTag("Character");

        if (selectedCharacter != null)
        {
            Destroy(selectedCharacter);
        }
        Instantiate(obj, Vector3.zero, Quaternion.identity, parentObj);

        int spaceIndex = obj.name.IndexOf(' ');
        string firstHalf = obj.name.Substring(0, spaceIndex);
        CustomizedData.SetCharacterName(firstHalf);

        //this.selectedCharacter = true;
    }

    public void SelectCharacter()
    {
        if (CustomizedData.GetCharacterName() == null)
        {
            return;
        }

        selectButton.SetActive(false);
        lockedIn.SetActive(true);

        SceneManager.LoadSceneAsync(2);

    }

}
