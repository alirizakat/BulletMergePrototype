using UnityEngine;
using System.Collections.Generic;
public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [Header("Config")]
    public List<Transform> allNodes = new List<Transform>();
    [SerializeField] int gridHeight;
    [SerializeField] int gridWidth;
    [SerializeField] int gridGap;
    [SerializeField] Transform cellPrefab;

    private Node[,] nodes;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CreateGrid();

        //not the best practice but a fast solution
        ResourceManager.instance.ResourceSpentEvent += SaveGridData;
        BulletManager.instance.BulletMergeEvent += SaveGridData;
        BulletManager.instance.BulletBuyEvent += SaveGridData;
        PickManager.instance.PickibleDroppedEvent += SaveGridData;
    }
    private void CreateGrid()
    {
        nodes = new Node[gridWidth, gridHeight];
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                Vector3 worldPos = new Vector3(i * gridGap,0,j * gridGap);
                Transform obj = Instantiate(cellPrefab, worldPos, Quaternion.identity);
                Cell cell = obj.GetComponent<Cell>();
                nodes[i,j] = new Node(worldPos, obj, cell, 0);
                allNodes.Add(obj);
            }
        }
        LoadGrids();
    }
    private void LoadGrids()
    {
        for(int i = 0; i < allNodes.Count; i++)
        {

            switch(PlayerPrefs.GetInt(i.ToString()))
            {
                case 1:
                    BulletManager.instance.CreateBullet(allNodes[i].GetComponent<Cell>(), 0);
                    break;
                case 2:
                    BulletManager.instance.CreateBullet(allNodes[i].GetComponent<Cell>(), 1);
                    break;
                case 3:
                    BulletManager.instance.CreateBullet(allNodes[i].GetComponent<Cell>(), 2);
                    break;
            }
        }
    }
    private void SaveGridData()
    {
        for(int i = 0; i < allNodes.Count; i++)
        {
            Cell cell = allNodes[i].GetComponent<Cell>();

            if(cell.occupierLevel > 0)
            {
                PlayerPrefs.SetInt(i.ToString(),cell.occupierLevel);
            }

            if(cell.occupierLevel == 0 & PlayerPrefs.HasKey(i.ToString()))
            {
                PlayerPrefs.DeleteKey(i.ToString());
            }
        }
    }
    
    public Cell GetAvailableCell()
    {
        List<Node> availableNodes = new List<Node>();
        foreach (Node item in nodes)
        {
            if(!item.cell.IsOccupied())
            {
                availableNodes.Add(item);
            }
        }
        if(availableNodes.Count > 0) return availableNodes[0].cell;
        else return null; 
    }
}
