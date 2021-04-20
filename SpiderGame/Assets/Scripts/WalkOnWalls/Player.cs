using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 facing_direction = Vector3.forward;
    float detect_distance = 1.0f;
    float speed = 50.0f;

    void Update()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += transform.right;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.transform.up = Vector3.up;
            rigidbody.isKinematic = false;
        }

        if (movement.magnitude > 0.0f)
        {
            facing_direction = movement;
        }

        rigidbody.MovePosition(rigidbody.transform.position + movement.normalized * Time.deltaTime * speed);

        Debug.DrawLine(transform.position, transform.position + facing_direction * detect_distance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, facing_direction, out hit, detect_distance))
        {
            string name = hit.transform.gameObject.name;

            if (name == "wall")
            {
                float player_height = GetComponent<CapsuleCollider>().height;
                rigidbody.transform.position = hit.point + (hit.normal * player_height * 0.5f);
                rigidbody.transform.up = hit.normal;
                rigidbody.isKinematic = true;
            }
            else
            {
                rigidbody.transform.up = Vector3.up;
                rigidbody.isKinematic = false;
            }
        }
    }
}
