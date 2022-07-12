using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider != transform.parent)
        {
            transform.parent.GetComponent<PlayerMovement>().SetIsGroundedState(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider != transform.parent)
        {
            transform.parent.GetComponent<PlayerMovement>().SetIsGroundedState(false);
        }
    }
}
