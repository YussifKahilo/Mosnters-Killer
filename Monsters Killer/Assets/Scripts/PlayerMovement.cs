using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform headHolder;
    [SerializeField] float maxXRotation = 90;
    float xRotation;
    public bool isGrounded = false;
    Rigidbody rb;
    float jumpForce = 5;
    public float speed = 2;
    public Vector3 movement;

    Vector2 lookRotation;

    public float senstivity = 10f;
    public float aimSenstivity = 3.5f;
    WeponsHolder weponsHolder;

    public void OnMove(InputAction.CallbackContext value)
    {
        SetMovement(value.ReadValue<Vector2>().x , value.ReadValue<Vector2>().y);
    }

    void SetMovement(float x, float z)
    {
        movement = new Vector3(x , 0 , z).normalized;
    }

    public void OnRun(InputAction.CallbackContext value)
    {
        switch (value.phase)
        {
            case InputActionPhase.Started:
                speed = 4;
                if (weponsHolder.aimGun)
                {
                    weponsHolder.StopAim();
                }
                break;
            case InputActionPhase.Canceled:
                speed = 2;
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Setlooking(value.ReadValue<Vector2>().x, value.ReadValue<Vector2>().y);
    }

    void Setlooking(float x, float y)
    {
        lookRotation = new Vector2(x, y) *(weponsHolder.aimGun ? aimSenstivity : senstivity )* Time.deltaTime;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            if (weponsHolder.aimGun)
            {
                weponsHolder.StopAim();
            }
            Jump();
        }
    }

    void Start()
    {
        weponsHolder = GetComponent<PlayerManager>().weponsHolder;
        rb = GetComponent<Rigidbody>();
        speed = Application.platform == RuntimePlatform.Android ? 4 : 2;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
       
    }

    private void LateUpdate()
    {
        Look();
    }

    void Move()
    {
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void Look()
    {         
        xRotation = Mathf.Clamp(xRotation - lookRotation.y , -maxXRotation, maxXRotation);
        headHolder.localRotation = Quaternion.Euler(xRotation , 0 , 0);
        transform.Rotate(transform.up , lookRotation.x);
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void SetIsGroundedState(bool state)
    {
        isGrounded = state;
    }

}
