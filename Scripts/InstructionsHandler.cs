using UnityEngine;

public class InstructionsHandler : MonoBehaviour
{

    public GameObject instructionsLeft; // Primary instructions
    public GameObject instructionsRight; // secondary instructions

    void Awake()
    {
        LoadSceneLogic.SetInstructions(instructionsLeft);
        LoadSceneLogic.SetInstructionsSecondary(instructionsRight);
    }

}
