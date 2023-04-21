using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject playerFollowCam;
    [SerializeField] private GameObject enemyZoomCam;

    private void Awake()
    {
        GamemodeManager.HackStartedEvent += OnHackBegin;
        GamemodeManager.HackMiniGameReadyEvent += OnHackLoaded;
    }

    private void OnHackBegin()
    {
        playerFollowCam.gameObject.SetActive(false);
    }

    private void OnHackLoaded()
    {
        enemyZoomCam.SetActive(false);
    }
}
