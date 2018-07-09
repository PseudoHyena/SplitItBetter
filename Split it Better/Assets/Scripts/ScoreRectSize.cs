using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreRectSize : IComparable {

    public ScoreRectSize(Vector2Int size) {
        x = size.x;
        y = size.y;
    }

    public int x { get; set; }
    public int y { get; set; }

    public int CompareTo(object obj) {
        ScoreRectSize scoreItem = obj as ScoreRectSize;

        if (x == scoreItem.x && y == scoreItem.y ||
            x == scoreItem.y && y == scoreItem.x) {
            return 0;
        }
        else if (x * y < scoreItem.x * scoreItem.y) {
            return -1;
        }
        else {
            return 1;
        }
    }
}
