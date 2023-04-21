using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackableEnemy : MonoBehaviour
{
    public Transform player;
    public ThirdPersonController playerMovement;

    public bool inRange = false;

    private void Start()
    {
        playerMovement = FindObjectOfType<ThirdPersonController>();
        player = playerMovement.transform;
        CheckIfInRange();

        if (!TargetingSystem.instance.hackableEnemies.Contains(this))
        {
            TargetingSystem.instance.hackableEnemies.Add(this);

            if (inRange)
            {
                TargetingSystem.instance.hackableEnemiesInRange.Add(this);
            }
        }
    }

    private void Update()
    {
        CheckIfInRange();
    }

    private void CheckIfInRange()
    {
        Vector3 playerToEnemyDistance = transform.position - playerMovement.transform.position;
        bool isBehindPlayer = (Vector3.Dot(Camera.main.transform.forward, playerToEnemyDistance) < 0);

        if (Vector3.Distance(transform.position, player.position) < TargetingSystem.instance.minReachDistance && !isBehindPlayer && !inRange)
        {
            inRange = true;
            if (TargetingSystem.instance.hackableEnemies.Contains(this))
                TargetingSystem.instance.hackableEnemiesInRange.Add(this);

        }

        if ((Vector3.Distance(transform.position, player.position) > TargetingSystem.instance.minReachDistance || isBehindPlayer) && inRange)
        {
            inRange = false;
            if (TargetingSystem.instance.hackableEnemiesInRange.Contains(this))
                TargetingSystem.instance.hackableEnemiesInRange.Remove(this);

            if (TargetingSystem.instance.currentTarget == this)
            {
                TargetingSystem.instance.StopTargetFocus();
            }

        }
    }

    private void OnBecameVisible()
    {
        if (!TargetingSystem.instance.hackableEnemies.Contains(this))
        {
            TargetingSystem.instance.hackableEnemies.Add(this);

            if (inRange)
                TargetingSystem.instance.hackableEnemiesInRange.Add(this);
        }
    }

    private void OnBecameInvisible()
    {
        if (TargetingSystem.instance.hackableEnemies.Contains(this))
        {
            TargetingSystem.instance.hackableEnemies.Remove(this);

            if (TargetingSystem.instance.hackableEnemiesInRange.Contains(this))
                TargetingSystem.instance.hackableEnemiesInRange.Remove(this);
        }

        if (TargetingSystem.instance.currentTarget == this)
        {
            TargetingSystem.instance.StopTargetFocus();
        }
    }
}
