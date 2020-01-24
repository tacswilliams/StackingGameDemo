using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public bool isDropped = false;
    public bool isLanded = false;

    public Rigidbody2D rb;

    public float xBounds = 1.5f;
    public float horizontalMoveSpeed = 10;

    public GameManager gm;

    public AudioClip drop;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Finds the Game MAnager and sets it as a reference 
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Finds the component called Rigidbody2D on the current gameObject
        rb = GetComponent<Rigidbody2D>();

        // Sets gravity to 0 so the crate doesn't fall as soon as it appears
        rb.gravityScale = 0;

        // Random number generator that determines if 
        // the crate goes left or right initally
        int random = Random.Range(0, 10);
        if(random % 2 == 0)
        {
            xBounds *= -1;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDropped)
        {
            // Moves the crate left to right and right to left
            transform.position = Vector3.Lerp(  transform.position,
                                                new Vector3(xBounds, transform.position.y, transform.position.z),
                                                Mathf.PingPong(Time.deltaTime * horizontalMoveSpeed, 10));

            // If the crate is close to the edge send it in the opposite direction
            if(Mathf.Abs(transform.position.x) > Mathf.Abs(xBounds * 0.99f))
            {
                xBounds *= -1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDropped)
        {
            // If I have not been dropped and space is pressed,
            // set isDropped to true and turn on gravity
            isDropped = true;
            rb.gravityScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gm.collidedWithGround)
        {
            // This will only happen to the first crate
            // This is key to getting our game to work correctly
            rb.Sleep();
        }

        if (collision.gameObject.name == "Ground")
        {
            // Run this evey time a crate collides with the ground
            gm.CollideWithGround();
        }
        if (!gm.gameOver && !isLanded)
        {
            audioSource.PlayOneShot(drop);

            // Once a crate has collided it has offically landed
            isLanded = true;

            // Move teh camera based on the logic found in the MoveCamera method
            gm.MoveCamera();
            
            // Create a new crate
            gm.SpawnNewCrate();
        }
    }
}
