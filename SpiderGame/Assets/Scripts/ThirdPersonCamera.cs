using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	private Transform follow;
	private Transform targetPosition;

	// Start is called before the first frame update
	private void Start()
	{
		follow = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		// targetPosition = follow.position
	}
}
