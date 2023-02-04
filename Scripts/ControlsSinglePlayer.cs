using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;

public class ControlsSinglePlayer : MonoBehaviour
{

    private Dictionary<string, KeyCode> controls;
    public TMP_Text[] controlsText;

    private string steamId;
    private TMP_Text txt;

    public GameObject warningPanel;
    public GameObject disableBack;

    public GameObject controlsPanelObj;
    private GameObject currentKey = null;
    public bool controlsPanel;


    // public SettingsSinglePlayer sC;


    public void SetPanelActive(bool val)
    {
        controlsPanel = val;
    }

    void Start()
    {
        controlsPanel = false;

        SetDefaultKeys();
        SetKeyConstants();

        if (SteamManager.Initialized)
        {
            LoadControls();
        }
    }

    void Update()
    {
        if (!controlsPanelObj.activeInHierarchy)
        {
            controlsPanel = false;
        }
        else
        {
            if (SteamManager.Initialized)
            {
                steamId = SteamUser.GetSteamID().ToString();
                controlsPanel = true;
            }
        }
    }

    private void SetKeyConstants()
    {
        ControlsConstants.keys = controls;
    }

    public void SetDefaultKeys()
    {
        controls = new Dictionary<string, KeyCode>();

        //mutable keybinds:
        controls.Add("sprint", KeyCode.LeftShift);
        controls.Add("crouch", KeyCode.LeftControl);
        controls.Add("jump", KeyCode.Space);
        controls.Add("open", KeyCode.E);
        controls.Add("grab", KeyCode.G);
        controls.Add("drop", KeyCode.Z);
        controls.Add("slide", KeyCode.F);

        // immutable keys:
        controls.Add("forward", KeyCode.W);
        controls.Add("backward", KeyCode.S);
        controls.Add("right", KeyCode.D);
        controls.Add("left", KeyCode.A);
        controls.Add("attack", KeyCode.Mouse0);
    }

    void OnGUI()
    {

        if (controlsPanel)
        {
            if (currentKey != null)
            {
                Event e = Event.current;
                if (e.isKey)
                {
                    controls[currentKey.name] = e.keyCode;
                    currentKey.GetComponentInChildren<TMP_Text>().text = e.keyCode.ToString();
                    PlayerPrefs.SetString(steamId + currentKey.name, e.keyCode.ToString());
                    currentKey = null;
                    return;
                }
            }
            //if (HasDuplicates())
            //{
            //    disableBack.SetActive(true);
            //    warningPanel.SetActive(true);
            //    // sC.otherFactors = false;
            //}
            //else
            //{
            //    disableBack.SetActive(false);
            //    warningPanel.SetActive(false);
            //    // sC.otherFactors = true;
            //}
        }
    }

    public bool HasDuplicates()
    {
        for (int i = 0; i < controls.Count; i++)
        {
            for (int j = 0; j < controls.Count; j++)
            {
                if (i != j)
                {
                    if (controls[controls.ElementAt(i).Key] == controls[controls.ElementAt(j).Key])
                        return true;
                }
            }
        }
        return false;
    }

    public void LoadControls()
    {
        for (int i = 0; i < controls.Count; i++)
        {
            if (PlayerPrefs.HasKey(steamId + controls.ElementAt(i).Key))
            {
                controls[controls.ElementAt(i).Key] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(steamId + controls.ElementAt(i).Key));
                controlsText[i].text = PlayerPrefs.GetString(steamId + controls.ElementAt(i).Key);
            }
        }
        SaveControls();
    }

    public void GetKeyButton(GameObject btn)
    {
        currentKey = btn;
    }

    public void SaveControls()
    {
        foreach (KeyValuePair<string, KeyCode> pair in controls)
        {
            PlayerPrefs.SetString(steamId + pair.Key, pair.Value.ToString());
        }
    }
}
