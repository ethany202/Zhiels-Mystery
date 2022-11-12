using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{

    public Transform parentObj;
    public bool selectedCharacter = false;
    //public GameObject defaultSkin;

    public GameObject selectButton, lockedIn;

    public CharacterSelectController characterSelectController;

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

        HashSet<string> selectedCharacters = characterSelectController.GetSelectedCharacters();

        if (selectedCharacters.Contains(CustomizedData.GetCharacterName()))
        {
            return;
        }

        selectButton.SetActive(false);
        lockedIn.SetActive(true);

        characterSelectController.SetPlayerReady();
    }

}
