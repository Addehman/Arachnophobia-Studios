using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WebSpider
{
    public Vector3 velocity;
    public Vector3 gravityDirection = new Vector3(0, 1, 0);
    Vector3 dampingDirection;
    public float drag;
    public float gravity = 20f;
    public float maxSpeed;

    public void ApplyGravity()
    {
        velocity -= gravityDirection * gravity * Time.deltaTime;
    }
    //Slow down when swing
    public void ApplyDamping()
    {
        dampingDirection = -velocity;
        dampingDirection *= drag;
        velocity += dampingDirection;
    }

    public void CapMaxSpeed()
    {
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }
}
