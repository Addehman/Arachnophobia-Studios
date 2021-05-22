using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftSetTransformUp : MonoBehaviour
{
	[SerializeField] private Transform sourceOfTransform;
	[SerializeField] private float slerpTime = 10f;


	void FixedUpdate()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, sourceOfTransform.rotation, slerpTime * Time.deltaTime);
	}
}
