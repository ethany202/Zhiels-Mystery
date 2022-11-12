using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{

    public CharacterManager characterBody;

    private void SyringeInjected()
    {
        characterBody.SyringeInjected();
    }
}
