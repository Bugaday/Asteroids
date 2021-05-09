using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    Camera cam;
    Vector3 point;
    Vector2 mousePos;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        print(cam.ScreenToWorldPoint(Input.mousePosition).x);

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        mousePos = Input.mousePosition;

        Vector3 nearClip = cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane));
        Vector3 farClip = cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.farClipPlane));
        point = cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 200f));

        transform.position = new Vector3(nearClip.x,nearClip.y,0);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 100, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
    }
}
