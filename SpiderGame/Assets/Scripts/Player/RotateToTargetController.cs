using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTargetController : MonoBehaviour
{
	[SerializeField] private Transform rotateToTargetObject;
	[SerializeField] private float rotationSlerpSpeed = 10f;
	
	private void Update()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTargetObject.rotation, rotationSlerpSpeed * Time.deltaTime);
	}
}