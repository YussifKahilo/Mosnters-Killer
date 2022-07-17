using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    [SerializeField] GameObject impactEffect;
    public Vector3 shootPosition;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
        StartCoroutine(DestroyAfterTime());
    }



    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform ie= Instantiate(impactEffect, transform.position, Quaternion.LookRotation(shootPosition - transform.position)).transform;
        Destroy(gameObject);
    }

}
