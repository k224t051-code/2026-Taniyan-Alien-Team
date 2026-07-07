using UnityEngine;

public class LeverPuzzleController : MonoBehaviour
{
    public DetectArea area;
    public GameObject[] correctAnswers;
    public PuzzleClearSequence clearSequence;

    private bool cleared = false;

    public bool TryClear()
    {
        if (cleared)
        {
            Debug.Log("LeverPuzzleController: already cleared.");
            return false;
        }

        if (area == null)
        {
            Debug.LogError("LeverPuzzleController: DetectArea is not assigned.");
            return false;
        }

        bool isCorrect = area.IsCorrect(correctAnswers);
        Debug.Log($"LeverPuzzleController: IsCorrect={isCorrect}, correctAnswers={correctAnswers?.Length ?? 0}, insideObjects={area.insideObjects?.Count ?? 0}");

        if (!isCorrect)
        {
            return false;
        }

        if (clearSequence == null)
        {
            Debug.LogError("LeverPuzzleController: Clear sequence is not assigned.");
            return false;
        }

        cleared = true;
        clearSequence.StartClear();
        Debug.Log("LeverPuzzleController: clear sequence started.");
        return true;
    }
}