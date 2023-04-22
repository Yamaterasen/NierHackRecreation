using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private AudioMixer audioMixer;



    private void Awake()
    {
        //Singleton instance
        instance = this;
    }

    private void Start()
    {

    }

    public void OnHackBegin()
    {
        StartCoroutine(DelayGamemodeTransition());
    }

    private IEnumerator DelayGamemodeTransition()
    {
        HackStartedEvent?.Invoke();
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "Volume1", 1f, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "Volume2", 1f, 1f));
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
