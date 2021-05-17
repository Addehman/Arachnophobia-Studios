using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
    [SerializeField] Transform debugHitPointTransform;
    public GameObject webStartPosition;
    SpiderMovement spiderMovement;
    State currentState;

    Vector3 hookShotPosition;

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
        //   spiderMovement.gravityValue = -0f;

        transform.position = Vector3.Lerp(currentPosition, hookShotPosition, 0.05f);

        if (Vector3.Distance(transform.position, hookShotPosition) < 0.05f)
        {
            //    spiderMovement.gravityValue = -9.82f;
            currentState = State.Normal;
        }
    }
}
