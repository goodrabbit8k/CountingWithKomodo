using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5f;

    [SerializeField] AudioClip hit;

    float xSpeed;

    Animator bulletAnim;
    Rigidbody bulletRb;
    AudioSource bulletAudio;
    Player player;

    void Start()
    {
        bulletAnim = GetComponent<Animator>();
        bulletRb = GetComponent<Rigidbody>();
        bulletAudio = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();

        xSpeed = player.transform.localScale.x * bulletSpeed;   
    }

    void Update()
    {
        BulletBehavior();
        BulletBoundary();
    }

    void BulletBehavior()
    {
        bulletRb.velocity = new Vector2(xSpeed, 0f);
        transform.localScale = new Vector2(Mathf.Sign(xSpeed), 1f);
    }

    void BulletBoundary() 
    {
        if (transform.position.x >= 72f || transform.position.x <= -8f) 
        {
            xSpeed = 0f;
            Destroy(gameObject, 0.5f);
            bulletAnim.SetTrigger("isHit");
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Enemy Boss") 
        {
            xSpeed = 0f;
            Destroy(gameObject, 0.5f);
            bulletAudio.PlayOneShot(hit, 0.5f);
            bulletAnim.SetTrigger("isHit");
        }    
    }
}
