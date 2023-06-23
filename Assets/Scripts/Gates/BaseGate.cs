using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum GateType 
{
    FireRate, Size, TripleShot, Range
}
public class BaseGate : MonoBehaviour
{
    public int healthPoints; // Health points of the gate
    public TextMeshProUGUI healthText; // Reference to the TextMeshPro component
    public GateType type;
    protected virtual void Start()
    {
        // Generate a random health value between -30 and +30
        healthPoints = Random.Range(-30, 31);

        // Get the TextMeshPro component from the gate's game object
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthText();
    }

    // Method to increase health points based on the damage amount of the projectile
    public virtual void IncreaseHealthPoints(int damageAmount)
    {
        healthPoints += damageAmount;
        UpdateHealthText();
    }

    // Method to update the health text
    protected virtual void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = type.ToString() + " " + healthPoints.ToString();
        }
    }
}
