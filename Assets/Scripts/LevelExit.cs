using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1F;
   void LoadNextScene(int currentSceneIndex)  
    {
        SceneManager.LoadScene(currentSceneIndex+1);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            StartCoroutine(LoadNextScene());
        }
        
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex+1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
    }
     
}
