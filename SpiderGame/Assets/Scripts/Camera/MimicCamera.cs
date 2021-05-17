using Cinemachine;
using UnityEngine;

public class MimicCamera : MonoBehaviour
{
	[SerializeField] private Transform cameraToMimic;
	[SerializeField] private float positionLerpSpeed = 10f;
	[SerializeField] private float rotationLerpSpeed = 10f;

	private Vector3 currentVelocity;


	private void LateUpdate()
	{
		transform.position = Vector3.SmoothDamp(transform.position, cameraToMimic.position, ref currentVelocity, positionLerpSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, cameraToMimic.rotation, rotationLerpSpeed * Time.deltaTime);
	}
}
