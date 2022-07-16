using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    [SerializeField] GameObject impactEffect;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
        //Invoke("AdjustSpeed" , 0.1f);
        StartCoroutine(DestroyAfterTime());
    }

    void AdjustSpeed()
    {
        rb.AddForce(transform.forward * bulletSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
