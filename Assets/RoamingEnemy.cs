using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingEnemy : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int enemyHealth = 1;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject orangeBullet;
    private GameObject newBullet;
    [SerializeField] private GameObject enemyGeo;
    private NavMeshAgent agent;
    private BoxCollider enemyCollider;
    private bool canMove = true;
    private Transform target;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;



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
        StartCoroutine(EnemyShooting());
    }

    private IEnumerator UpdateTargetPosition()
    {
        while(canMove)
        {
            target = PlayerShipController.instance.gameObject.transform;
            agent.SetDestination(target.position);
            yield return new WaitForSeconds(.5f);
        }
    }

    private IEnumerator EnemyShooting()
    {
        while(canMove)
        {
            newBullet = ObjectPool.SharedInstance.GetPooledObject("OrangeEnemyProjectile");
            //audioSource.PlayOneShot(shotSound, 1f);
            if (newBullet != null)
            {
                newBullet.transform.position = transform.position;
                newBullet.transform.rotation = transform.rotation;
                newBullet.SetActive(true);
                yield return new WaitForSeconds(fireRate);
            }
        }

        //myTime = myTime + Time.deltaTime;

        //if (myTime > nextFire)
        //{
        //    nextFire = myTime + fireRate;
        //    newBullet = ObjectPool.SharedInstance.GetPooledObject("OrangeBullet");
        //    //audioSource.PlayOneShot(shotSound, 1f);
        //    if (orangeBullet != null)
        //    {
        //        newBullet.transform.position = transform.position;
        //        newBullet.transform.rotation = transform.rotation;
        //        newBullet.SetActive(true);
        //    }
        //    nextFire = nextFire - myTime;
        //    myTime = 0.0F;
        //}
    }
}
