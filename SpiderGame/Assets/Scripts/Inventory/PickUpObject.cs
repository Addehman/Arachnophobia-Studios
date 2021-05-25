using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Makes it possible to pick up and check the object via the Inventory

public class PickUpObject : MonoBehaviour
{
	public bool isAllItemsCollected = false;

	public SpiderAudio spiderAudio;
	public GameObject tomatoCheck;
	public GameObject bananaCheck;
	public GameObject blueberryCheck;
	public GameObject appleCheck;
	public GameObject carrotCheck;
	public GameObject cheeseCheck;
	public GameObject tBoneCheck;
	public GameObject cookieCheck;
	public GameObject chickenBone;
	public GameObject helpText;

	public int itemID;
	[SerializeField]
	public int numberOfItemsPickedUp;

	private Inventory inventoryOnPlayer;
	private GameObject thisObjectThatWeStandOn;
	[SerializeField]
	private bool canPickUp;

	public event Action pickedUpItem;

	private void Start()
	{
		inventoryOnPlayer = FindObjectOfType<Inventory>();
		canPickUp = false;
	}
	private void Update()
	{
		if (Input.GetButtonDown("Interact") && canPickUp == true)
		{
			numberOfItemsPickedUp++;
			Debug.Log("number of items picked up: " + numberOfItemsPickedUp);
			helpText.SetActive(false);
			spiderAudio.PickUpSound();

			inventoryOnPlayer.GiveItem(itemID);

			if (pickedUpItem != null)
			{
				pickedUpItem(); 
			}

			if (thisObjectThatWeStandOn != null)
			{
				Destroy(thisObjectThatWeStandOn);
				thisObjectThatWeStandOn = null;
			}

			canPickUp = false;

			CheckIgredients();
		}

		if(numberOfItemsPickedUp >= 9)
		{
			isAllItemsCollected = true;
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("PickUpAble"))
		{
			helpText.SetActive(true);
			canPickUp = true;
			thisObjectThatWeStandOn = collider.gameObject;
			itemID = thisObjectThatWeStandOn.GetComponent<ItemID>().itemID;
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.CompareTag("PickUpAble")) // Player
		{
			helpText.SetActive(false);
			thisObjectThatWeStandOn = null;
			canPickUp = false;
		}
	}

	void CheckIgredients()
    {
		if (itemID == 0)
		{
			tomatoCheck.SetActive(true);
		}

		else if (itemID == 1)
        {
			bananaCheck.SetActive(true);
		}

		else if (itemID == 2)
		{
			blueberryCheck.SetActive(true);
		}

		else if (itemID == 3)
		{
			appleCheck.SetActive(true);
		}

		else if (itemID == 4)
		{
			carrotCheck.SetActive(true);
		}

		else if (itemID == 6)
		{
			cheeseCheck.SetActive(true);
		}

		else if (itemID == 7)
		{
			tBoneCheck.SetActive(true);
		}

		else if (itemID == 8)
		{
			chickenBone.SetActive(true);
		}

		else if (itemID == 9)
		{
			cookieCheck.SetActive(true);
		}
	}
}

