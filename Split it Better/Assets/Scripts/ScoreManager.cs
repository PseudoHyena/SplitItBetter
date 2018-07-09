using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ScoreManager {

    static SortedDictionary<ScoreRectSize, int> fullScore = new SortedDictionary<ScoreRectSize, int>();

    static bool isSaved = true;

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

        isSaved = false;
    }

    static public void ReadScore() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fs = new FileStream("Score.dat", FileMode.Open)) {
             fullScore = binaryFormatter.Deserialize(fs) as SortedDictionary<ScoreRectSize, int>;
        }
    }

    static public void SaveScore() {
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
