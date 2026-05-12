using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BreakoutManager : MonoBehaviour
{
    //I use a static variable to make this accessible from anywhere
    //You can access this variable from anywhere by typing 'BreakoutManager.Me'
    //No need to capture the BreakoutManager in a variable name first, like you usually need
    public static BreakoutManager Me;
    //As a manager, I keep a link to all the major game elements
    public PaddleController Paddle;
    public BallController Ball;
    
    //The brick prefab
    public BrickController BrickPrefab;
    
    //I keep a list of all bricks that exist
    public List<BrickController> AllBricks =  new List<BrickController>();

    public int rows = 6;
    public int columns = 8;
    public Vector2 BrickStart = new Vector2(-7.3f, 3.6f);
    public float BrickSpacingX = 0.092f;
    public float BrickSpacingY = 0.092f;
    public int Score;
    public int Lives = 3;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LivesText;
    public string LoseSceneName = "Game Over";
    public string WinSceneName = "You Win";

    [Header("Brick Colors")]
    
    //This is not working since initially i didnt have any other color but white now i had to change in inspector almost made me lose my mind ;-;
    public Color[] NormalBrickColors =
    {
        Color.blue,
        Color.cyan,
        Color.green,
        new Color(1f, 0.5f, 0f),
        new Color(0.5f, 0f, 1f)
    };

    public Color NormalBrickDamagedColor = Color.gray;

    [Header("Strong Bricks")]
    [Range(0f, 1f)]
    public float StrongBrickChance = 0.2f;

    public Color StrongBrickColor = Color.red;
    public Color StrongBrickDamagedColor = Color.yellow;

    void Start()
    {
        //I need to register myself as 'the' BreakoutManager
        Me = this;
        SpawnBricks();
        UpdateUI();
    }

    void Update()
    {
        //Check to see if all the bricks have been broken
        if (AllBricks.Count == 0 && Score == rows * columns * 100)
        {
            SceneManager.LoadScene(WinSceneName);
        }
    }

    public void SpawnBricks()
    {
        SpriteRenderer sr = BrickPrefab.GetComponent<SpriteRenderer>();
        Vector2 brickSize = sr.bounds.size;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 pos = new Vector3(
                    BrickStart.x + j * (brickSize.x + BrickSpacingX),
                    BrickStart.y - i * (brickSize.y + BrickSpacingY),
                    0f
                );

                BrickController newBrick = Instantiate(
                    BrickPrefab,
                    pos,
                    Quaternion.identity
                );

                //Decide if this brick should be a strong brick
                bool isStrongBrick = Random.value < StrongBrickChance;

                if (isStrongBrick)
                {
                    newBrick.SetupBrick(
                        2,
                        250,
                        StrongBrickColor,
                        StrongBrickDamagedColor
                    );
                }
                else
                {
                    //Pick one normal color from the list for this brick
                    Color chosenColor = GetRandomNormalBrickColor();
                    //Normal bricks take 1 hit and give normal points
                    newBrick.SetupBrick(
                        1,
                        100,
                        chosenColor,
                        NormalBrickDamagedColor
                    );
                }
            }
        }
    }

    Color GetRandomNormalBrickColor()
    {
        if (NormalBrickColors != null && NormalBrickColors.Length > 0)
        {
            return NormalBrickColors[
                Random.Range(0, NormalBrickColors.Length)
            ];
        }
        //If the list is empty, just use white as a backup this backfired on me lowkey as even when i had the list it still said nope only had white
        //I had to manualy go on breakout manager and assign the colors there to make it work 

        return Color.white;
    }


   public void AddScore(int points)
   {
       Score += points;
       if (Paddle != null)
       {
           //As the score goes up, make the paddle smaller because why not make the game harder as you do better hehe 
           Paddle.UpdateSizeFrom(Score);
       }
       UpdateUI();
   }

   public bool LoseLife()
    {
        Lives--;
        UpdateUI();

        if (Lives <= 0)
        {
            SceneManager.LoadScene(LoseSceneName);
            return false;
        }

        return true;
    }

    void UpdateUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = "Score: " + Score;
        }
        if (LivesText != null)
        {
            LivesText.text = "Lives: " + Lives;
        }
    }
    
}
