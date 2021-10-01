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
        {"isCrouchWalking", Animator.StringToHash("isCrouchWalking") },   
        {"holdingKnife", Animator.StringToHash("holdingKnife") },
        {"isAiming", Animator.StringToHash("isAiming") },
        {"halfAiming", Animator.StringToHash("halfAiming") },
        {"isMoving", Animator.StringToHash("isMoving") },       
    };

    public static Dictionary<string, int> floats = new Dictionary<string, int>()
    {
        { "velocityNormalized", Animator.StringToHash("velocityNormalized")},
        {"knife", Animator.StringToHash("knife") },
        {"crouching", Animator.StringToHash("crouching") }
    };

    public static Dictionary<string, int> triggers = new Dictionary<string, int>()
    {
        {"idlePunch", Animator.StringToHash("idlePunch") },
        {"kick", Animator.StringToHash("kick") },
        {"jump", Animator.StringToHash("jump") },
        {"punch", Animator.StringToHash("punch") },
        {"slide", Animator.StringToHash("slide") },
        {"shoot", Animator.StringToHash("shoot") },
        {"halfShoot", Animator.StringToHash("halfShoot") }
    };
}
