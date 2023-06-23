using UnityEngine;

public class BuyManager : MonoBehaviour
{
    public static BuyManager instance;
    GridManager gridManager;
    UIManager uIManager;

    [Header("Config")]
    public Transform level1Bullet;
    public int bulletPrice;
    void Awake()
    {
        instance = this;   
    }
    void Start()
    {
        gridManager = GridManager.instance;
        uIManager = UIManager.instance;

        uIManager.BuyButtonClickEvent += BuyBullet;   
    }

    public void BuyBullet()
    {
        Cell availableCell = gridManager.GetAvailableCell();
        if(ResourceManager.instance.SpendResource(bulletPrice) & availableCell != null)
        {
            BulletManager.instance.CreateBullet(availableCell);
        }
    }
}
