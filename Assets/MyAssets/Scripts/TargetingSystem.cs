using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TargetingSystem : MonoBehaviour
{
    private StarterAssetsInputs input;
    private PlayerInput playerInput;

    public static TargetingSystem instance;

    public List<HackableEnemy> hackableEnemies;
    public List<HackableEnemy> hackableEnemiesInRange;

    public HackableEnemy currentTarget;

    [Header("UI")]
    [SerializeField] private RectTransform rectImage;
    [SerializeField] private RectTransform hackParent;
    [SerializeField] private RectTransform hackRing;
    [SerializeField] private Image hackFill;
    public float rectSizeMultiplier = 2;

    [Header("Parameters")]
    [SerializeField] private float hackFillSpeed = .07f;
    //Weight values that determine what distance (screen/player) gets prioritized
    [SerializeField] float screenDistanceWeight = 1;
    [SerializeField] float positionDistanceWeight = 8;
    //Min Distance for targets
    public float minReachDistance = 70;
    //public float targetDisableCooldown = 4;

    [Header("Zoom Camera")]
    [SerializeField] private Transform playerFocusPos;
    [SerializeField] private CinemachineVirtualCamera enemyZoomCam;


    private void Awake()
    {
        //Singleton instance
        instance = this;
    }

    private void Start()
    {
        //Get player input to read when hack begins/ends
        playerInput = GetComponent<PlayerInput>();
        input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        //If no enemy is in range, disable reticle
        if (hackableEnemiesInRange.Count < 1)
        {
            rectImage.gameObject.SetActive(false);
            hackParent.gameObject.SetActive(false);
            return;
        }

        //Get the current target, and prepare zoom in camera to focus on it
        currentTarget = hackableEnemiesInRange[TargetIndex()];
        enemyZoomCam.LookAt = currentTarget.transform;
        Transform currentTargetPos = currentTarget.transform;
        //Move zoom camera in between player position and targeted enemy
        enemyZoomCam.transform.position = (currentTargetPos.position + playerFocusPos.position) / 2f;


        //Enable reticle and hack slider UI
        rectImage.gameObject.SetActive(true);
        hackParent.gameObject.SetActive(true);
        rectImage.transform.position = ClampedScreenPosition(currentTarget.transform.position);
        float distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        rectImage.sizeDelta = new Vector2(Mathf.Clamp(115 - (distanceFromTarget - rectSizeMultiplier), 10, 25), Mathf.Clamp(115 - (distanceFromTarget - rectSizeMultiplier), 10, 25));
        Vector3 reticlePosition = rectImage.transform.position;
        hackParent.transform.position = new Vector3(reticlePosition.x, reticlePosition.y + 50, reticlePosition.z);


        //If hack button pressed, begin filling hack slider
        if(input.hack)
        {
            hackRing.gameObject.SetActive(true);
            if (hackFill.fillAmount < 1)
            {
                hackFill.fillAmount += hackFillSpeed;
            }
            if(hackFill.fillAmount >= 1)
            {
                playerInput.SwitchCurrentActionMap("BlockInputs");
                GamemodeManager.instance.OnHackBegin();
            }
        }
        else
        {
            hackFill.fillAmount = 0;
            hackRing.gameObject.SetActive(false);
        }
    }

    public int TargetIndex()
    {
        //Creates an array where the distances between the target and the screen/player will be stored
        float[] distances = new float[hackableEnemiesInRange.Count];

        //Populates the distances array with the sum of the Target distance from the screen center and the Target distance from the player
        for (int i = 0; i < hackableEnemiesInRange.Count; i++)
        {

            distances[i] =
                (Vector2.Distance(Camera.main.WorldToScreenPoint(hackableEnemiesInRange[i].transform.position), MiddleOfScreen()) * screenDistanceWeight)
                +
                (Vector3.Distance(transform.position, hackableEnemiesInRange[i].transform.position) * positionDistanceWeight);
        }

        //Finds the smallest of the distances
        float minDistance = Mathf.Min(distances);

        int index = 0;

        //Find the index number relative to the target with the smallest distance
        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;

    }

    public void StopTargetFocus()
    {
        currentTarget = null;
    }

    Vector3 ClampedScreenPosition(Vector3 targetPos)
    {
        Vector3 WorldToScreenPos = Camera.main.WorldToScreenPoint(targetPos);
        Vector3 clampedPosition = new Vector3(Mathf.Clamp(WorldToScreenPos.x, 0, Screen.width), Mathf.Clamp(WorldToScreenPos.y, 0, Screen.height), WorldToScreenPos.z);
        return clampedPosition;
    }

    Vector2 MiddleOfScreen()
    {
        return new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
