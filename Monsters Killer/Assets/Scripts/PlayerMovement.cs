using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform head;

    Animator anim;

    float xRotation;
    bool isGrounded = false;
    Rigidbody rb;
    float jumpForce = 5;
    float speed = 2;
    Vector3 movement;
    Vector2 lookMovement;
    float senstivity = 10f;


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
        lookMovement = new Vector2(x, y) * senstivity * Time.deltaTime;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            Jump();
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = Application.platform == RuntimePlatform.Android ? 4 : 2;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    void Move()
    {
        anim.SetFloat("X", (movement.x * speed) / 4.0f);
        anim.SetFloat("Y", (movement.z * speed) / 4.0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void Look()
    {
        xRotation = Mathf.Clamp(xRotation - lookMovement.y , -70 , 70);
        head.localRotation = Quaternion.Euler(xRotation , 0 , 0);
        transform.Rotate(transform.up , lookMovement.x );
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
