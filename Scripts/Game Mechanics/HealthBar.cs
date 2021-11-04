using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public float maxHealth = 100f;
    public Image healthBar;
    public float characterCurrentHealth;
    public PlayerMovement player;

    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    void Update()
    {
        characterCurrentHealth = player.GetHealth();
        healthBar.fillAmount = characterCurrentHealth / maxHealth;
    }
}
