using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public SoundManager soundManager;
    SpiderAudio spiderAudio;
    public Animator spiderAnimator;
    public GameOver gameOver;
    public Quest quest;
    float currentHealth;
    float maxHealth = 100f;
    private PickUpObject pickUpObject;
    public Image healthBar;
    public float burnTimer;

    private void Start()
    {
        spiderAudio = GetComponent<SpiderAudio>();
/*        pickUpObject = GetComponent<PickUpObject>();
        pickUpObject.pickedUpItem += PickUpObject_pickedUpItem;*/
        currentHealth = maxHealth;
    }

    public void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

/*    private void PickUpObject_pickedUpItem()
    {
        GoDoQuest();
    }*/

/*    public void GoDoQuest() //This function hold the quest that is needed to be done right now. At the moment, fruitcollected.
    {
        if (quest.isAccepted)
        {
            quest.finished.GatherFood();
            if (quest.finished.IsReached())
            {
                quest.Complete();
            }
        }
    }*/

    private void OnTriggerStay (Collider other)
    {
        if (other.gameObject.name == "HotHob")
        {
            burnTimer += Time.deltaTime;

            if (burnTimer > 2f && currentHealth > 0f)
            {
                burnTimer = 0f;
                currentHealth -= 10f;

                spiderAudio.Burn();
            }

            if(currentHealth <= 0f)
            {
                spiderAnimator.SetBool("Dead", true);
                gameOver.GameOverScreen();
            }
        }
    }
}
