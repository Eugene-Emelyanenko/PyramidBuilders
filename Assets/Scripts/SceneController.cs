using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BuildNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Game");
    }
    
    public void ReturnToLastGame()
    {
        SceneManager.LoadScene("Game");
    }
}
