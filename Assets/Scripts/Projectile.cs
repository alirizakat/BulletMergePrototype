using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifespan = 3f; // The duration after which the projectile returns to the object pool
    private float elapsedTime = 0f; // The time elapsed since the projectile was spawned

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetRigidbodyComponent(out BaseGate gate)) 
        {
            gate.IncreaseHealthPoints(damage);
            ReturnToPool();
        }

        if(collider.TryGetRigidbodyComponent(out EndgameWall wall)) 
        {
            wall.DecreaseHealthPoints(damage);
        }
        //ReturnToPool();
    }

    private void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Increase the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the projectile's lifespan has exceeded
        if (elapsedTime >= lifespan)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // Deactivate the projectile
        gameObject.SetActive(false);

        // Reset the elapsed time
        elapsedTime = 0f;

        // Return the projectile to the object pool
        PoolerManager.instance.ReturnObjectToPool(gameObject);
    }
}
