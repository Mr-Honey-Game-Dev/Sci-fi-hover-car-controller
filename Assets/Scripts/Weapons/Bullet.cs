using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Transform startTranform;
    Vector3 direction;
    [SerializeField] ParticleSystem Explosion;

    private void Start()
    {
        direction = startTranform.forward;
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity=(direction*Time.deltaTime*1000*speed); }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && collision.collider.GetComponent<Reference>().Ref == GetComponent<Reference>().Ref)
        {
            return;
        }
        if (Explosion)
        {
            Explosion.gameObject.SetActive(true);
            Explosion.Play();
            Explosion.gameObject.transform.parent = null;
        }
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<Reference>().Ref == GetComponent<Reference>().Ref)
        {
            return;
        }
        if (Explosion)
        {
            Explosion.gameObject.SetActive(true);
            Explosion.Play();
            Explosion.gameObject.transform.parent = null;
        }
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
  
}
