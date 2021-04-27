using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Quest quest;
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    private PickUpObject pickUpObject;
    public Image healthBar;
    public float burnTimer;

    private void Start()
    {
        pickUpObject = GetComponent<PickUpObject>();
        pickUpObject.pickedUpItem += PickUpObject_pickedUpItem;
        currentHealth = maxHealth;
    }

    public void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void PickUpObject_pickedUpItem()
    {
        GoDoQuest();
    }

    public void GoDoQuest() //This function hold the quest that is needed to be done right now. At the moment, fruitcollected.
    {
        if (quest.isAccepted)
        {
            quest.finished.GatherFood();
            if (quest.finished.IsReached())
            {
                quest.Complete();
            }
        }
    }
    private void OnTriggerStay (Collider other)
    {
        if (other.gameObject.name == "Trigger")
        {
            Debug.Log("Hot!");
            burnTimer += Time.deltaTime;

            if (burnTimer > 2f)
            {
                burnTimer = 0f;
                currentHealth -= 10f;
                Debug.Log(currentHealth);
            }
        }
    }
}
