using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    public List<GameObject> bulletPrefabs = new List<GameObject>();

    public List<Transform> aliveBullets = new List<Transform>();

    public event System.Action BulletMergeEvent;
    public event System.Action BulletBuyEvent;
    public event System.Action BulletsFinishedEvent;
    void Awake()
    {
        instance = this;
    }
    public void MergeBullets(Cell cell, int mergeLevel)
    {
        switch (mergeLevel)
        {
            case 1:
                GameObject level2Bullet = Instantiate(bulletPrefabs[mergeLevel], cell.GetCenter(), Quaternion.identity);
                level2Bullet.GetComponent<Pickible>().GoToCell(cell);
                BulletMergeEvent?.Invoke();
                break;
            
            case 2:
                GameObject level3Bullet = Instantiate(bulletPrefabs[mergeLevel], cell.GetCenter(), Quaternion.identity);
                level3Bullet.GetComponent<Pickible>().GoToCell(cell);
                BulletMergeEvent?.Invoke();
                break;
        }
    }
    public void CreateBullet(Cell cell)
    {
        GameObject createdBullet = Instantiate(bulletPrefabs[0], cell.GetCenter(), Quaternion.identity);
        createdBullet.GetComponent<Pickible>().GoToCell(cell);
        BulletBuyEvent?.Invoke();
    }
    public void CreateBullet(Cell cell, int level)
    {
        GameObject createdBullet = Instantiate(bulletPrefabs[level], cell.GetCenter(), Quaternion.identity);
        createdBullet.GetComponent<Pickible>().GoToCell(cell);
    }
    public void AddBulletToList(Transform transform) 
    {
        aliveBullets.Add(transform);
    }
    public void RemoveBulletFromList(Transform transform) 
    {
        aliveBullets.Remove(transform);
        if(aliveBullets.Count < 1) 
        {
            BulletsFinishedEvent?.Invoke();
        }
    }


}