using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerShipController : MonoBehaviour
{
    [Header("Player Bullets")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private AudioClip shotSound;
    private GameObject newBullet;
    [SerializeField] private float fireRate = 0.5F;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;
    private bool isShooting;

    [Header("Player Movement")]
    [SerializeField] private float speed = 3;
    [SerializeField] private Camera topDownCam;
    private Rigidbody shipRigidbody;
    private Vector2 move;
    private Vector2 mouseLook;
    private Vector3 rotationTarget;

    [Header("Player Movement")]
    [SerializeField] private ParticleSystem shootParticleSystem;
    [SerializeField] private ParticleSystem trailParticleSystem;
    private ParticleSystem.EmissionModule shootEmission;
    private ParticleSystem.EmissionModule trailEmission;

    [Header("Health")]
    [SerializeField] private GameObject LeftShipGeo;
    [SerializeField] private GameObject RightShipGeo;
    [SerializeField] private int playerHealth = 3;

    public static PlayerShipController instance;
    private LayerMask groundLayer;

    private AudioSource audioSource;
    private void Awake()
    {
        //Singleton instance
        instance = this;

        shootEmission = shootParticleSystem.emission;
        trailEmission = trailParticleSystem.emission;
        shipRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        shootParticleSystem.Play();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        if (context.started)
        {
            trailEmission.enabled = true;
        }
        if (context.canceled)
        {
            trailEmission.enabled = false;
        }
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isShooting = true;
        }
        if(context.canceled)
        {
            isShooting = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        RotatePlayer();

        PlayerShooting();
    }
    private void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        shipRigidbody.velocity = movement * speed;
    }

    private void RotatePlayer()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        if(Physics.Raycast(ray, out hit, 50f, groundLayer))
        {
            rotationTarget = hit.point;
        }

        Vector3 lookPosition = rotationTarget - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

        if(aimDirection != Vector3.zero)
        {
            shipRigidbody.MoveRotation(rotation);
        }
    }

    private void PlayerShooting()
    {
        myTime = myTime + Time.deltaTime;

        if (isShooting && myTime > nextFire)
        {
            nextFire = myTime + fireRate;
            newBullet = ObjectPool.SharedInstance.GetPooledObject("PlayerProjectile");
            audioSource.PlayOneShot(shotSound, 1f);
            if (bullet != null)
            {
                newBullet.transform.position = bulletSpawn.transform.position;
                newBullet.transform.rotation = bulletSpawn.transform.rotation;
                newBullet.SetActive(true);
            }
            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }
    }

    public void TakeDamage()
    {
        playerHealth--;
        switch (playerHealth)
        {
            case 2:
                LeftShipGeo.SetActive(false);
                break;
            case 1:
                RightShipGeo.SetActive(false);
                break;
            default:
                break;
        }
        if (playerHealth <= 0)
        {
            //canMove = false;
            //PlayDeathAnimation();
        }
    }
}
