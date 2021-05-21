using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsPositionRandomizer : MonoBehaviour
{
	[SerializeField] private bool doPositionRandomizer = false;
	[SerializeField] private Transform pickUpHolder;
	[SerializeField] private Transform[] randomPositions;
	[SerializeField] private Transform[] ingredients;
	[SerializeField] private List<Transform> occupiedPositions;


	private void Start()
	{
		FindChildrenAndFillArray();
		FindAndFillIngredientsArray();
		PositionRandomizer();
	}

	private void Update()
	{
		if (doPositionRandomizer == true)
		{
			PositionRandomizer();
			doPositionRandomizer = false;
		}
	}

	private void FindChildrenAndFillArray()
	{
		int childCount = 0;
		foreach(Transform child in transform)
		{
			childCount ++;
		}

		randomPositions = new Transform[childCount];

		for (int i = 0; i < childCount; i++)
		{
			randomPositions[i] = transform.GetChild(i);
		}
	}

	private void FindAndFillIngredientsArray()
	{
		if (GameObject.Find("PickUpHolder") != null)
		{
			pickUpHolder = GameObject.Find("PickUpHolder").transform;
		}
		else
		{
			Debug.LogError("Couldn't find 'PickUpHolder' automagically, please assign manually.");
		}

		int childCount = 0;
		foreach(Transform child in pickUpHolder)
		{
			childCount ++;
		}

		ingredients = new Transform[childCount];

		for (int i = 0; i < childCount; i++)
		{
			ingredients[i] = pickUpHolder.GetChild(i);
		}
	}

	public void PositionRandomizer()
	{
		occupiedPositions = new List<Transform>();

		foreach(Transform item in ingredients)
		{
			int randomNumber = Random.Range(0, 17);
			if (occupiedPositions.Count > 0)
			{
				for (int i = 0; i < randomPositions.Length; i++)
				{
					if (occupiedPositions.Contains(randomPositions[randomNumber]) == true)
					{
						randomNumber = Random.Range(0, 17);
					}
					else
					{
						item.position = randomPositions[randomNumber].position;
						occupiedPositions.Add(randomPositions[randomNumber]);
						break;
					}
				}
			}
			else
			{
				item.position = randomPositions[randomNumber].position;
				occupiedPositions.Add(randomPositions[randomNumber]);
			}
		}
	}
}
