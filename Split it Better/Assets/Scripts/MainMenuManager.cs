using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void Score() {

    }

	public void Quit() {
        Application.Quit();
    }
}
