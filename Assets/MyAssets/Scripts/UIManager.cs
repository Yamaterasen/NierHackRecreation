using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Image whiteBackground;

    private void Awake()
    {
        GamemodeManager.HackStartedEvent += FadeToWhite;
        GamemodeManager.HackMiniGameReadyEvent += FadeToTransparent;
    }

    private void OnEnable()
    {

    }

    private void FadeToWhite()
    {
        whiteBackground?.DOFade(1, 0.5f).SetEase(Ease.InExpo);
    }

    private void FadeToTransparent()
    {
        whiteBackground?.DOFade(0, 0.5f).SetEase(Ease.InExpo);
    }

    private void OnDestroy()
    {
        whiteBackground?.DOKill();
    }
}
