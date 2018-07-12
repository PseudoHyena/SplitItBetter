using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(GameManager))]
public class GameMenuManager : MonoBehaviour {

    [SerializeField] Animator menuAnim;

    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI lastScoreText;
    [SerializeField] TextMeshProUGUI maxText;
    [SerializeField] TextMeshProUGUI minText;
    [SerializeField] TMP_InputField XText;
    [SerializeField] TMP_InputField YText;

    GameManager gameManager;

    bool isMenuActive;

    private void Start() {
        gameManager = GetComponent<GameManager>();

        isMenuActive = false;

        SetScore(0, 0);
    }

    private void Update() {
        ToggleMenu();
    }

    public void BackToMenu() {
        SceneManager.LoadScene(0);
    }

    public void SetScore(int max, int min) {
        bestScoreText.text = ScoreManager.Score(gameManager.Size).ToString();
        lastScoreText.text = (max - min).ToString();
        SetMaxMin(max, min);
    } 

    public void SetMaxMin(int max, int min) {
        maxText.text = max.ToString();
        minText.text = min.ToString();
    }

    public void SetX(string sx) {
        int x;

        if (sx == string.Empty) {
            x = 0;
            return;
        }

        if (!int.TryParse(sx, out x)) {
            x = 3;
        }

        if (x > gameManager.MaxSize.x) {
            XText.text = gameManager.MaxSize.x.ToString();
            x = gameManager.MaxSize.x;
        }

        gameManager.Size = new Vector2Int(x, gameManager.Size.y);

        SetScore(0, 0);
    }

    public void SetY(string sy) {
        int y;

        if (sy == string.Empty) {
            y = 0;
            return;
        }

        if (!int.TryParse(sy, out y)) {
            y = 3;
        }


        if (y > gameManager.MaxSize.y) {
            YText.text = gameManager.MaxSize.x.ToString();
            y = gameManager.MaxSize.y;
        }

        gameManager.Size = new Vector2Int(gameManager.Size.x, y);

        SetScore(0, 0);
    }

    private void ToggleMenu() {
        if (Input.GetButtonDown("Cancel")) {
            isMenuActive = !isMenuActive;
            menuAnim.SetBool("Active", isMenuActive);
        }
    }
}
