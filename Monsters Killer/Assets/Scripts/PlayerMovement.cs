using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] Transform head;

    float xRotation;
    bool isGrounded = false;
    Rigidbody rb;
    float jumpForce = 5;
    float speed = 2;
    Vector3 movement;
    Vector2 lookMovement;
    [SerializeField] float senstivity = 0.5f;

    public void OnMove(InputAction.CallbackContext value)
    {
        SetMovement(value.ReadValue<Vector2>().y , value.ReadValue<Vector2>().x);
    }

    void SetMovement(float z, float x)
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
        lookMovement = value.ReadValue<Vector2>() * senstivity;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed /*&& isGrounded*/)
        {
            Jump();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = Application.platform == RuntimePlatform.Android ? 4 : 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            SetMovement(joystick.Vertical , joystick.Horizontal);
        }
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

}
