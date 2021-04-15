using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private GameObject target;
	private Vector3 targetPosition;
	private Quaternion targetRotation;


	private void LateUpdate()
	{
		targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
		transform.position = targetPosition;

		/*targetRotation = target.transform.rotation;
		transform.rotation = targetRotation;*/
	}
}
