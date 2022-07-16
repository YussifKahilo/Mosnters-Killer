using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform headTransform;

    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    public float headBobSmoothing = 0.1f;

    private bool isWalking = false;
    private float walkingTime;
    private Vector3 targetCameraPosition;

    public Transform groundCheck;
    public LayerMask groundLyaers;
    public float groundCheckRadius;

    private bool grounded;

   // public bl_Joystick joystick;
    float vertical = 0, horizontal = 0;

    private void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLyaers);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        /*vertical = joystick.Vertical;
        horizontal = joystick.Horizontal;*/

        if ((vertical >= 1 || vertical <= -1
            || horizontal >= 1 || horizontal <= -1) && grounded)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (!isWalking)
        {
            walkingTime = 0;
        }
        else
        {
            walkingTime += Time.deltaTime;
        }

        targetCameraPosition = headTransform.position + CalculateHeadBobOffset(walkingTime);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, headBobSmoothing);

        if ((cameraTransform.position - targetCameraPosition).magnitude <= 0.001)
        {
            cameraTransform.position = targetCameraPosition;
        }

    }


    private Vector3 CalculateHeadBobOffset(float t)
    {
        float horizontalOffset = 0;
        float verticalOffset = 0;
        Vector3 offset = Vector3.zero;

        if (t > 0)
        {
            horizontalOffset = Mathf.Cos(t * bobFrequency) * bobHorizontalAmplitude;
            verticalOffset = Mathf.Sin(t * bobFrequency * 2) * bobVerticalAmplitude;

            offset = headTransform.right * horizontalOffset + headTransform.up * verticalOffset;

        }

        return offset;
    }

}
