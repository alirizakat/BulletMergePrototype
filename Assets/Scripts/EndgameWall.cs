using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndgameWall : MonoBehaviour
{
    public int healthPoints; // Health points of the gate
    public TextMeshProUGUI healthText; // Reference to the TextMeshPro component
    void Start()
    {
        // Generate a random health value between -30 and +30
        healthPoints = 999;

        // Get the TextMeshPro component from the gate's game object
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthText();
    }

    public void DecreaseHealthPoints(int damageAmount)
    {
        healthPoints -= damageAmount;
        UpdateHealthText();
    }

    // Method to update the health text
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = healthPoints.ToString();
        }
    }
}
