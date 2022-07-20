using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    public Vector3 hitPosition;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position , hitPosition , bulletSpeed);
        if (transform.position == hitPosition)
        {
            Destroy(gameObject);
        }
    }
}
