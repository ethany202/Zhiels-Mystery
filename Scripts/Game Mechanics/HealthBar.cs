using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public float maxHealth = 100f;
    private Image healthBar;
    private TMP_Text healthText;
    public float characterCurrentHealth=100f;

    public void InitializeUI()
    {
        healthBar = GameObject.FindWithTag("HealthFill").GetComponent<Image>();
        healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
    }

    public void SetCurrentHealth(float val)
    {
        characterCurrentHealth = val;
        healthBar.fillAmount = characterCurrentHealth / maxHealth;
        healthText.text = characterCurrentHealth+"";
    }

    public float GetCurrentHealth()
    {
        return characterCurrentHealth;
    }

    public void IncreaseHealth(float val)
    {
        if (val + characterCurrentHealth > 100f)
        {
            SetCurrentHealth(100f);
        }
        else
        {
            SetCurrentHealth(characterCurrentHealth + val);
        }
        
    }

    public void DecreaseHealth(float increment)
    {
        if (characterCurrentHealth - increment < 0)
        {
            SetCurrentHealth(0f);
        }
        else
        {
            SetCurrentHealth(characterCurrentHealth - increment);
        }
    }
}
