using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    bool scrolling;
    float speed = 2f;

    Text header;
    Text creditList;
    private void Start()
    {
        header = GetComponent<Text>();
        creditList = GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        {
            if (!scrolling)
            {
                return;
            }

            header.transform.Translate(Vector3.up * Time.deltaTime * speed);
            creditList.transform.Translate(Vector3.up * Time.deltaTime * speed);

            if (gameObject.transform.position.y > .8)
            {
                scrolling = false;
            }
        }
    }
}
