using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
    [SerializeField] Transform debugHitPointTransform;
    public GameObject webStartPosition;
    SpiderMovement spiderMovement;
    State currentState;

    public float speed = 0.005f;

    Vector3 hookShotPosition;
    Vector3 newTransformUp;
    Vector3 previousTransformUp;

    enum State
    {
        Normal,
        HookFlying,
    }
    void Start()
    {
        spiderMovement = GetComponent<SpiderMovement>();
    }

    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, hookShotPosition));
        if (currentState == State.Normal)
        {
            HandleHookShotStart();
        }

        if (currentState == State.HookFlying)
        {
            HandleHookShotMovement();
        }
    }

    void HandleHookShotStart()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
            {
                previousTransformUp = transform.up;
                newTransformUp = raycastHit.normal;

                debugHitPointTransform.position = raycastHit.point;
                hookShotPosition = raycastHit.point;
                currentState = State.HookFlying;
            }
        }
    }

    void HandleHookShotMovement()
    {
        //   Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;
        Vector3 currentPosition = transform.position;
        float hookShotSpeed = Vector3.Distance(currentPosition, hookShotPosition);

        spiderMovement.gravityValue = 0f;

        transform.position = Vector3.Lerp(currentPosition, hookShotPosition, speed);
        transform.up = Vector3.Lerp(previousTransformUp, newTransformUp, speed);

        if (Vector3.Distance(transform.position, hookShotPosition) < 0.05f)
        {
            spiderMovement.gravityValue = -9.82f;
            currentState = State.Normal;
        }
    }
}
