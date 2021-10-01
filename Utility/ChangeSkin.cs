using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{

    public Transform parentObj;
    public GameObject defaultSkin;

    void Start()
    {
        //Instantiate(defaultSkin, Vector3.zero, Quaternion.identity, parentObj);
        SetCurrentSkin(defaultSkin);
    }

    public void SetCurrentSkin(UnityEngine.Object obj)
    {
        GameObject o = GameObject.FindWithTag("Character");
        Destroy(parentObj.GetChild(0).gameObject);
        Instantiate(obj, Vector3.zero, Quaternion.identity, parentObj);

        int spaceIndex = obj.name.IndexOf(' ');
        string firstHalf = obj.name.Substring(0, spaceIndex + 1);
        CustomizedData.SetSkinName(firstHalf + "Skin");
    }
}
