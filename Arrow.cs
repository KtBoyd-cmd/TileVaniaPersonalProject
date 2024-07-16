using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 8f;
    [SerializeField] AudioClip flemmySFX;
    [SerializeField] AudioClip arrow;
    [SerializeField] int pointsForKill = 100;



    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;
    
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
        AudioSource.PlayClipAtPoint(arrow, Camera.main.transform.position);

    }    


    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
            { 
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
            }
        
       
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject, 0f);
            AudioSource.PlayClipAtPoint(flemmySFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(pointsForKill);

        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
