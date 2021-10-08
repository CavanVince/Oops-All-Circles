using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timer;
    private float wallBounceCounter;
    private GameObject parentPlayer;

    public GameObject ParentPlayer
    {
        get
        {
            return parentPlayer;
        }
        set
        {
            parentPlayer = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wallBounceCounter = 1;
    }

    void Update()
    {
        Destroy(gameObject, timer); //Destroy the bullet after a set amount of time
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        wallBounceCounter--;

        //Determine if the ball hit two walls
        if (wallBounceCounter < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject != parentPlayer)
        {
            collision.GetComponent<PlayerController>().GameOver();
            Destroy(gameObject);
        }
    }
}
