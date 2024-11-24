using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public float loadingTime = 1.5f;
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(loadingTime);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync("Menu");
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
