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
            return false;
        }

        if (area == null)
        {
            return false;
        }

        if (!area.IsCorrect(correctAnswers))
        {
            return false;
        }

        if (clearSequence == null)
        {
            Debug.LogWarning("Clear sequence is not set.");
            return false;
        }

        cleared = true;
        clearSequence.StartClear();
        return true;
    }
}