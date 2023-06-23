using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public Transform bulletCam;
    public Transform pistolCam;

    public bool bulletCamMove = false;
    public bool pistolCamMove = false;
    public bool stop;
    public Transform pistol;
    void Awake()
    {
        instance = this;   
    }
    private void Start()
    {
        GameManager.instance.BulletFiredEvent += FollowBullets;
        GameManager.instance.LevelFailedEvent += Stop;
        GameManager.instance.LevelFinishedEvent += Stop;
        BulletManager.instance.BulletsFinishedEvent += FollowPistol;
    }

    private void Stop()
    {
        stop = true;
    }

    private void LateUpdate()
    {
        if (stop) return;
        if(bulletCamMove & FollowBulletZVal() != -1) 
        {
            Vector3 targetPos = new Vector3(bulletCam.position.x, bulletCam.position.y, FollowBulletZVal());
            bulletCam.position = Vector3.Lerp(bulletCam.position, targetPos, Time.deltaTime);
        }

        if (pistolCamMove)
        {
            Vector3 targetPos = new Vector3(pistolCam.position.x, pistolCam.position.y, pistol.position.z);
            pistolCam.position = Vector3.Lerp(pistolCam.position, targetPos, Time.deltaTime);
        }
    }
    private void FollowBullets()
    {
        bulletCamMove = true;
    }

    private void FollowPistol() 
    {
        bulletCamMove = false;
        pistolCamMove = true;
        pistolCam.GetComponent<CinemachineVirtualCamera>().Priority = 50;
    }

    public Vector3 ConvertToCameraRelativeInput(Vector2 input)
    {
        // first find out basis vectors
        Vector3 forward = bulletCam.forward;
        Vector3 right = bulletCam.right;

        // project onto ground plane & normalize
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // scale by input
        forward *= input.y;
        right *= input.x;

        return forward + right;
    }
    public float FollowBulletZVal() 
    {
        if (BulletManager.instance.aliveBullets.Count > 0)
        {
            return BulletManager.instance.aliveBullets[0].position.z;
        }
        else return -1;
    }
}
