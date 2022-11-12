using UnityEngine;

public class InstructionsHandler : MonoBehaviour
{

    public GameObject instructions;

    void Awake()
    {
        LoadSceneLogic.SetInstructionsGameObject(instructions);
    }

}
