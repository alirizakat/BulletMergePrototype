using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Pickible occupier;
    public int occupierLevel;
    public Vector3 GetCenter()
    {
        return transform.position;
    }

    public void SetOccupied(Pickible occupier, int occupierLevel)
    {
        this.occupier = occupier;
        this.occupierLevel = occupierLevel;
    }

    public bool IsOccupied()
    {
        return occupier != null;
    }
}
