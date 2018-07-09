using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class CamManipulation : MonoBehaviour {

    [SerializeField] Transform mainCam;
    [SerializeField] float scrollSpeed;

    GameManager gm;

    Vector2 lastMousePos = -Vector2.one;

    float scrollSmooth = 10f;
    float scrollScaleMax = 1.5f;

    private void Start() {
        gm = GetComponent<GameManager>();
    }

    private void Update() {
        if (mainCam == null) {
            return;
        }

        MoveCam();
        ScaleView();
    }

    private void MoveCam() {
        if (Input.GetMouseButton(2)) {
            if (lastMousePos == -Vector2.one) {
                lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dist = mousePos - lastMousePos;

            Vector2 topLeft = new Vector2(gm.Size.x / -2f, gm.Size.y / 2f);
            Vector2 bottomRight = new Vector2(-topLeft.x, -topLeft.y);

            Vector3 camPos = mainCam.position - new Vector3(dist.x, dist.y);

            mainCam.position = new Vector3(Mathf.Clamp(camPos.x, topLeft.x, bottomRight.x), 
                Mathf.Clamp(camPos.y, bottomRight.y, topLeft.y), camPos.z);

            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else {
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void ScaleView() {
        float newSize = Camera.main.orthographicSize - Input.mouseScrollDelta.y / scrollSmooth * scrollSpeed;

        Camera.main.orthographicSize = Mathf.Clamp(newSize, 1, gm.Size.y / scrollScaleMax);
    }
}
