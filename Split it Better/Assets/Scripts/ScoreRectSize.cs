using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRectSize : IComparable {

    public ScoreRectSize(Vector2Int size) {
        Size = size;
    }

    public Vector2Int Size { get; set; }

    public int CompareTo(object obj) {
        ScoreRectSize scoreItem = obj as ScoreRectSize;

        if (Size.x == scoreItem.Size.x && Size.y == scoreItem.Size.y ||
            Size.x == scoreItem.Size.y && Size.y == scoreItem.Size.x) {
            return 0;
        }
        else if (Size.x * Size.y < scoreItem.Size.x * scoreItem.Size.y) {
            return -1;
        }
        else {
            return 1;
        }
    }
}
