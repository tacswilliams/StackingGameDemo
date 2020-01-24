using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public bool collidedWithGround = false;
    private int moveCount = 0;

    public Transform crateSpawnPos;

    private Vector3 cameraTargetPos;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI scoreText;
    public int score = 0;

    public GameObject gameOverText;
    Crate newCrate;

    public AudioSource audioSource;
    public AudioClip lose;

    // Start is called before the first frame update
    void Start()
    {
        cameraTargetPos = Camera.main.transform.position;
        // Sets the best sore from the last round
        bestScoreText.text = "Best: " + ScoreTracker.INSTANCE.bestScore.ToString("0000");
        // Hides the game over text
        gameOverText.SetActive(false);
    }

    public void SpawnNewCrate()
    {
        if (!gameOver)
        {
            // adjusts the z positon of the newly created crate
            crateSpawnPos.position = new Vector3(crateSpawnPos.position.x, crateSpawnPos.position.y, -5);
            // Creates the new crate
            newCrate = Instantiate(Resources.Load<Crate>("Prefabs/Crate"), crateSpawnPos.position, Quaternion.identity);
            // Increases score and updates UI text
            score += 1;
            scoreText.text = score + "";
            if (score > ScoreTracker.INSTANCE.bestScore)
            {
                // If current score is larger than the best score,
                // set current score to the best score and update UI text
                ScoreTracker.INSTANCE.bestScore = score;
                // Adds leading 0s to best score
                bestScoreText.text = "Best: " + ScoreTracker.INSTANCE.bestScore.ToString("0000");
            }
        }
    }

    public void CollideWithGround()
    {
        if (!collidedWithGround)
        {
            // If none of the crates have hit the ground, 
            // then we have dropped our first crate
            collidedWithGround = true;

            // We keep track of this variable to make sure if 
            // another crate falls on the ground, then we should
            // set the game to game over
        }
        else
        {
            audioSource.PlayOneShot(lose);
            if (!newCrate.isDropped && !gameOver)
            {
                // If the gmae is over and the crate has not been
                // dropped, then destroy that crate
                Destroy(newCrate.gameObject);
            }
            // Set gameOVer to true and display game over text

            gameOver = true;
            gameOverText.SetActive(true);

        }
    }

    public void MoveCamera()
    {
        // After 3 successful crate drops the camera
        // moves up 3 units
        moveCount++;
        if(moveCount == 3)
        {
            moveCount = 0;
            cameraTargetPos.y += 3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Moves camera to the target position
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                                                        cameraTargetPos, Time.deltaTime);

       
        if (gameOver)
        {
            // If the game is over the new target position is back at the bottom
            // of the game
            cameraTargetPos = new Vector3(0, 0, -10);
        }
       

        if(gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // If the game is over and space is pressed,
            // then reload the game.
            SceneManager.LoadScene(0);
        }
    }
}
