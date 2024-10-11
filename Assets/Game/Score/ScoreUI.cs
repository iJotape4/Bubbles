using System.Linq;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;

    // TODO This is placing all scores on the UI from the START, if we only need certaing amount of scores we need to change that.
    // Change this into a method
    void Start()
    {
        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.rank.text = (i + 1).ToString();
            row.nameRow.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}