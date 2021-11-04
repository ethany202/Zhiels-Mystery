using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManagement : MonoBehaviour
{
    private static int currentRole = -1;

    public static int GetRole()
    {
        return currentRole;
    }

    public static void SetRole(int r)
    {
        currentRole = r;
    }
}
