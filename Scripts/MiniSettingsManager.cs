using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;
using System.Diagnostics;
using System;

public class MiniSettingsManager : MonoBehaviour
{

    private string steamId = "";

    // Framerate Cap
    public Slider framerateSlider;
    public int maxFramerate = 300;
    public TMP_Text currentFPS;

    // public GameObject videoSettings;

    void Start()
    {

        if (SteamManager.Initialized)
        {
            steamId = SteamUser.GetSteamID().ToString();
        }
        openSettings = false;

        #region Window Settings
        FindResolutions();
        PopulateOptions();
        if (PlayerPrefs.HasKey(steamId + " Window Settings"))
        {
            if (PlayerPrefs.GetInt(steamId + " Window Settings") == 1)
            {
                windowSettings.text = "FULLSCREEN";
                Screen.fullScreen = true;
                isFull = true;
            }
            else
            {
                windowSettings.text = "WINDOWED";
                Screen.fullScreen = false;
                isFull = false;
            }
        }
        else
        {
            isFull = true;
            Screen.fullScreen = isFull;
            windowSettings.text = "FULLSCREEN";
        }
        if (PlayerPrefs.HasKey(steamId + " Resolution Width") && PlayerPrefs.HasKey(steamId + " Resolution Height"))
        {
            resolutionWidth = PlayerPrefs.GetInt(steamId + " Resolution Width");
            resolutionHeight = PlayerPrefs.GetInt(steamId + " Resolution Height");
            Screen.SetResolution(resolutionWidth, resolutionHeight, isFull);
           //  QualityProperties.SetResolution(resolutionWidth, resolutionHeight);
        }
        else
        {
            InitializeResolution();
        }
        #endregion

        #region Framerate Settings
        if (PlayerPrefs.HasKey(steamId + " Limit Framerate"))
        {
            int currentFramerate = PlayerPrefs.GetInt(steamId + " Limit Framerate");
            LimitFramerate(((float)currentFramerate) / maxFramerate);
        }
        else
        {
            LimitFramerate(0.25f);
        }
        #endregion

        #region Video Quality
        if (PlayerPrefs.HasKey(steamId + " Video Quality"))
        {
            ChangeVideoQuality(PlayerPrefs.GetInt(steamId + " Video Quality"));
            ChangeQualityText(PlayerPrefs.GetString(steamId + " Video Quality Text"));
        }
        /*else
        {
            ChangeVideoQuality(1);
            ChangeQualityText("MEDIUM");
        }*/
        #endregion

        #region Sensitivity
        if (PlayerPrefs.HasKey(steamId + " Normal Sensitivity"))
        {
            float sense = PlayerPrefs.GetFloat(steamId + " Normal Sensitivity") / maxSensitivity;
            ChangeNormalSensitivity(sense);

        }
        else
        {
            ChangeNormalSensitivity(50f);
        }

        #endregion

        #region FPS + Ping
        if (PlayerPrefs.HasKey(steamId + " showFPS"))
        {
            if (PlayerPrefs.GetInt(steamId + " showFPS") == 1)
            {
                SetFPS(true);
                SetFPSText("yes");
            }
            else
            {
                SetFPS(false);
                SetFPSText("no");
            }
        }
        else
        {
            SetFPS(false);
            SetFPSText("no");
        }
        #endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openSettings)
            {
                ExitToMainMenu();
            }
        }

    }

    void OnGUI()
    {
        SetDisplayResolutionLabel();
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("Quitting Game");
        Application.Quit();
    }

    public Slider normalSensitivity;
    public int maxSensitivity = 100;
    public TMP_Text senseText;
    public void ChangeNormalSensitivity()
    {
        float sensitivity = normalSensitivity.value * 100;
        int roundedSensitivity = (int)sensitivity;
        senseText.text = "" + roundedSensitivity;

        CustomizedData.normalSensitivity = roundedSensitivity;

        SetPlayerPrefs(steamId + " Normal Sensitivity", sensitivity);
    }

    public void ChangeNormalSensitivity(float val)
    {
        normalSensitivity.value = val;
        CustomizedData.normalSensitivity = val;

        ChangeNormalSensitivity();
    }

    public void LimitFramerate(float value)
    {
        framerateSlider.value = value;
        LimitFramerate();
    }

    public void LimitFramerate()
    {
        float frameRateCap = framerateSlider.value * 300;
        int fpsRounded = (int)frameRateCap;
        currentFPS.text = "" + fpsRounded;

        Application.targetFrameRate = fpsRounded;

        SetPlayerPrefs(steamId + " Limit Framerate", fpsRounded);
    }

    public GameObject settingsMenu;
    private bool openSettings;

    public void ExitToMainMenu()
    {
        settingsMenu.SetActive(false);
        openSettings = false;
    }

    public void SetOpenSettings(bool val)
    {
        openSettings = val;
    }

    public TMP_Text windowSettings;
    private bool isFull;

    public void WindowSettings()
    {
        isFull = !isFull;
        Screen.fullScreen = isFull;

        if (!isFull)
        {
            SetPlayerPrefs(steamId + " Window Settings", 0);
            windowSettings.text = "WINDOWED";
        }
        else
        {
            SetPlayerPrefs(steamId + " Window Settings", 1);
            windowSettings.text = "FULLSCREEN";
        }
    }

    private Resolution[] resolutions;
    private int resolutionWidth, resolutionHeight;
    public TMP_Dropdown resolutionOptions;
    public TMP_Text currentResolutionLabel;

    public void PopulateOptions()
    {
        HashSet<Resolution> resolutionsSet = new HashSet<Resolution>(resolutions);
        /*for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.options.Add(new TMP_Dropdown.OptionData(resolutions[i].width + " x " + resolutions[i].height));
        }
        resolutionOptions.onValueChanged.AddListener(delegate { ChangeResolution(currentResolutionLabel); });*/

        foreach (Resolution currentRes in resolutionsSet)
        {
            resolutionOptions.options.Add(new TMP_Dropdown.OptionData(currentRes.width + " x " + currentRes.height));
        }
        resolutionOptions.onValueChanged.AddListener(delegate { ChangeResolution(currentResolutionLabel); });
    }

    private void ChangeResolution(TMP_Text currentResolutionLabel)
    {
        int firstSpace = currentResolutionLabel.text.IndexOf(" ");
        int xChar = currentResolutionLabel.text.IndexOf("x");
        resolutionWidth = Convert.ToInt32(currentResolutionLabel.text.Substring(0, firstSpace));
        resolutionHeight = Convert.ToInt32(currentResolutionLabel.text.Substring(xChar + 2));

        Screen.SetResolution(resolutionWidth, resolutionHeight, isFull);

        SaveResolution();
    }

    private void SaveResolution()
    {
        SetPlayerPrefs(steamId + " Resolution Width", resolutionWidth);
        SetPlayerPrefs(steamId + " Resolution Height", resolutionHeight);
    }

    private void FindResolutions()
    {
        resolutions = Screen.resolutions;
    }

    private void InitializeResolution()
    {
        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, isFull);
        resolutionWidth = resolutions[resolutions.Length - 1].width;
        resolutionHeight = resolutions[resolutions.Length - 1].height;
        SaveResolution();
    }

    public void SetDisplayResolutionLabel()
    {
        currentResolutionLabel.text = resolutionWidth + " x " + resolutionHeight;
    }

    public TMP_Text qualityText;

    public void ChangeVideoQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        SetPlayerPrefs(steamId + " Video Quality", index);
    }

    public void ChangeQualityText(string text)
    {
        qualityText.text = text.ToUpper();
        SetPlayerPrefs(steamId + " Video Quality Text", text);
    }

    public TMP_Text showFPSText;
    public void SetFPS(bool val)
    {
        CustomizedData.showFPS = val;
        int key = 0;
        if (val)
        {
            key = 1;
        }
        SetPlayerPrefs(steamId + " showFPS", key);
    }

    public void SetFPSText(string text)
    {
        showFPSText.text = text;
        SetPlayerPrefs(steamId + " showFPSText", text);
    }

    public void ResetSettings()
    {
        ChangeNormalSensitivity(1f);

        LimitFramerate(0.5f);
        InitializeResolution();

        ChangeVideoQuality(1);
        ChangeQualityText("MEDIUM");

        SetFPS(false);
        SetFPSText("no");
    }

    public void SetPlayerPrefs(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
    }

    public void SetPlayerPrefs(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }

    public void SetPlayerPrefs(string key, string val)
    {
        PlayerPrefs.SetString(key, val);
    }
}
