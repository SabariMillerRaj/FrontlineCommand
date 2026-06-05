using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text finalScoreLabel;
    public TMP_Text wavesSurvivedLabel;
    public TMP_Text highScoreLabel;

    void Start()
    {
        int score = PlayerPrefs.GetInt("FinalScore", 0);
        int waves = PlayerPrefs.GetInt("WavesSurvived", 0);
        int best = PlayerPrefs.GetInt("HighScore", 0);

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("HighScore", best);
            PlayerPrefs.Save();
            if (highScoreLabel != null) highScoreLabel.text = "New High Score!";
        }
        else
        {
            if (highScoreLabel != null) highScoreLabel.text = $"Best: {best}";
        }

        if (finalScoreLabel != null) finalScoreLabel.text = $"Score: {score}";
        if (wavesSurvivedLabel != null) wavesSurvivedLabel.text = $"Waves Survived: {waves}";
    }

    public void OnRetryClicked()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("WavesSurvived");
        SceneManager.LoadScene("Game");
    }

    public void OnMenuClicked() => SceneManager.LoadScene("MainMenu");
}
