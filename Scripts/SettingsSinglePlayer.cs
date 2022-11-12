using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;
using System.Diagnostics;
using System;

public class SettingsSinglePlayer : MonoBehaviour
{
    private float LastHereTime;

    private string steamId = "";

    public GameObject options; // Settings tab, escape to desktop
    private bool isPresent;

    // Framerate Cap
    public Slider framerateSlider;
    public int maxFramerate = 300;
    public TMP_Text currentFPS;

    public GameObject videoSettings;

    public bool otherFactors;

    void Start()
    {

        if (SteamManager.Initialized)
        {
            steamId = SteamUser.GetSteamID().ToString();
        }

        audioSources = GameObject.FindObjectsOfType<AudioSource>();

        otherFactors = true;
        openSettings = false;
        isPresent = false;

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
                QualityProperties.SetFullscreen(true);
            }
            else
            {
                windowSettings.text = "WINDOWED";
                Screen.fullScreen = false;
                isFull = false;
                QualityProperties.SetFullscreen(false);
            }
        }
        else
        {
            //isFull = true;
            //Screen.fullScreen = isFull;
            windowSettings.text = "FULLSCREEN";
            QualityProperties.SetFullscreen(true);
        }
        if (PlayerPrefs.HasKey(steamId + " Resolution Width") && PlayerPrefs.HasKey(steamId + " Resolution Height"))
        {
            resolutionWidth = PlayerPrefs.GetInt(steamId + " Resolution Width");
            resolutionHeight = PlayerPrefs.GetInt(steamId + " Resolution Height");
            Screen.SetResolution(resolutionWidth, resolutionHeight, isFull);
            QualityProperties.SetResolution(resolutionWidth, resolutionHeight);
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
        if (PlayerPrefs.HasKey(steamId + " showFPS"))
        {
            if (PlayerPrefs.GetInt(steamId + " showFPS") == 0)
            {
                SetFPS(false);
                SetFPSText("no");
            }
            else
            {
                SetFPS(true);
                SetFPSText("yes");
            }
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

        #region Volume
        if (PlayerPrefs.HasKey(steamId + " Master Volume"))
        {
            AudioSettings.SetMasterVolume(PlayerPrefs.GetInt(steamId + " Master Volume"));
            float currentMasterVol = PlayerPrefs.GetFloat(steamId + " Master Volume");
            SetMasterVolume(currentMasterVol / 100);
        }
        else
        {
            AudioSettings.SetMasterVolume(100);
            SetMasterVolume(1f);
        }

        if (PlayerPrefs.HasKey(steamId + " Music Volume"))
        {
            AudioSettings.SetMusicVolume(PlayerPrefs.GetInt(steamId + " Music Volume"));
            float musicVol = PlayerPrefs.GetFloat(steamId + " Music Volume");
            SetMusicVolume(musicVol / 100);
        }
        else
        {
            AudioSettings.SetMusicVolume(50);
            SetMusicVolume(0.5f);
        }
        if (PlayerPrefs.HasKey(steamId + " SFX Volume"))
        {
            AudioSettings.SetSFXVolume(PlayerPrefs.GetInt(steamId + "SFX Volume"));
            float sfxVol = PlayerPrefs.GetFloat(steamId + " SFX Volume");
            SetSFXVolume(sfxVol / 100);
        }
        else
        {
            AudioSettings.SetSFXVolume(100);
            SetSFXVolume(1f);
        }
        #endregion

        #region VSync
        if (PlayerPrefs.HasKey(steamId + " VSync"))
        {
            ChangeVSync(PlayerPrefs.GetInt(steamId + " VSync"));
            ChangeVSyncText(PlayerPrefs.GetString(steamId + " VSync Text"));
        }
        else
        {
            ChangeVSync(0);
            ChangeVSyncText("off");
        }
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

        #region Idle Time
        if (PlayerPrefs.HasKey(steamId + " Idle Time"))
        {
            SetIdleTime(PlayerPrefs.GetInt(steamId + " Idle Time"));
        }
        else
        {
            SetIdleTime(5);
        }
        #endregion
    }


    void Update()
    {
        CheckIdle();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options.GetComponent<Animator>().SetTrigger("showOptions");

            if (openSettings)
            {
                ExitToMainMenu();
                //SetOpenSettings(false);
            }
        }
    }

    void OnGUI()
    {
        SetDisplayResolutionLabel();
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public GameObject checkPopup;
    public void CancelQuit()
    {
        checkPopup.SetActive(false);
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

    /// <summary>
    /// Changes the display value of the framerate slider and then applies a target framerate for the user
    /// </summary>
    /// <param name="value"></param>
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

        QualityProperties.SetFPSCap(fpsRounded);
    }

    public GameObject settingsMenu;
    public GameObject background;
    public GameObject menus;
    private bool openSettings;

    /// <summary>
    /// Method called when user exits settings menu to the main menu
    /// </summary>
    public void ExitToMainMenu()
    {
        settingsMenu.SetActive(false);
        menus.SetActive(true);
        //background.SetActive(true);
        openSettings = false;
        isPresent = false;
        options.SetActive(true);
    }

    /// <summary>
    /// Deactivates specified GameObjects when settings menu is opened
    /// </summary>
    /// <param name="val"></param>
    public void SetOpenSettings(bool val)
    {
        openSettings = val;
    }

    public TMP_Text windowSettings;
    private bool isFull;

    /// <summary>
    /// Changes the type of window setting(fullscreen or windowed)
    /// </summary>
    public void WindowSettings()
    {
        isFull = !isFull;
        Screen.fullScreen = isFull;

        QualityProperties.SetFullscreen(isFull);

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
        QualityProperties.SetResolution(resolutionWidth, resolutionHeight);

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

    /// <summary>
    /// Changes the video quality of the game
    /// </summary>
    public TMP_Text qualityText;

    public void ChangeVideoQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        SetPlayerPrefs(steamId + " Video Quality", index);
        QualityProperties.SetQualityIndex(index);
    }

    public void ChangeQualityText(string text)
    {
        qualityText.text = text.ToUpper();
        SetPlayerPrefs(steamId + " Video Quality Text", text);
    }

    /// <summary>
    /// Alters the master volume
    /// </summary>
    public Slider masterVolume;
    public TMP_Text masterVolText;

    public void SetMasterVolume(float vol)
    {
        masterVolume.value = vol;
        SetMasterVolume();
    }

    public void SetMasterVolume()
    {
        float val = masterVolume.value * 100;
        int roundedVal = (int)val;
        masterVolText.text = "" + roundedVal;

        SetPlayerPrefs(steamId + " Master Volume", val);

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].tag == "Music")
            {
                if (roundedVal < AudioSettings.GetMusicVolume())
                {
                    audioSources[i].volume = val / 100f;
                }
            }
            else if (audioSources[i].tag == "SFX")
            {
                if (roundedVal < AudioSettings.GetSFXVolume())
                {
                    audioSources[i].volume = val / 100f;
                }
            }
            else
            {
                audioSources[i].volume = val / 100f;
            }
        }
        AudioSettings.SetMasterVolume(roundedVal);
    }

    /// <summary>
    /// Alters the volume of only music
    /// </summary>
    public Slider musicVolume;
    public TMP_Text musicVolText;
    private AudioSource[] audioSources;

    public void SetMusicVolume(float vol)
    {
        musicVolume.value = vol;
        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        float val = musicVolume.value * 100;
        musicVolText.text = "" + (int)val;


        SetPlayerPrefs(steamId + " Music Volume", val);

        float actualVolume = (int)val > AudioSettings.GetMasterVolume() ? AudioSettings.GetMasterVolume() : val;
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].tag == "Music")
            {
                audioSources[i].volume = actualVolume / 100f;
            }
        }
        AudioSettings.SetMusicVolume((int)actualVolume);
    }

    /// <summary>
    /// Changes the SFX volume
    /// </summary>
    public Slider sfxVolume;
    public TMP_Text sfxVolText;

    public void SetSFXVolume(float vol)
    {
        sfxVolume.value = vol;
        SetSFXVolume();
    }

    public void SetSFXVolume()
    {
        float val = sfxVolume.value * 100;
        int roundedVal = (int)val;
        sfxVolText.text = "" + roundedVal;

        SetPlayerPrefs(steamId + " SFX Volume", val);

        float actualVolume = roundedVal > AudioSettings.GetMasterVolume() ? AudioSettings.GetMasterVolume() : roundedVal;
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].tag == "SFX")
            {
                audioSources[i].volume = actualVolume / 100f;
            }
        }
        AudioSettings.SetSFXVolume((int)actualVolume);

    }

    /// <summary>
    /// Either adds or removes VSync
    /// </summary>
    public TMP_Text vsyncText;
    public void ChangeVSync(int val)
    {
        QualitySettings.vSyncCount = val;
        QualityProperties.SetVSyncIndex(val);
        SetPlayerPrefs(steamId + " VSync", val);
    }

    public void ChangeVSyncText(string text)
    {
        vsyncText.text = text;
        SetPlayerPrefs(steamId + " VSync Text", text);
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




    public void OpenWebsite()
    {
        string site = "https://zhiels-mystery-test.herokuapp.com";
        Process.Start(site);
    }

    public TMP_Text idleTimeText;
    private int idleTimeMinutes;
    public void SetIdleTime(int time)
    {
        idleTimeMinutes = time;
        idleTimeText.text = $"({idleTimeMinutes} min.)";
        SetPlayerPrefs(steamId + " Idle Time", idleTimeMinutes);
    }

    public void ResetSettings()
    {
        ChangeNormalSensitivity(1f);

        LimitFramerate(0.5f);
        InitializeResolution();

        ChangeVideoQuality(1);
        ChangeQualityText("MEDIUM");

        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);

        ChangeVSync(0);
        ChangeVSyncText("off");

        SetFPS(false);
        SetFPSText("no");

        SetIdleTime(5);

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

    private void CheckIdle()
    {
        if (Input.anyKey)
        {
            LastHereTime = UnityEngine.Time.time;
            SetAFKState(false);
            afkMask.SetActive(false);
        }
        if (UnityEngine.Time.time - LastHereTime >= (idleTimeMinutes * 60f))
        {
            SetAFKState(true);
            afkMask.SetActive(true);
        }
    }

    public GameObject afkMask;
    private void SetAFKState(bool val)
    {
        AudioListener.pause = val;
        if (val)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
