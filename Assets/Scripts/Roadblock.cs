using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roadblock : MonoBehaviour
{
    public int wallDamage;
    bool doOnce;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetRigidbodyComponent(out Bullet bullet) & !doOnce) 
        {
            doOnce = true;
            PoolerManager.instance.ReturnObjectToPool(gameObject);
            bullet.TakeDamage(wallDamage);
        }
    }
}
