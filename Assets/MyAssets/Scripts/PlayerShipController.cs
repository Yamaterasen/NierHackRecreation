using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private Camera topDownCam;
    private Rigidbody shipRigidbody;
    private Vector2 move;
    private Vector2 mouseLook;
    private Vector3 rotationTarget;

    private void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        RotatePlayer();
    }
    private void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        shipRigidbody.velocity = movement * speed;
    }

    private void RotatePlayer()
    {
        //Vector2 rbPosition = new Vector2(shipRigidbody.position.x, shipRigidbody.position.z);
        //Vector2 aimDirection = mouseLook - rbPosition;
        //float angleValue = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //Quaternion angleQuaternion = Quaternion.Euler(0, angleValue, 0);
        //shipRigidbody.MoveRotation(angleQuaternion);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        if(Physics.Raycast(ray, out hit))
        {
            rotationTarget = hit.point;
        }

        Vector3 lookPosition = rotationTarget - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

        if(aimDirection != Vector3.zero)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, .5f);
            shipRigidbody.MoveRotation(rotation);
        }
    }

}
