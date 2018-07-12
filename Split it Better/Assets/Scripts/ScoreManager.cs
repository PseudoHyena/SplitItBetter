using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ScoreManager {

    static bool isSaved = true;

    static public SortedDictionary<ScoreRectSize, int> FullScore { get; private set; }

    public static void Add(Vector2Int size, int score) {
        if (FullScore == null || score <= 0) {
            return;
        }

        ScoreRectSize scoreRectSize = new ScoreRectSize(size);

        if (FullScore.ContainsKey(scoreRectSize)) {
            if (FullScore[scoreRectSize] > score) {
                FullScore[scoreRectSize] = score;
            }
        }
        else {
            FullScore.Add(scoreRectSize, score);
        }

        isSaved = false;
    }

    public static int Score(Vector2Int size) {
        if (FullScore == null) {
            return -1;
        }

        ScoreRectSize scoreRectSize = new ScoreRectSize(size);

        return FullScore.ContainsKey(scoreRectSize) ? FullScore[scoreRectSize] : -1;
    }

    static public void ReadScore() {
        if (FullScore != null) {
            return;
        }

        if (!File.Exists("Score.dat")) {
            FullScore = new SortedDictionary<ScoreRectSize, int>();
            
            return;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fs = new FileStream("Score.dat", FileMode.Open)) {
             FullScore = binaryFormatter.Deserialize(fs) as SortedDictionary<ScoreRectSize, int>;
        }
    }

    static public void SaveScore() {
        if (FullScore == null || FullScore.Count == 0) {
            isSaved = true;

            return;
        }

        if (isSaved == true) {
            return;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fs = new FileStream("Score.dat", FileMode.Create)) {
            binaryFormatter.Serialize(fs, FullScore);
        }

        isSaved = true;
    }
}
