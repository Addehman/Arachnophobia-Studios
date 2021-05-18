using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNote : MonoBehaviour
{
    bool stickyNoteActive = true;
    public GameObject stickyNote;
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (stickyNoteActive == true)
            {
                stickyNote.SetActive(true);
                stickyNoteActive = false;
            }

            else
            {
                stickyNote.SetActive(false);
                stickyNoteActive = true;
            }
        }
        /*
                else if(Input.GetKeyUp(KeyCode.Tab))
                {
                    stickyNote.SetActive(false);
                }*/
    }
}
