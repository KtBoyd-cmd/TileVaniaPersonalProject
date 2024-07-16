using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.2f;
    [SerializeField] AudioClip ExitSfx;

   
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag ==  "Player")
        {
        StartCoroutine(LoadNextLevel());
        }
        AudioSource.PlayClipAtPoint(ExitSfx, Camera.main.transform.position);

    }

    IEnumerator LoadNextLevel()
   {
    yield return new WaitForSecondsRealtime(levelLoadDelay);
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int nextSceneIndex = currentSceneIndex + 1;

    if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
    {
        nextSceneIndex = 0;
    }

    FindObjectOfType<ScenePersist>().ResetScenePersist();


    SceneManager.LoadScene(nextSceneIndex);
   }
}
