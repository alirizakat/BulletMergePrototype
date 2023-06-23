using System;
using UnityEngine;

public class Pickible : MonoBehaviour
{
    [Header("Config")]
    public LayerMask cellLayer;
    public float moveSpeed;
    public float virtualPosOffset;
    public int bulletLevel;

    [Header("Debug")]
    [SerializeField] bool isPicked;
    [SerializeField] Vector3 virtualPos;
    [SerializeField] Vector3 lastLegalPos;
    public Cell occupiedCell;
    public Bullet bullet;

    private void Start()
    {
        lastLegalPos = transform.position;
        bullet = GetComponent<Bullet>();
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        Vector3 pos;
        if (isPicked)
        {
            Cell cell = GetCellBelow();
            if (cell != null)
            {
                pos = cell.GetCenter();

                pos.y +=5;
                
            }
            else
            {
                pos = virtualPos + Vector3.up * 1;
                pos.y += 1.5f;
            }
        }

        else
        {
            pos = lastLegalPos;
            pos.y = 1.5f;
        }

        transform.position = Utilities.Decay(transform.position, pos, 12, Time.deltaTime);
    }

    public void GetPicked()
    {
        PickManager.instance.BlockPicking(true);

        isPicked = true;

        virtualPos = transform.position;

        InputManager.instance.TouchContinueEvent += OnTouchContinue;
        InputManager.instance.TouchEndEvent += OnTouchEnd;
    }

    private void OnTouchEnd(InputManager.TouchData data)
    {
        InputManager.instance.TouchContinueEvent -= OnTouchContinue;
        InputManager.instance.TouchEndEvent -= OnTouchEnd;
        PickManager.instance.BlockPicking(false);

        isPicked = false;

        Cell cell = GetCellBelow();

        if (cell != null)
        {
            GoToCell(cell);
        }

    }

    private void OnTouchContinue(InputManager.TouchData data)
    {
        Vector3 input = CameraManager.instance.ConvertToCameraRelativeInput(data.frameDelta);
        virtualPos += input * moveSpeed;
    }
    public void GoToCell(Cell targetCell)
    {
        LeaveCell();

        if (targetCell.IsOccupied())
        {
            targetCell.occupier.bullet.Merge(targetCell);
            bullet.UnsubscribeFromEvent();
            Destroy(gameObject);
        }

        else
        {
            lastLegalPos = targetCell.GetCenter();
            targetCell.SetOccupied(this, bulletLevel);
            occupiedCell = targetCell;
        }

        PickManager.instance.PickibleDropped();
    }

    public void LeaveCell()
    {
        if (occupiedCell != null) occupiedCell.SetOccupied(null, 0);
        occupiedCell = null;
    }

    Cell GetCellBelow()
    {
        RaycastHit hit;
        Ray ray = new Ray(virtualPos + Vector3.up * 10, -transform.up);

        if (Physics.Raycast(ray, out hit, 100, cellLayer))
        {
            if (hit.collider.TryGetRigidbodyComponent(out Cell cell))
            {
                if (cell.IsOccupied())
                {
                    if(CanMergeWithCell(cell)) return cell;
                    return null;
                } 

                return cell;
            }
        }

        return null;
    }

    public bool CanMergeWithCell(Cell cell)
    {
        if(bullet.bulletLevel > 2) return false;
        if(cell.occupier.bullet.bulletLevel != bullet.bulletLevel) return false;

        return true;
    }
}
