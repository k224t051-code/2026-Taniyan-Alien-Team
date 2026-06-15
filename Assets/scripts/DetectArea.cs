using UnityEngine;
using System.Collections.Generic;

public class DetectArea : MonoBehaviour
{
    public List<GameObject> insideObjects = new List<GameObject>();

    public bool IsCorrect(GameObject[] correctAnswers)
    {
        if (correctAnswers == null || insideObjects == null)
        {
            return false;
        }

        if (correctAnswers.Length != insideObjects.Count)
        {
            return false;
        }

        foreach (GameObject obj in correctAnswers)
        {
            if (!insideObjects.Contains(obj))
            {
                return false;
            }
        }

        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!insideObjects.Contains(other.gameObject))
        {
            insideObjects.Add(other.gameObject);
        }
    }

   void OnTriggerExit(Collider other)
    {
        if (insideObjects.Contains(other.gameObject))
        {
            insideObjects.Remove(other.gameObject);
        }
    }
}
