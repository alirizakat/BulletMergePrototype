using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickManager : MonoBehaviour
{
    public static PickManager instance;
    public event System.Action PickibleDroppedEvent;
    InputManager inputManager;
    public LayerMask PickibleLayer;
    bool blockPicking;
    
    void Awake()
    {
        instance = this;   
    }
    private void Start()
    {
        inputManager = InputManager.instance;
        
        inputManager.TouchContinueEvent += OnTouchContinue;
    }

    private void OnTouchContinue(InputManager.TouchData data)
    {
        if (blockPicking) return;

        Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.fingerScreenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 300, PickibleLayer))
        {
            if (hit.collider.TryGetRigidbodyComponent(out Pickible pickible))
            {
                pickible.GetPicked();
            }
        }
    }

    public void BlockPicking(bool state)
    {
        blockPicking = state;
    }
    public void PickibleDropped() 
    {
        PickibleDroppedEvent?.Invoke();
    }
}
