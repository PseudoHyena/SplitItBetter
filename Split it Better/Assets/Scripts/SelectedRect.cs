using UnityEngine;

public class SelectedRect {

    public SelectedRect(Vector2Int topLeft, Vector2Int bottomRight) {
        TopLeft = topLeft;
        BottomRight = bottomRight;

        ComputeSquare();
    }

    public Vector2Int TopLeft { get; set; }
    public Vector2Int BottomRight { get; set; }
    public int Square { get; set; }

    public bool Contains(Vector2Int point) {
        return (point.x >= TopLeft.x) && (point.y >= TopLeft.y) &&
            (point.x <= BottomRight.x) && (point.y <= BottomRight.y);
    }

    public bool Contains(SelectedRect selectedRect) {
        return (selectedRect.TopLeft.y <= BottomRight.y && selectedRect.TopLeft.x <= BottomRight.x) &&
            (selectedRect.BottomRight.y >= TopLeft.y && selectedRect.BottomRight.x >= TopLeft.x);
    }

    public bool AreRectsEqual(SelectedRect selectedRect) {
        return ((BottomRight.x - TopLeft.x) == (selectedRect.BottomRight.x - selectedRect.TopLeft.x) &&
            (BottomRight.y - TopLeft.y) == (selectedRect.BottomRight.y - selectedRect.TopLeft.y)) ||
            ((BottomRight.x - TopLeft.x) == (selectedRect.BottomRight.y - selectedRect.TopLeft.y) &&
            (BottomRight.y - TopLeft.y) == (selectedRect.BottomRight.x - selectedRect.TopLeft.x));
    }

    private void ComputeSquare() {
        Square = (BottomRight.x - TopLeft.x + 1) * (BottomRight.y - TopLeft.y + 1);
    }
}
