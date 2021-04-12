using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WebPendulum
{
    public Transform spiderTransform;
    public WebTether tether;
    public WebArm arm;
    public WebSpider spider;

    Vector3 previousPosition;

    public void Initialise()
    {
        spiderTransform.transform.parent = tether.tetherTransform;
        arm.lenght = Vector3.Distance(spiderTransform.localPosition, tether.position);
    }

    public Vector3 MoveSpider(Vector3 currentPosition, float time)
    {
        spider.velocity += GetConstraintVelocity(currentPosition, previousPosition, time);
        
        spider.ApplyGravity();
        spider.ApplyDamping();
        spider.CapMaxSpeed();
        
        currentPosition += spider.velocity * time;

        if(Vector3.Distance(currentPosition, tether.position) < arm.lenght)
        {
            currentPosition = Vector3.Normalize(currentPosition - tether.position) * arm.lenght;
            arm.lenght = (Vector3.Distance(currentPosition, tether.position));
            return currentPosition;
        }

        previousPosition = currentPosition;

        return currentPosition;
    }

    public Vector3 MoveSpider(Vector3 currentPosition, Vector3 prevPos, float time)
    {
        spider.velocity += GetConstraintVelocity(currentPosition, prevPos, time);

        spider.ApplyGravity();
        spider.ApplyDamping();
        spider.CapMaxSpeed();

        currentPosition += spider.velocity * time;

        if (Vector3.Distance(currentPosition, tether.position) < arm.lenght)
        {
            currentPosition = Vector3.Normalize(currentPosition - tether.position) * arm.lenght;
            arm.lenght = (Vector3.Distance(currentPosition, tether.position));
            return currentPosition;
        }

        previousPosition = currentPosition;

        return currentPosition;
    }

    public Vector3 GetConstraintVelocity(Vector3 currentPosition, Vector3 previousPosition, float time)
    {
        float distanceToTether;
        Vector3 constrainedPosition;
        Vector3 predictedPosition;

        distanceToTether = Vector3.Distance(currentPosition, tether.position);

        if(distanceToTether > arm.lenght)
        {
            constrainedPosition = Vector3.Normalize(currentPosition - tether.position) * arm.lenght;
            predictedPosition = (constrainedPosition - previousPosition) / time;

            return predictedPosition;
        }

        return Vector3.zero;
    }

    public void SwitchTether(Vector3 newPosition)
    {
        spiderTransform.transform.parent = null;
        tether.tetherTransform.position = newPosition;
        spiderTransform.transform.parent = tether.tetherTransform;
        tether.position = tether.tetherTransform.InverseTransformPoint(newPosition);
        arm.lenght = Vector3.Distance(spiderTransform.transform.localPosition, tether.position);
    }

    public Vector3 Fall(Vector3 position, float time)
    {
        spider.ApplyGravity();
        spider.ApplyDamping();
        spider.CapMaxSpeed();

        position += spider.velocity * time;
        return position;
    }
}
