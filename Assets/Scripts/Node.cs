using UnityEngine;

public class Node
{
    public Vector3 cellPos;
    public Transform obj;
    public Cell cell;
    public int occupierLevel;
    public Node(Vector3 cellPos, Transform obj, Cell cell, int occupierLevel)
    {
        this.cellPos = cellPos;
        this.obj = obj;
        this.cell = cell;
        this.occupierLevel = occupierLevel;
    }
}