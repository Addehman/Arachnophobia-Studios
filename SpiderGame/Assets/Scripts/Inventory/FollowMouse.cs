using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Runs in lateupdate due to camera movement on mouseclick

public class FollowMouse : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}