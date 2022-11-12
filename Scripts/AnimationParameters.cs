using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParameters : MonoBehaviour
{
    public static Dictionary<string, int> parameters = new Dictionary<string, int>()
    {
        {"isIdle", Animator.StringToHash("isIdle") },
        {"isWalkingBackwards", Animator.StringToHash("isWalkingBackwards") },
        {"isCrouching", Animator.StringToHash("isCrouching") },
        {"isMoving", Animator.StringToHash("isMoving") },
        {"isWalking", Animator.StringToHash("isWalking") },
        {"isRunning", Animator.StringToHash("isRunning") }
    };

    public static Dictionary<string, int> floats = new Dictionary<string, int>()
    {
        {"velocityNormalized", Animator.StringToHash("velocityNormalized")},
        {"crouching", Animator.StringToHash("crouching") }
    };

    public static Dictionary<string, int> triggers = new Dictionary<string, int>()
    {
        {"jump", Animator.StringToHash("jump") },
        {"shoot", Animator.StringToHash("shoot") }
    };
}
