using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timer;
    private float wallBounceCounter;

    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
        wallBounceCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Decrement the timer and possibly destroy the bullet
        timer -= Time.deltaTime;
        if (timer <= 0) 
        {
            Destroy(gameObject);
        }

        //Determine if the ball hit two walls
        if (wallBounceCounter < 0) 
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        wallBounceCounter--;
    }
}
