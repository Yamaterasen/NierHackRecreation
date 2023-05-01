using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed;

    void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject objectCollided = collision.gameObject;
        if (objectCollided.CompareTag("Enemy"))
        {
            objectCollided.GetComponent<RoamingEnemy>().TakeDamage();
        }
        gameObject.SetActive(false);
    }
}
