using System.Collections.Generic;
using UnityEngine;

public class BlockBreakerLogic : MonoBehaviour 
{
    public int ballSpeedIncrement;	                    //The amount of speed the ball will increase by everytime it hits a brick

    bool wonGame;                                       //Set true when the game has been won
    GameObject ball;

    [SerializeField] int maxLives = 3;
    [SerializeField] GameObject paddle;                 //The paddle game object

    [SerializeField] List<GameObject> bricks = new();   //List of all the bricks currently on the screen
    [SerializeField] int brickCountX;                   //The amount of bricks that will be spawned horizontally (Odd numbers)
    [SerializeField] int brickCountY;                   //The amount of bricks that will be spawned vertically
    [SerializeField] Color[] colors;                    //The color array of the bricks. This can be modified to create different brick color patterns

    //Prefabs
    [SerializeField] GameObject ballPrefab;             //The ball game object
    [SerializeField] GameObject brickPrefab;            //The prefab of the Brick game object which will be spawned


    public void Start()
    {
        // Instatiate the ball
        ball = Instantiate(ballPrefab, ballPrefab.transform.position, Quaternion.identity);
        ball.GetComponent<Ball>().blockBreaker = this;
        ball.SetActive(false);

        GameManager.Instance.score = 0;
        GameManager.Instance.lives = maxLives;
        GameManager.Instance.UpdateScoreAndLives();
        wonGame = false;

        paddle.SetActive(true);
        ball.SetActive(true);
        paddle.GetComponent<Paddle>().ResetPaddle();
        CreateBrickArray();
    }

    //Spawns the bricks and sets their colours
    public void CreateBrickArray()
    {
        //'colorId' is used to tell which color is currently being used on the bricks. Increased by 1 every row of bricks
        int colorId = 0;                    

        for (int y = 0; y < brickCountY; y++)
        {
            for (int x = -(brickCountX / 2); x < (brickCountX / 2); x++)
            {
                //The 'pos' variable is where the brick will spawn at
                Vector3 pos = new Vector3(0.8f + (x * 1.6f), 1 + (y * 0.4f), 0);
                //Creates a new brick game object at the 'pos' value
                GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);
                //Gets the 'Brick' component of the game object and sets its 'manager' variable to this the GameManager
                brick.GetComponent<Brick>().blockBreaker = this;
                //Gets the 'SpriteRenderer' component of the brick object and sets the color
                brick.GetComponent<SpriteRenderer>().color = colors[colorId];

                bricks.Add(brick);
            }

            colorId++;                      //Increases the 'colorId' by 1 as a new row is about to be made

            if (colorId == colors.Length)   //If the 'colorId' is equal to the 'colors' array length. This means there is no more colors left
                colorId = 0;
        }
    }

    //Called when there is no bricks left and the player has won
    public void WinGame()
    {
        wonGame = true;
        paddle.SetActive(false);                        //Disables the paddle so it's invisible
        Destroy(ball);                                  //Disables the ball so it's invisible
        GameManager.Instance.SetGameOver(wonGame);      //Set the game over UI screen
    }

    //Called when the ball goes under the paddle and "dies"
    public void LiveLost()
    {
        GameManager.Instance.RemoveLife();              //Removes a life

        if (GameManager.Instance.lives <= 0)            //Are the lives less than 0? Are there no lives left?
        {
            paddle.SetActive(false);                    //Disables the paddle so it's invisible
            Destroy(ball);                              //Disables the ball so it's invisible
            GameManager.Instance.SetGameOver(wonGame);  //Set the game over UI screen

            for (int x = 0; x < bricks.Count; x++)      //Loops through the 'bricks' list and destroy each brick
                Destroy(bricks[x]);

            bricks = new List<GameObject>();            //Resets the 'bricks' list variable
        }
    }

    public void RemoveBrick(GameObject brick)
    {
        bricks.Remove(brick);

        if (bricks.Count == 0)
            WinGame();
    }
}
