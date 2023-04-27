using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingEnemy : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int enemyHealth = 1;
    [SerializeField] private GameObject enemyGeo;
    private Collider enemyCollider;


    private void Start()
    {
        enemyCollider = GetComponent<Collider>();
    }
    public void TakeDamage()
    {
        enemyHealth--;
        if(enemyHealth <= 0)
        {
            PlayDeathAnimation();
        }
    }
    private void PlayDeathAnimation()
    {
        enemyCollider.enabled = false;
        enemyGeo.SetActive(false);
    }
}
