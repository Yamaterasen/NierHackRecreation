using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using StarterAssets;

public class GamemodeManager : MonoBehaviour
{
    public static GamemodeManager instance;
    public delegate void HackStarted();
    public static event HackStarted HackStartedEvent;
    public delegate void HackMiniGameReady();
    public static event HackMiniGameReady HackMiniGameReadyEvent;
    [SerializeField] private PlayerInput playerThirdPersonInput;
    [SerializeField] private PlayerInput shipPlayerInput;



    private void Awake()
    {
        //Singleton instance
        instance = this;
    }

    private void Start()
    {
        shipPlayerInput.gameObject.SetActive(false);
    }

    public void OnHackBegin()
    {
        StartCoroutine(DelayGamemodeTransition());
    }
    private IEnumerator DelayGamemodeTransition()
    {
        HackStartedEvent?.Invoke();
        //Delay before running OnHackMiniGameReady
        yield return new WaitForSeconds(.5f);
        OnHackMiniGameReady();
    }

    public void OnHackMiniGameReady()
    {
        HackMiniGameReadyEvent?.Invoke();
        RenderSettings.fog = false;
        playerThirdPersonInput.enabled = false;
        shipPlayerInput.gameObject.SetActive(true);
        shipPlayerInput.ActivateInput();
        shipPlayerInput.SwitchCurrentActionMap("PlayerShip");
    }



}
