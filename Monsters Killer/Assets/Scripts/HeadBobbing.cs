using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform headTransform;

    public float bobFrequency = 5f;
    float frequency;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    public float headBobSmoothing = 0.1f;

    private float walkingTime;
    private Vector3 targetCameraPosition;

    [SerializeField] PlayerManager playerManager;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = playerManager.GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        if (playerMovement.isGrounded)
        {
            walkingTime += Time.deltaTime;
        }
        else
        {
            walkingTime = 0;
        }

        if (playerManager.weponsHolder.aimGun)
        {
            frequency = 0.5f + (playerMovement.movement.magnitude > 0 
                && !playerManager.weponsHolder.isFireing ? 4.5f : 0);
            bobHorizontalAmplitude = 0.002f + (playerMovement.movement.magnitude > 0
                && !playerManager.weponsHolder.isFireing ? 0.002f : 0);
            bobVerticalAmplitude = 0.002f + (playerMovement.movement.magnitude > 0
                && !playerManager.weponsHolder.isFireing ? 0.002f : 0);
        }
        else
        {

            if (playerMovement.movement.magnitude > 0)
            {
                frequency = bobFrequency * (playerMovement.speed > 2 ? 2 : 1);
                bobHorizontalAmplitude = playerMovement.speed > 2 ? 0.1f : 0.05f;
                bobVerticalAmplitude = playerMovement.speed > 2 ? 0.1f : 0.05f;
            }
            else
            {
                frequency = 1.25f;
                bobHorizontalAmplitude = 0.01f;
                bobVerticalAmplitude = 0.01f;
            }
        }
        

        targetCameraPosition = headTransform.position + CalculateHeadBobOffset(walkingTime);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition,
            playerManager.weponsHolder.aimGun ? 10 : headBobSmoothing);

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
            horizontalOffset = Mathf.Cos(t * frequency) * bobHorizontalAmplitude;
            verticalOffset = Mathf.Sin(t * frequency * 2) * bobVerticalAmplitude;

            offset = headTransform.right * horizontalOffset + headTransform.up * verticalOffset;

        }

        return offset;
    }

}
