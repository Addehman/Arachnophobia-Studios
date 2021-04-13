using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            print("Picked Up Object");
            Destroy(gameObject);
        } 
    }
}



