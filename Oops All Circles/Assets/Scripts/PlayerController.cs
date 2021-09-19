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

public class PlayerController : MonoBehaviour, IPunObservable
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
            view.RPC("InstantiateBullet", RpcTarget.All, shootVector);
        }
    }

    /// <summary>
    /// RPC to spawn local bullets on both clients
    /// </summary>
    /// <param name="shootVector">Vector that the bullet will travel on</param>
    [PunRPC]
    void InstantiateBullet(Vector3 shootVector)
    {
        GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody2D>().AddForce(shootVector * bulletSpeed, ForceMode2D.Impulse);
        spawnedBullet.GetComponent<Bullet>().ParentPlayer = gameObject;
    }

    /// <summary>
    /// Reduces the player's health and then determines if the game is over
    /// </summary>
    public void GameOver()
    {
        health--;
        if (health <= 0)
        {
            PhotonNetwork.Instantiate(deathParticles.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    /// <summary>
    /// Have this here because Unity console started crying at me...
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
