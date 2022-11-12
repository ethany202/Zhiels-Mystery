using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class QualityController : MonoBehaviour
{

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    void Awake()
    {
        QualitySettings.SetQualityLevel(QualityProperties.GetQualityIndex());
        QualitySettings.vSyncCount = QualityProperties.GetVSyncIndex();

        Screen.SetResolution(QualityProperties.GetResolutionWidth(), QualityProperties.GetResolutionHeight(), QualityProperties.GetFullscreen());

        Application.targetFrameRate = QualityProperties.GetFPSCap();

        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }


        QualitySettings.particleRaycastBudget = QualityProperties.GetParticleRaycastBudget();
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {}
}
