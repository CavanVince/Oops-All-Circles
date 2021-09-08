using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using System.Diagnostics;

enum Powerup 
{
    normal,
    quad
}

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float bulletSpeed;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject deathParticles;

    private Vector2 playerPosition;
    private Vector2 mousePosition;
    private Powerup powerupStatus;
    private int health;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        //Increase the amount of times the server is updated
        PhotonNetwork.SerializationRate = 50;

        view = GetComponent<PhotonView>();
        powerupStatus = Powerup.normal;
        health = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Only run the code if it is the player's specific character
        if (view.IsMine)
        {
            //Update the player and mouse positions
            playerPosition = transform.position;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Move the player
            MovePlayer();

            //Shoot the bullets
            ShootBullet();
        }
    }

    /// <summary>
    /// Moves the player object based on the given user input.
    /// </summary>
    void MovePlayer()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f);
    }


    /// <summary>
    /// Instantiates a bullet prefab when the player left clicks
    /// </summary>
    void ShootBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Calculate the vector
            Vector3 shootVector = mousePosition - playerPosition;
            shootVector.Normalize();

            //Determine if the player has a power-up
            switch (powerupStatus)
            {
                case Powerup.normal:   
                    GameObject spawnedBullet = PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
                    //GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    spawnedBullet.GetComponent<Rigidbody2D>().AddForce(shootVector * bulletSpeed, ForceMode2D.Impulse);
                    spawnedBullet.GetComponent<Bullet>().ParentPlayer = gameObject;
                    break;

                case Powerup.quad:
                    GameObject[] spawnedBullets = new GameObject[4];
                    for (int i = 0; i < spawnedBullets.Length; i++) 
                    {
                        //THIS INSTANTIATION NEEDS TO BE MODIFIED DUE TO THE FACT THAT THE BULLETS SPAWN IN THE PLAYER
                        spawnedBullets[i] = PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
                    }

                    //Calculate the adjusted shoot vector for the other bullets
                    Vector2 adjustedShootVector = shootVector;
                    adjustedShootVector.x = -adjustedShootVector.x;

                    spawnedBullets[0].GetComponent<Rigidbody2D>().AddForce(shootVector * bulletSpeed, ForceMode2D.Impulse);
                    spawnedBullets[1].GetComponent<Rigidbody2D>().AddForce(-shootVector * bulletSpeed, ForceMode2D.Impulse);
                    spawnedBullets[2].GetComponent<Rigidbody2D>().AddForce(adjustedShootVector * bulletSpeed, ForceMode2D.Impulse);
                    spawnedBullets[3].GetComponent<Rigidbody2D>().AddForce(-adjustedShootVector * bulletSpeed, ForceMode2D.Impulse);
                    break;
            }

        }
    }

    /// <summary>
    /// Determines if the game is over
    /// </summary>
    void GameOver() 
    {
            if (health <= 0)
            {
                PhotonNetwork.Instantiate(deathParticles.name, transform.position, Quaternion.identity);
                //Instantiate(deathParticles, transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
                //Destroy(gameObject);
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Only call if it is the client's player being collided with
        if (view.IsMine)
        {
            if (collision.gameObject.CompareTag("Power-Up"))
            {
                powerupStatus = Powerup.quad;
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.CompareTag("Bullet"))
            {
                if (collision.GetComponent<Bullet>().ParentPlayer != gameObject)
                {
                    health--;
                    Destroy(collision.gameObject);

                    //End the game if the player's health <= 0
                    GameOver();
                }
            }
        }
    }
}
