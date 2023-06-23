using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RoadblockManager : MonoBehaviour
{
    public static RoadblockManager instance;

    public event System.Action PlayerObjectsCreatedEvent;
    [Header("Config")]
    [SerializeField] int roadblockWidth;
    [SerializeField] int roadblockLength;
    [SerializeField] int roadblockGap;
    [SerializeField] int roadblockDistance;
    [SerializeField] int playerObjDistance;
    [SerializeField] int playerObjGap;

    public List<GameObject> roadBlocks = new List<GameObject>();
    public List<GameObject> playerObjects = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }
    public void SubscribeToPoolCreation()
    {
        PoolerManager.instance.PoolCreatedEvent += OnPoolCreated;
    }

    void OnPoolCreated()
    {
        PoolerManager pooler = PoolerManager.instance;
        int totalCount = roadblockWidth * roadblockLength;
        int firstCount = Mathf.Min(roadblockWidth * 5, totalCount);
        int secondCount = (int)(totalCount * 0.3f);
        int thirdCount = totalCount - firstCount - secondCount;

        List<GameObject> roadblockList = new List<GameObject>();

        // Add roadblocks for the first 5 lines (level 1)
        for (int i = 0; i < firstCount; i++)
        {
            roadblockList.Add(pooler.prefabsList[0]);
        }

        // Add remaining roadblocks according to the desired distribution percentages
        for (int i = 0; i < secondCount; i++)
        {
            roadblockList.Add(pooler.prefabsList[1]);
        }

        for (int i = 0; i < thirdCount; i++)
        {
            roadblockList.Add(pooler.prefabsList[2]);
        }

        // Shuffle the roadblock list
        ShuffleList(roadblockList);

        int index = 0;

        for (int j = 0; j < roadblockLength; j++)
        {
            for (int i = 0; i < roadblockWidth; i++)
            {
                Vector3 worldPos = new Vector3(i * roadblockGap, 5, (j * roadblockGap) + roadblockDistance);
                GameObject obj = pooler.GetObjectFromPool(roadblockList[index]);
                obj.transform.position = worldPos;
                roadBlocks.Add(obj);
                index++;
                if (index >= roadblockList.Count)
                {
                    // Handle the case when the roadblockList is exhausted
                    // You can add fallback behavior or display an error message

                    //create player objects
                    int width = roadblockWidth; // Specify the width of the player objects
                    Vector3 playerObjPos = Vector3.zero; // Starting position for player objects
                    for (int b = 0; b < width; b++)
                    {
                        playerObjPos.z = roadblockDistance + (roadblockLength * roadblockGap) + playerObjDistance;
                        playerObjPos.x = b * roadblockGap;

                        GameObject playerObj = pooler.GetObjectFromPool(pooler.playerObjectPrefab);
                        playerObj.transform.position = playerObjPos;
                        playerObj.SetActive(true);
                        playerObjects.Add(playerObj);
                    }
                    PlayerObjectsCreatedEvent?.Invoke();
                    return;
                }
            }
        }

        
    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
