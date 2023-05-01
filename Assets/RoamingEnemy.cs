using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingEnemy : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int enemyHealth = 1;
    [SerializeField] private GameObject enemyGeo;
    private NavMeshAgent agent;
    private BoxCollider enemyCollider;
    private bool canMove = true;
    private Transform target;


    private void Start()
    {
        enemyCollider = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        StartCoroutine(StartChasingPlayer());
    }
    public void TakeDamage()
    {
        enemyHealth--;
        if(enemyHealth <= 0)
        {
            canMove = false;
            agent.isStopped = true;
            PlayDeathAnimation();
        }
    }
    private void PlayDeathAnimation()
    {
        enemyCollider.enabled = false;
        enemyGeo.SetActive(false);
    }

    private IEnumerator StartChasingPlayer()
    {
        yield return new WaitForSeconds(1f);
        target = PlayerShipController.instance.gameObject.transform;
        agent.SetDestination(target.position);
        StartCoroutine(UpdateTargetPosition());
    }

    private IEnumerator UpdateTargetPosition()
    {
        while(canMove)
        {
            Debug.LogWarning("target updated!");
            target = PlayerShipController.instance.gameObject.transform;
            agent.SetDestination(target.position);
            yield return new WaitForSeconds(.5f);
        }
    }

}
