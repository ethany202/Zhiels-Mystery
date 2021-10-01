using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredAnimations : MonoBehaviour
{

    public static Dictionary<string, int> clips = new Dictionary<string, int>() 
    {
        {"Idle", Animator.StringToHash("Breathing Idle") },
        {"Walking", Animator.StringToHash("Walking") },
        {"Chair Sitting", Animator.StringToHash("Chair Sitting") },
        {"Running", Animator.StringToHash("Running") },
        {"Walking Backwards", Animator.StringToHash("Walking Backwards") },
        {"Jump", Animator.StringToHash("Jump") },
        {"Ground Sitting", Animator.StringToHash("Ground Sitting") },
        {"Running Jump", Animator.StringToHash("Running Jump") },
        {"Walking Jump", Animator.StringToHash("Walking Jump") }
    };




}
