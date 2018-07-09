using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

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

    }

	public void Quit() {
        Application.Quit();
    }
}
