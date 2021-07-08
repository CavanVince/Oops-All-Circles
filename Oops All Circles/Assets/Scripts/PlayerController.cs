using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float bulletSpeed;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject bullet;

    private Vector2 playerPosition;
    private Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
            GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            //Calculate the vector
            Vector2 shootVector = mousePosition - playerPosition;
            shootVector.Normalize();

            spawnedBullet.GetComponent<Rigidbody2D>().AddForce(shootVector * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
