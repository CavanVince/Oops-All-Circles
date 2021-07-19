using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

enum Powerup 
{
    normal,
    quad
}

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float bulletSpeed;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject bullet;

    private Vector2 playerPosition;
    private Vector2 mousePosition;
    private Powerup powerupStatus;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        powerupStatus = Powerup.normal;
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

            //Render line for aiming
            AimLine();

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
    /// Renders the line that the player uses to aim
    /// </summary>
    void AimLine()
    {
        lineRenderer.SetPosition(0, playerPosition);
        lineRenderer.SetPosition(1, mousePosition);
    }


    /// <summary>
    /// Instantiates a bullet prefab when the player left clicks
    /// </summary>
    void ShootBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Calculate the vector
            Vector2 shootVector = mousePosition - playerPosition;
            shootVector.Normalize();

            //Determine if the player has a power-up
            switch (powerupStatus)
            {
                case Powerup.normal:
                    GameObject spawnedBullet = PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
                    spawnedBullet.GetComponent<Rigidbody2D>().AddForce(shootVector * bulletSpeed, ForceMode2D.Impulse);
                    break;

                case Powerup.quad:
                    GameObject[] spawnedBullets = new GameObject[4];
                    for (int i = 0; i < spawnedBullets.Length; i++) 
                    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Power-Up"))
        {
            powerupStatus = Powerup.quad;
            Destroy(collision.gameObject);
        }
    }
}
