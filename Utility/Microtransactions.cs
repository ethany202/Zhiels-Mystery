using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Microtransactions : MonoBehaviour
{

    private string steamUsername;
    private string steamID;
    private int quetzalAmount;

    public void OpenPaymentSite(int amount)
    {
        string website = "https://quetzal-payment.herokuapp.com/username="+steamUsername+"&quetzalAmount="+amount+"&steamID="+steamID;
        Process.Start(website);
    }

    public void SetSteamID(string newID)
    {
        steamID=newID;
    }

    public void SetSteamUsername(string newName)
    {
        steamUsername = newName;
    }

}
