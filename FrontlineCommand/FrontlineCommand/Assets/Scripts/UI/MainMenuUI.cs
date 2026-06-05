using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text highScoreLabel;

    void Start()
    {
        int best = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreLabel != null)
            highScoreLabel.text = best > 0 ? $"Best Score: {best}" : "";
    }

    public void OnPlayClicked() => SceneManager.LoadScene("Game");

    public void OnQuitClicked() => Application.Quit();
}
