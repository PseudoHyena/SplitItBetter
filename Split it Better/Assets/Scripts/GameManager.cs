﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [SerializeField] Vector2Int size;

    [SerializeField] [Range(0f, 1f)] float HSVSaturationMin;
    [SerializeField] [Range(0f, 1f)] float HSVSaturationMax;
    [SerializeField] [Range(0f, 1f)] float HSVValueMin;
    [SerializeField] [Range(0f, 1f)] float HSVValueMax;

    [SerializeField] GameObject squarePref;
    [SerializeField] Transform fieldHolder;

    SpriteRenderer[,] field;

    Vector2 mouseClickDownPos = -Vector2.one;
    Vector2 mouseClickUpPos = -Vector2.one;

    Queue<SpriteRenderer> rectsToMark = new Queue<SpriteRenderer>();

    List<SelectedRect> selectedRects = new List<SelectedRect>();

    public Vector2Int Size {
        get {
            return size;
        }
    }

    private void Start() {
        SetField();
    }

    private void Update() {
        Selection();
    }

    private void SetField() {
        if (squarePref == null) {
            Debug.Log("GameManager: Sprite prefab is mised");
            return;
        }

        if (fieldHolder == null) {
            Debug.Log("GameManager: Field holder is mised");
            return;
        }

        if (size.x < 3 || size.y < 3) {
            size.x = 3;
            size.y = 3;
        }

        field = new SpriteRenderer[size.x, size.y];

        float startPosX = size.x / -2f + 0.5f;
        float startPosY = size.y / 2f - 0.5f;

        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                GameObject go = Instantiate(squarePref, new Vector3(startPosX + x, startPosY - y),
                    Quaternion.identity, fieldHolder);
                go.name = $"Square[{x},{y}]";

                field[x, y] = go.GetComponent<SpriteRenderer>();
            }
        }
    }

    private void Selection() {
        if (field == null) {
            Debug.Log("GameManager: Field is null");
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            mouseClickDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && mouseClickDownPos != -Vector2.one) {
            mouseClickUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (mouseClickDownPos != -Vector2.one) {
            MarkSelected();
        }

        if (mouseClickDownPos != -Vector2.one && mouseClickUpPos != -Vector2.one) {
            SelectRects();
        }

        if (Input.GetMouseButtonDown(1)) {
            DeselectRects();
        }
    }

    private void SelectRects() {
        Vector2Int topLeft = new Vector2Int(Mathf.RoundToInt(mouseClickDownPos.x + size.x / 2f - 0.5f), 
            Mathf.RoundToInt(size.y / 2f - mouseClickDownPos.y - 0.5f));

        Vector2Int bottomRight = new Vector2Int(Mathf.RoundToInt(mouseClickUpPos.x + size.x / 2f - 0.5f),
            Mathf.RoundToInt(size.y / 2f - mouseClickUpPos.y - 0.5f));

        if (topLeft.x < 0 || topLeft.x >= size.x ||
            bottomRight.x < 0 || bottomRight.x >= size.x ||
            topLeft.y < 0 || topLeft.y >= size.y ||
            bottomRight.y < 0 || bottomRight.y >= size.y) {
            ClearMouseClickPos();

            return;
        }

        if (topLeft.x > bottomRight.x) {
            int temp = topLeft.x;
            topLeft.x = bottomRight.x;
            bottomRight.x = temp;
        }

        if (topLeft.y > bottomRight.y) {
            int temp = topLeft.y;
            topLeft.y = bottomRight.y;
            bottomRight.y = temp;
        }

        SelectedRect selectedRect = new SelectedRect(topLeft, bottomRight);

        foreach (var square in selectedRects) {
            if (square.AreRectsEqual(selectedRect) || square.Contains(selectedRect)) {
                ClearMouseClickPos();
                UnmarkSelected(topLeft, bottomRight);

                return;
            }
        }

        selectedRects.Add(selectedRect);

        DrawSelected(topLeft, bottomRight);

        ClearMouseClickPos();

        if (IsCompleted()) {
            Score();
        }
    }

    private void DeselectRects() {
        Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int pos = new Vector2Int(Mathf.RoundToInt(currentMousePos.x + size.x / 2f - 0.5f), 
            Mathf.RoundToInt(size.y / 2f - currentMousePos.y - 0.5f));

        for (int i = 0; i < selectedRects.Count; i++) {
            if (selectedRects[i].Contains(pos)) {
                SetRectsToDefault(selectedRects[i].TopLeft, selectedRects[i].BottomRight);

                selectedRects.RemoveAt(i);
                return;
            }
        }
    }

    private void MarkSelected() {
        while (rectsToMark.Count > 0) {
            SpriteRenderer sr = rectsToMark.Dequeue();

            Color color = sr.color;
            color.a = 1f;
            sr.color = color;
        }

        Vector2Int topLeft = new Vector2Int(Mathf.RoundToInt(mouseClickDownPos.x + size.x / 2f - 0.5f),
           Mathf.RoundToInt(size.y / 2f - mouseClickDownPos.y - 0.5f));

        Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int bottomRight = new Vector2Int(Mathf.RoundToInt(currentMousePos.x + size.x / 2f - 0.5f),
            Mathf.RoundToInt(size.y / 2f - currentMousePos.y - 0.5f));

        if (topLeft.x < 0 || topLeft.x >= size.x ||
            bottomRight.x < 0 || bottomRight.x >= size.x ||
            topLeft.y < 0 || topLeft.y >= size.y ||
            bottomRight.y < 0 || bottomRight.y >= size.y) {
            ClearMouseClickPos();

            return;
        }

        if (topLeft.x > bottomRight.x) {
            int temp = topLeft.x;
            topLeft.x = bottomRight.x;
            bottomRight.x = temp;
        }

        if (topLeft.y > bottomRight.y) {
            int temp = topLeft.y;
            topLeft.y = bottomRight.y;
            bottomRight.y = temp;
        }

        for (int x = topLeft.x; x <= bottomRight.x; x++) {
            for (int y = topLeft.y; y <= bottomRight.y; y++) {
                Color color = field[x, y].color;
                color.a = 0.5f;
                field[x, y].color = color;

                rectsToMark.Enqueue(field[x, y]);
            }
        }
    }

    private void UnmarkSelected(Vector2Int topLeft, Vector2Int bottomRight) {
        for (int x = topLeft.x; x <= bottomRight.x; x++) {
            for (int y = topLeft.y; y <= bottomRight.y; y++) {
                Color color = field[x, y].color;
                color.a = 1f;
                field[x, y].color = color;
            }
        }
    }

    private void SetRectsToDefault(Vector2Int topLeft, Vector2Int bottomRight) {
        for (int x = topLeft.x; x <= bottomRight.x; x++) {
            for (int y = topLeft.y; y <= bottomRight.y; y++) {
                field[x, y].color = Color.white;
            }
        }
    }

    private void DrawSelected(Vector2Int topLeft, Vector2Int bottomRight) {
        Color color = Random.ColorHSV(0f, 1f, HSVSaturationMin, HSVSaturationMax, HSVValueMin, HSVValueMax, 1f, 1f);

        for (int x = topLeft.x; x <= bottomRight.x; x++) {
            for (int y = topLeft.y; y <= bottomRight.y; y++) {
                field[x, y].color = color;
            }
        }
    }

    private void ClearMouseClickPos() {
        mouseClickDownPos = -Vector2.one;
        mouseClickUpPos = -Vector2.one;
    } 

    private bool IsCompleted() {
        if (selectedRects.Count < 2) {
            return false;
        }

        int fullSquare = size.x * size.y;
        int selectedSquare = 0;

        foreach (var selectedRect in selectedRects) {
            selectedSquare += selectedRect.Square;
        }

        return fullSquare == selectedSquare;
    }

    private void Score() {
        int max = selectedRects.Max((obj) => obj.Square);
        int min = selectedRects.Min((obj) => obj.Square);

        Debug.Log($"Score: {max - min}; Max: {max}; Min:{min}");

        ScoreManager.Add(size, max - min);
    }
}
