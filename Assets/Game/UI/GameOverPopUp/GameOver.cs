using Events;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPopUp,Stars;
    [SerializeField] private TextMeshProUGUI  gameOverText, finalScore;
    [SerializeField] private Star[] starsUI;
    [SerializeField] private GameObject playAgainButton, nextLevelButton;
    private const string gameOverLoose = "Game Over";
    private const string gameOverWon = "You Win";
    private void Awake()
    {
        EventManager.AddListener<int>(LevelFlowEvents.GameOverLoose, ShowGameOverPopUpLoose);
        EventManager.AddListener<int[]>(LevelFlowEvents.GameOverWon, ShowGameOverPopUpWon);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(LevelFlowEvents.GameOverLoose, ShowGameOverPopUpLoose);
        EventManager.RemoveListener<int[]>(LevelFlowEvents.GameOverWon, ShowGameOverPopUpWon);
    }
    private void ShowGameOverPopUpLoose(int currentScore)
    {
        EventManager.Dispatch(SongsEvents.PlaySFX, songs.LoseLevel);
        gameOverText.text = gameOverLoose;
        Stars.SetActive(false);
        gameOverPopUp.SetActive(true);
        playAgainButton.SetActive(true);
        finalScore.text = currentScore.ToString();
        showStars(0);
    }
    private void ShowGameOverPopUpWon(int[] currentScore)//vector int is a vector with de information for the finish level pos:0 current Score pos:1 stars achivement
    {
        EventManager.Dispatch(SongsEvents.PlaySFX, songs.WinLevel);
        gameOverText.text = gameOverWon;
        gameOverPopUp.SetActive(true);
        nextLevelButton.SetActive(true);
        finalScore.text = ""+ currentScore[0];
        showStars(currentScore[1]);
    }
    private void showStars(int stars)
    {
        Stars.SetActive(true);
        for (int i = 0; i < starsUI.Length; i++)
        {
            if (i < stars) starsUI[i].setStar(true);
            else starsUI[i].setStar(false);
        }

    }
}