using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ResourceType
{
    Gold
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public event System.Action ResourceSpentEvent;
    [SerializeField] TextMeshProUGUI resourceCountText;
    [SerializeField] int resourceCount;
    [SerializeField] string saveName;
    [SerializeField] int startAmount;

    void Awake()
    {
        instance = this;

        if (!PlayerPrefs.HasKey(saveName)) SetResource(startAmount);
        else 
        {
            resourceCount = PlayerPrefs.GetInt(saveName, resourceCount);
            UpdateText();
        }
    }

    void PerformSave()
    {
        PlayerPrefs.SetInt(saveName, resourceCount);
    }
    void UpdateText()
    {
        resourceCountText.text = resourceCount.ToString();
        PerformSave();
    }
        
    public void ModifyResource(int amount)
    {
        SetResource(resourceCount + amount);
    }

    public bool SpendResource(int amount)
    {
        bool isSpent = CanSpend(amount);

        if(isSpent)
        {
            ModifyResource(-amount);
            ResourceSpentEvent?.Invoke();
        }
        return isSpent;
    }
    bool CanSpend(int amount)
    {
        if(amount > resourceCount) return false;
        else return true;
    }

    public void SetResource(int amount)
    {
        resourceCount = amount;
        UpdateText();
    }
}
