using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] private float bulletSpeed;

    void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject objectCollided = collision.gameObject;
        if (objectCollided.CompareTag("Player"))
        {
            objectCollided.GetComponent<PlayerShipController>().TakeDamage();
        }
        gameObject.SetActive(false);
    }
}
