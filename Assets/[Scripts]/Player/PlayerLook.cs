using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerLook : MonoBehaviour
{
    public Camera camera;

    private float xRotation = 0f;

    public float xSensitivity = 30f;

    public float ySensitivity = 30f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Look(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.fixedDeltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.fixedDeltaTime) * xSensitivity);
    }
}
