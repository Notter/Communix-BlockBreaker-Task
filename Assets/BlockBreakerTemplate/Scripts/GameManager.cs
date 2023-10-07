using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BlockBreakerLogic BlockBreaker;                      //The Game logic

    [HideInInspector] public int score;                         //The player's current score
    [HideInInspector] public int lives;                         //The amount of lives the player has remaining
                                                                
    [SerializeField] TextMeshProUGUI ScoreText;                 //The Text component that will display the score
    [SerializeField] TextMeshProUGUI LivesText;                 //The Text component that will display the lives
    [SerializeField] TextMeshProUGUI GameOverScoreText;         //The Text component that will display the score when the player lost
    [SerializeField] TextMeshProUGUI Title;                     
    [SerializeField] TextMeshProUGUI Subtitle;                  
                                                                
    [SerializeField] GameObject GameOverScreen;                 //The game over screen game object
    [SerializeField] GameObject WinScreen;                      //The win screen game object

    const string titleParamter = "Menu_Title";
    const string subtitleParamter = "Menu_Subtitle";

    async void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return; // only used in Main Menu (scene ID 0)

        var mainMenuTexts = await new FireBaseRemoteConfig().GetParameters(titleParamter, subtitleParamter);

        Title.text = mainMenuTexts[titleParamter].StringValue;
        Subtitle.text = mainMenuTexts[subtitleParamter].StringValue;
    }

    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void LoadScene(int sceneID) => SceneManager.LoadScene(sceneID);

    //Called when the game is over
    public void SetGameOver(bool gameWon)
    {
        if (gameWon)
            WinScreen.SetActive(true);
        else
        {
            GameOverScreen.SetActive(true);
            GameOverScoreText.text = "<b>YOU ACHIEVED A SCORE OF</b>\n" + score;
        }

        ScoreText.text = "";
        LivesText.text = "";
    }

    //Called when the 'TRY AGAIN' button is pressed
    public void TryAgainButton()
    {
        GameOverScreen.SetActive(false);
        WinScreen.SetActive(false);
        BlockBreaker.Start();
    }

    public void AddScore()
    {
        score++;
        UpdateScoreAndLives();
    }

    public void RemoveLife()
    {
        lives--;
        UpdateScoreAndLives();
    }

    public void UpdateScoreAndLives()
    {
        ScoreText.text = "<b>SCORE</b>\n" + score;
        LivesText.text = "<b>LIVES</b>: " + lives;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // In the Unity editor, stop the play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // In a standalone build, quit the application
        Application.Quit();
#endif
    }
}
