using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityProperties : MonoBehaviour
{

    private static int qualityIndex=1;
    private static int vsyncIndex;

    private static int resolutionWidth, resolutionHeight;
    private static bool isFullscreen;

    private static int fpsCap;

    private static int particleRaycastBudget;

    public static void SetQualityIndex(int val)
    {
        qualityIndex = val;
    }

    public static int GetQualityIndex()
    {
        return qualityIndex;
    }

    public static void SetVSyncIndex(int val)
    {
        vsyncIndex = val;
    }

    public static int GetVSyncIndex()
    {
        return vsyncIndex;
    }

    public static void SetResolution(int w, int h)
    {
        resolutionWidth = w;
        resolutionHeight = h;
    }

    public static int GetResolutionWidth()
    {
        return resolutionWidth;
    }

    public static int GetResolutionHeight()
    {
        return resolutionHeight;
    }

    public static void SetFullscreen(bool val)
    {
        isFullscreen = val;
    }

    public static bool GetFullscreen()
    {
        return isFullscreen;
    }

    public static void SetFPSCap(int val)
    {
        fpsCap = val;
    }

    public static int GetFPSCap()
    {
        return fpsCap;
    }

    public static int GetParticleRaycastBudget()
    {
        return particleRaycastBudget;
    }

    public static void SetParticleRaycastBudget(int val)
    {
        particleRaycastBudget = val;
    }
}
