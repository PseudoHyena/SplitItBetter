using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ScoreManager {

    static SortedDictionary<ScoreRectSize, int> fullScore;

    static bool isSaved = true;

    public static void Add(Vector2Int size, int score) {
        if (fullScore == null || score <= 0) {
            return;
        }

        ScoreRectSize scoreRectSize = new ScoreRectSize(size);

        if (fullScore.ContainsKey(scoreRectSize)) {
            if (fullScore[scoreRectSize] > score) {
                fullScore[scoreRectSize] = score;
            }
        }
        else {
            fullScore.Add(scoreRectSize, score);
        }

        isSaved = false;
    }

    public static int Score(Vector2Int size) {
        if (fullScore == null) {
            return -1;
        }

        ScoreRectSize scoreRectSize = new ScoreRectSize(size);

        return fullScore.ContainsKey(scoreRectSize) ? fullScore[scoreRectSize] : -1;
    }

    static public void ReadScore() {
        if (fullScore != null) {
            return;
        }

        if (!File.Exists("Score.dat")) {
            fullScore = new SortedDictionary<ScoreRectSize, int>();
            
            return;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fs = new FileStream("Score.dat", FileMode.Open)) {
             fullScore = binaryFormatter.Deserialize(fs) as SortedDictionary<ScoreRectSize, int>;
        }
    }

    static public void SaveScore() {
        if (fullScore == null || fullScore.Count == 0) {
            isSaved = true;

            return;
        }

        if (isSaved == true) {
            return;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fs = new FileStream("Score.dat", FileMode.Create)) {
            binaryFormatter.Serialize(fs, fullScore);
        }

        isSaved = true;
    }
}
