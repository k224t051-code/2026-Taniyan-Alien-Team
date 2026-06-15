using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleClearSequence : MonoBehaviour
{
    public string nextSceneName;
    public float delaySeconds = 1f;

    private bool isRunning = false;

    public void StartClear()
    {
        if (isRunning)
        {
            return;
        }

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("Next scene name is not set.");
            return;
        }

        StartCoroutine(ClearRoutine());
    }

    private IEnumerator ClearRoutine()
    {
        isRunning = true;
        Debug.Log("Clear!");
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(nextSceneName);
    }
}