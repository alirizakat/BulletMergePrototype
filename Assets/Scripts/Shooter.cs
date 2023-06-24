using UnityEngine;
using System.Collections.Generic;

public class Shooter : MonoBehaviour
{
    public List<GameObject> projectilePrefab = new List<GameObject>(); // The prefab of the projectile to be spawned
    public Transform projectileSpawnPoint; // The spawn point of the projectile
    public float baseFireRate = 1f; // The base rate at which projectiles are fired (in seconds)
    public float range = 10f; // The maximum range of the projectiles
    public float fireRateMultiplier = 1f; // Multiplier to adjust the fire rate
    public bool canShoot; // Check if can shoot
    public bool isTripleShotEnabled; // Flag to enable triple shot
    public bool isProjectileSizeUpEnabled; // Flag to enable projectile size up
    public int bulletLevel; // Level of the projectile
    public float sizeModifier = 0.5f; // Multiplier to adjust the projectile size
    public Vector3 projectileScale;

    [Header("Debug")]
    [SerializeField] private float fireRate; // The adjusted fire rate
    [SerializeField] private float fireTimer; // Timer to track the fire rate
    //[SerializeField] private List<Vector3> initialScales = new List<Vector3>(); // Store the initial scales of the projectile prefabs

    private void Start()
    {
        fireRate = baseFireRate / fireRateMultiplier;
    }

    private void Update()
    {
        if (!canShoot) return;
        // Update the fire timer
        fireTimer += Time.deltaTime;

        // Check if the fire button is pressed and the fire rate condition is met
        if (InputManager.instance.touchState == InputManager.TouchState.Continue && fireTimer >= fireRate)
        {
            if (isTripleShotEnabled)
            {
                TripleShoot();
            }
            else
            {
                FireProjectile();
            }

            fireTimer = 0f; // Reset the fire timer
        }
    }

    private void FireProjectile()
    {
        // Spawn a projectile from the object pool
        GameObject projectile = PoolerManager.instance.GetObjectFromPool(projectilePrefab[bulletLevel]);
        if (projectile == null)
        {
            Debug.LogWarning("Projectile pool is not available.");
            return;
        }

        // Set the projectile's position and rotation to match the spawn point
        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.rotation = projectileSpawnPoint.rotation;

        // Apply additional modifications to the projectile based on the specifications
        // For example, you can adjust the projectile's speed, size, etc.

        // Set the projectile's forward direction to match the spawn point's forward direction
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.speed = CalculateProjectileSpeed();
        }
        projectile.transform.localScale = projectileScale;
        // Activate the projectile
        projectile.SetActive(true);
    }

    private void TripleShoot()
    {
        // Spawn three projectiles with adjusted angles
        for (int i = -1; i <= 1; i++)
        {
            // Spawn a projectile from the object pool
            GameObject projectile = PoolerManager.instance.GetObjectFromPool(projectilePrefab[bulletLevel]);
            if (projectile == null)
            {
                Debug.LogWarning("Projectile pool is not available.");
                return;
            }

            // Set the projectile's position and rotation to match the spawn point
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.transform.rotation = projectileSpawnPoint.rotation;

            // Apply additional modifications to the projectile based on the specifications
            // For example, you can adjust the projectile's speed, size, etc.

            // Set the projectile's forward direction to match the spawn point's forward direction
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.speed = CalculateProjectileSpeed();
            }

            // Apply rotation to the additional projectiles
            float angle = 10f * i;
            projectile.transform.Rotate(Vector3.up, angle);
            projectile.transform.localScale = projectileScale;
            // Activate the projectile
            projectile.SetActive(true);
        }
    }

    public void AdjustFireRate(float adjustmentAmount)
    {
        fireRateMultiplier -= adjustmentAmount;
        fireRateMultiplier = Mathf.Min(fireRateMultiplier, 0.25f);

        fireRate = baseFireRate * fireRateMultiplier;
    }
    public void AdjustRange(float adjustmentAmount) 
    {
        range += adjustmentAmount;
    }
    public void EnableTripleShot()
    {
        isTripleShotEnabled = true;
    }

    public void DisableTripleShot()
    {
        isTripleShotEnabled = false;
    }

    public void AdjustSize(float sizeMultiplier)
    {
        projectileScale += new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier);
    }

    private float CalculateProjectileSpeed()
    {
        // Calculate the projectile's speed based on the range
        // Adjust this calculation based on your requirements
        return range / fireRate;
    }

    public void BulletLevel(int bulletLevel)
    {
        this.bulletLevel = bulletLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetRigidbodyComponent(out FireRateGate gate)) 
        {
            Debug.Log("Works?");
            float fireRateAdjustment = 0.2f;
            if (gate.healthPoints >= 0 && gate.healthPoints <= 25)
            {
                AdjustFireRate(fireRateAdjustment);
            }
            else if (gate.healthPoints > 25 && gate.healthPoints <= 50)
            {
                AdjustFireRate(fireRateAdjustment * 2f);
            }
            else if (gate.healthPoints >= -50 && gate.healthPoints < 0)
            {
                AdjustFireRate(-fireRateAdjustment * 2f);
            }

        }
        if(other.TryGetRigidbodyComponent(out Wall wall)) 
        {
            if (GetComponentInParent<PlayerPistol>().invincible) return;
            GameManager.instance.LevelFailed();
            PlayerPistol playerObj = GetComponentInParent<PlayerPistol>();
            Destroy(playerObj.gameObject);
        }
        if(other.TryGetRigidbodyComponent(out EndgameWall endgame))
        {
            GameManager.instance.LevelFinished();
            PlayerPistol playerObj = GetComponentInParent<PlayerPistol>();
            Destroy(playerObj.gameObject);
        }

        if(other.TryGetRigidbodyComponent(out Gold gold)) 
        {
            ResourceManager.instance.ModifyResource(gold.increaseAmount);
            Destroy(gold.gameObject);
        }

        if(other.TryGetRigidbodyComponent(out RangeGate range)) 
        {
            float rangeAdjustment = 50f;
            if (range.healthPoints >= 0 && range.healthPoints <= 25)
            {
                AdjustRange(rangeAdjustment);
            }
            else if (range.healthPoints > 25 && range.healthPoints <= 50)
            {
                AdjustRange(rangeAdjustment * 2f);
            }
            else if (range.healthPoints >= -50 && range.healthPoints < 0)
            {
                AdjustRange(-rangeAdjustment/4);
            }
        }

        if (other.TryGetRigidbodyComponent(out TripleShotGate triple)) 
        {
            if(triple.healthPoints > 0) 
            {
                EnableTripleShot();
            }

            if(triple.healthPoints < 0 && isTripleShotEnabled) 
            {
                DisableTripleShot();
            }
        }

        if(other.TryGetRigidbodyComponent(out SizeUpGate size)) 
        {
            if (size.healthPoints >= 0 && size.healthPoints <= 25)
            {
                AdjustSize(sizeModifier);
            }
            else if (size.healthPoints > 25 && size.healthPoints <= 50)
            {
                AdjustSize(sizeModifier * 2f);
            }
            else if (size.healthPoints >= -50 && size.healthPoints < 0)
            {
                AdjustSize(-sizeModifier);
            }
        }
    }
}
