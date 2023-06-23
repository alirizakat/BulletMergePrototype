using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool canMove = false;

    public int bulletLevel;
    public int bulletHealth;

    public float bulletSpeed;

    public Pickible pickible;

    void Start()
    {
        pickible = GetComponent<Pickible>();

        GameManager.instance.BulletFiredEvent += OnBulletFire;
    }
    private void Update()
    {
        if(!canMove) 
        {
            return;
        }
        else 
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }

    private void OnBulletFire()
    {
        pickible.enabled = false;
        BulletManager.instance.AddBulletToList(transform);
        canMove = true;
    }

    public void Merge(Cell cell)
    {
        pickible.LeaveCell();
        BulletManager.instance.MergeBullets(cell, bulletLevel);
        UnsubscribeFromEvent();
        Destroy(gameObject);
    }
    public void UnsubscribeFromEvent() 
    {
        GameManager.instance.BulletFiredEvent -= OnBulletFire;
    }
    public void TakeDamage(int damage) 
    {
        bulletHealth -= damage;
        if(bulletHealth < 1) 
        {
            BulletManager.instance.RemoveBulletFromList(transform);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetRigidbodyComponent(out Shooter shooter)) 
        {
            PlayerPistol.instance.isWorking = true;
            shooter.canShoot = true;
            shooter.BulletLevel(bulletLevel - 1);
            BulletManager.instance.RemoveBulletFromList(transform);
            Destroy(gameObject);
        }
    }
}

