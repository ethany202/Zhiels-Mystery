using System.Collections.Generic;
using UnityEngine;

public class ControlsConstants : MonoBehaviour
{

    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public static Dictionary<string, KeyCode> GetKeys()
    {
        return keys;
    }

    public static void SetDefaultKeys()
    {
        keys.Add("sprint", KeyCode.LeftControl);
        keys.Add("crouch", KeyCode.LeftShift);
        keys.Add("jump", KeyCode.Space);
        keys.Add("open", KeyCode.E);
        keys.Add("grab", KeyCode.G);
        keys.Add("drop", KeyCode.Z);
        keys.Add("targetData", KeyCode.Alpha1);
        keys.Add("slide", KeyCode.F);

        // immutable keys:
        keys.Add("forward", KeyCode.W);
        keys.Add("backward", KeyCode.S);
        keys.Add("right", KeyCode.D);
        keys.Add("left", KeyCode.A);
        keys.Add("attack", KeyCode.Mouse0);
        keys.Add("scope", KeyCode.Mouse1);
    }
}
