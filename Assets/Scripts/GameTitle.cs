using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using SFB;

public class GameTitle : MonoBehaviour {

	[SerializeField]
	static string musicPath;

	public static bool callToggle;

	public void StartTutorial() {

		SceneManager.LoadScene("TestInterSection");
	}

	public void StartMaingame() {

		SceneManager.LoadScene("MainInterSection");
	}

	public void QuitGame() {
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#elif UNITY_WEBPLAYER
		Application.OpenURL("http://www.google.com");
	#else
		Application.Quit();
	#endif
	}

	public void GoAudioImportMenu() {
		SceneManager.LoadScene("Settings");
	}

	public void GoMainMenu() {
		SceneManager.LoadScene("Menu");
	}

	public void OpenMusicImportDialog() {
		//パスの取得
    string[] paths = StandaloneFileBrowser.OpenFilePanel("Open Music", "", "", false);
		        musicPath = "";
        foreach (var p in paths) {
            musicPath += p;
        }
		  if (string.IsNullOrEmpty(musicPath)){

			}
		callToggle = true;
		}

	public static bool GetCallToggle() {
		return callToggle;
	}

	public static string GetMusicPath() {
		return musicPath;
	}

}
