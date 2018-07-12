using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject scoreMenu;

    [SerializeField] Transform scoreContent;

    [SerializeField] GameObject scoreTextPrefab;
    [SerializeField] GameObject sizeTextPrefab;

    [SerializeField] Vector2 sizeTextStartPosition;
    [SerializeField] Vector2 scoreTextStartPosition;
    [SerializeField] Vector2 step;

    private void Start() {
        ScoreManager.ReadScore();
    }

    private void OnApplicationQuit() {
        ScoreManager.SaveScore();
    }

    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void Score() {
        startMenu.SetActive(false);
        scoreMenu.SetActive(true);

        GenerateScoreTable();
    }

    public void Controls() {
        startMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void BackToMenu() {
        startMenu.SetActive(true);
        controlsMenu.SetActive(false);
        scoreMenu.SetActive(false);
    }

	public void Quit() {
        Application.Quit();
    }

    private void GenerateScoreTable() {
        var fullScore = ScoreManager.FullScore;

        RectTransform rectTransform = scoreContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta += new Vector2(0, -step.y * (fullScore.Count));

        int i = 0;
        foreach (var item in fullScore) {         
            var scoreText = Instantiate(scoreTextPrefab, scoreContent).GetComponent<TextMeshProUGUI>();
            var sizeText = Instantiate(sizeTextPrefab, scoreContent).GetComponent<TextMeshProUGUI>();

            scoreText.text = $"{item.Value}";
            sizeText.text = $"{item.Key.x} x {item.Key.y}";

            scoreText.rectTransform.anchorMax = new Vector2(1f, 1f);
            scoreText.rectTransform.anchorMin = new Vector2(1f, 1f);

            sizeText.rectTransform.anchorMax = new Vector2(0f, 1f);
            sizeText.rectTransform.anchorMin = new Vector2(0f, 1f);
            
            scoreText.rectTransform.anchoredPosition = scoreTextStartPosition + step * i;
            sizeText.rectTransform.anchoredPosition = sizeTextStartPosition + step * i;

            i++;
        }
    }
}
