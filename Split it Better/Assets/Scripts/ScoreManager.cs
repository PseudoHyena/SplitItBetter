using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager {

    static SortedDictionary<ScoreRectSize, int> fullScore = new SortedDictionary<ScoreRectSize, int>();

    public static void Add(Vector2Int size, int score) {
        ScoreRectSize scoreRectSize = new ScoreRectSize(size);

        if (fullScore.ContainsKey(scoreRectSize)) {
            if (fullScore[scoreRectSize] > score) {
                fullScore[scoreRectSize] = score;
            }
        }
        else {
            fullScore.Add(scoreRectSize, score);
        }
    }
}
