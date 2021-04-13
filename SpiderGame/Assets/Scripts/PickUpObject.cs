using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    bool canPickUp = false;

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            print("Picked Up Object");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canPickUp = false;
        }
    }
}



