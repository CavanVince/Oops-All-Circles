using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
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
    }
}
