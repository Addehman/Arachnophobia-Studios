using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNote : MonoBehaviour
{
    public GameObject stickyNote;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            stickyNote.SetActive(true);
        }

        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            stickyNote.SetActive(false);
        }
    }
}
