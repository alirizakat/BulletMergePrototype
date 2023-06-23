using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPistol : MonoBehaviour
{
    public static PlayerPistol instance;

    public bool isWorking;
    public bool invincible;

    public float forwardSpeed = 5f;
    public float sidewaysSpeed = 10f;
    public float minX = -50f;
    public float maxX = 120f;
    public float transitionSpeed = 10f;
    public float childObjectDistance = 11f;

    private bool isMoving = false;
    private bool isTransitioning = false;
    private float targetPositionX;
    private float transitionStartTime;
    private float transitionStartX;

    public List<Transform> childObjects = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BulletManager.instance.BulletsFinishedEvent += BulletFinished;
    }

    private void BulletFinished()
    {
        Shooter[] shooterScripts = GetComponentsInChildren<Shooter>();

        foreach (Shooter shooterScript in shooterScripts)
        {
            if (shooterScript.canShoot && !childObjects.Contains(shooterScript.transform)) childObjects.Add(shooterScript.transform);
        }
        foreach (Shooter shooterScript in shooterScripts)
        {
            if (!shooterScript.canShoot) 
            {
                Destroy(shooterScript.gameObject);
            }     
        }
        AdjustChildObjectPositions();
    }

    private void Update()
    {
        if (!isWorking)
            return;

        if (InputManager.instance.touchState == InputManager.TouchState.Continue)
        {
            if (!isTransitioning)
            {
                isMoving = true;
                float touchDeltaX = InputManager.instance.touchData.frameDelta.x * Screen.width;
                targetPositionX = Mathf.Clamp(transform.position.x + touchDeltaX * sidewaysSpeed / Screen.width, minX, maxX);
            }
        }
        else if (InputManager.instance.touchState == InputManager.TouchState.End || InputManager.instance.touchState == InputManager.TouchState.None)
        {
            if (isMoving)
            {
                isTransitioning = true;
                transitionStartTime = Time.time;
                transitionStartX = transform.position.x;
            }
            isMoving = false;
        }

        if (isMoving)
        {
            Vector3 forwardDirection = transform.forward;
            Vector3 movementVector = forwardDirection * forwardSpeed * Time.deltaTime;
            Vector3 targetPosition = transform.position + movementVector;
            targetPosition.x = Mathf.Clamp(targetPositionX, minX, maxX);

            transform.position = targetPosition;
        }
        else if (isTransitioning)
        {
            float transitionProgress = (Time.time - transitionStartTime) * transitionSpeed;
            float newX = Mathf.Lerp(transitionStartX, targetPositionX, transitionProgress);

            Vector3 currentPosition = transform.position;
            currentPosition.x = newX;

            transform.position = currentPosition;

            if (transitionProgress >= 1f)
            {
                isTransitioning = false;
            }
        }
    }

    private void AdjustChildObjectPositions()
    {
        int activeChildCount = 0;

        foreach (Transform childObject in childObjects)
        {
            if (childObject.gameObject.activeSelf)
            {
                childObject.transform.localPosition = new Vector3(activeChildCount * childObjectDistance, 0f, 150f);

                activeChildCount++;
            }
            else
            {
                childObject.transform.localPosition = Vector3.zero;
            }
        }
    }
    #region Shield
    public void BecomeInvincible()
    {
        invincible = true;
        Invoke("LoseInvincible", 3f);
    }

    private void LoseInvincible()
    {
        invincible = false;
    }
    #endregion
}
