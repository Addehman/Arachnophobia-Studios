using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {

    public Transform lookAt;
    public Transform camTransform;

    public float sensitivityX = 4f;
    public float sensitivityY = 4f;
    public float minAngleY = -70.0f;
    public float maxAngleY = 70.0f;
    private float distance = 0.2f;

    float currentX = 0f;
    float currentY = 0f;

    private void Start()
    {
        camTransform = transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

        currentY = Mathf.Clamp(currentY, minAngleY, maxAngleY);

        Vector3 dir = new Vector3(0f, 0f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);

        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
