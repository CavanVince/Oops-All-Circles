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
    //Speed variables
    public float moveSpeed;
    public float bulletSpeed;

    //Bullet related variables
    public float cooldownTimer;
    private bool canShoot;

    //Instantiable prefabs
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject deathParticles;

    //Position variables
    private Vector2 playerPosition;
    private Vector2 mousePosition;

    //Player status variables
    private Powerup powerupStatus;
    private int health;

    //Component Variables
    private GameManager gameManager;
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        //Increase the amount of times the server is updated
        PhotonNetwork.SerializationRate = 50;

        gameManager = FindObjectOfType<GameManager>();
        view = GetComponent<PhotonView>();
        powerupStatus = Powerup.normal;
        health = 1;
        canShoot = true;
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

    #region Movement

    /// <summary>
    /// Moves the player object based on the given user input.
    /// </summary>
    void MovePlayer()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f);
    }

    #endregion

    #region Shooting

    /// <summary>
    /// Instantiates a bullet prefab when the player left clicks and they aren't on cooldown
    /// </summary>
    void ShootBullet()
    {
        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            canShoot = false;

            //Calculate the vector
            Vector3 shootVector = mousePosition - playerPosition;
            shootVector.Normalize();
            view.RPC("InstantiateBullet", RpcTarget.All, shootVector);
            StartCoroutine(ShotCooldown());
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
    /// Coroutine that starts the shot cooldown for the player
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        canShoot = true;
    }

    #endregion

    #region Logic
    /// <summary>
    /// Reduces the player's health and then tells the game manager if the round is over
    /// </summary>
    public void GameOver()
    {
        if (view.IsMine)
        {
            health--;
            if (health <= 0)
            {
                gameManager.EndRound();
                PhotonNetwork.Instantiate(deathParticles.name, transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
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
    #endregion
}
