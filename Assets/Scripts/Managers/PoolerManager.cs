using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerManager : MonoBehaviour
{
    public static PoolerManager instance;

    public event System.Action PoolCreatedEvent;
    public List<GameObject> prefabsList;
    public GameObject playerObjectPrefab;
    public GameObject projectilePrefabLevel1;
    public GameObject projectilePrefabLevel2;
    public GameObject projectilePrefabLevel3;
    public int poolSize;
    public Transform pistolParent;

    public GameObject goldPrefab;
    public int goldAmount = 100;
    public Vector3 spawnBoundsMin;
    public Vector3 spawnBoundsMax;

    public List<GameObject> gates = new List<GameObject>();
    void Awake()
    {
        instance = this;
    }

    private Dictionary<GameObject, List<GameObject>> objectPools;
    private Dictionary<GameObject, GameObject> prefabToPoolMapping;

    private void Start()
    {
        objectPools = new Dictionary<GameObject, List<GameObject>>();
        prefabToPoolMapping = new Dictionary<GameObject, GameObject>();

        foreach (GameObject prefab in prefabsList)
        {
            CreateObjectPool(prefab);
        }
        foreach (GameObject item in gates)
        {
            CreateObjectPool(item);
        }
        CreateObjectPool(playerObjectPrefab);
        CreateObjectPool(projectilePrefabLevel1);
        CreateObjectPool(projectilePrefabLevel2);
        CreateObjectPool(projectilePrefabLevel3);
        CreateObjectPool(goldPrefab, goldAmount);

        RoadblockManager.instance.SubscribeToPoolCreation();
        PoolCreatedEvent?.Invoke();
        SpawnGoldObjects();
        SpawnGates();
    }

    private void CreateObjectPool(GameObject prefab)
    {
        List<GameObject> pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            if (prefab == playerObjectPrefab)
                obj.transform.parent = pistolParent;
            obj.SetActive(false);
            pool.Add(obj);
        }

        objectPools.Add(prefab, pool);
        prefabToPoolMapping.Add(prefab, prefab);
    }
    private void CreateObjectPool(GameObject prefab, int poolSize)
    {
        List<GameObject> pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        objectPools.Add(prefab, pool);
        prefabToPoolMapping.Add(prefab, prefab);
    }
    public GameObject GetObjectFromPool(GameObject prefab)
    {
        if (objectPools.ContainsKey(prefab))
        {
            List<GameObject> pool = objectPools[prefab];

            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            GameObject newObj = Instantiate(prefab);
            pool.Add(newObj);
            return newObj;
        }

        Debug.LogWarning("Prefab is not registered in the object pool.");
        return null;
    }
    public GameObject[] GetObjectsFromPool(GameObject prefab, int amount) 
    {
        GameObject[] objects = new GameObject[amount];
        for(int i = 0; i < amount; i++) 
        {
            objects[i] = GetObjectFromPool(prefab);
        }
        return objects;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void SpawnGoldObjects()
    {
        for (int i = 0; i < goldAmount; i++)
        {
            GameObject goldObject = GetObjectFromPool(goldPrefab);
            if (goldObject != null)
            {
                // Randomly generate a position within the spawn bounds
                Vector3 randomPosition = new Vector3(
                    Random.Range(spawnBoundsMin.x, spawnBoundsMax.x),
                    Random.Range(spawnBoundsMin.y, spawnBoundsMax.y),
                    Random.Range(spawnBoundsMin.z, spawnBoundsMax.z)
                );
                goldObject.transform.position = randomPosition;
                goldObject.SetActive(true);
            }
        }
    }
    public void SpawnGates() 
    {
        for (int i = 0; i < gates.Count; i++)
        {
            GameObject gate = GetObjectFromPool(gates[i]);
            if (gate != null)
            {
                // Randomly generate a position within the spawn bounds
                Vector3 randomPosition = new Vector3(
                    Random.Range(spawnBoundsMin.x, spawnBoundsMax.x),
                    Random.Range(spawnBoundsMin.y + 15, spawnBoundsMax.y + 15),
                    Random.Range(spawnBoundsMin.z, spawnBoundsMax.z)
                );
                gate.transform.position = randomPosition;
                gate.SetActive(true);
            }
        }
    }
}
