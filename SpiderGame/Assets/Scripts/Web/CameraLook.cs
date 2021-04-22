using UnityEngine;

public class CameraLook : MonoBehaviour 
{
	public string findPlayerName = "PlayerSpider";
	[Header("OBS!\nIf Player.name =='findPlayerName': Automagic!\nElse: Assign by dragging Player GameObject here")]
	public Transform lookAt;
	[Space(5f)]

	public float sensitivityX = 4f;
	public float sensitivityY = 4f;
	public float minAngleY = -70.0f;
	public float maxAngleY = 70.0f;
	public float distance = 0.2f;

	private float currentX = 0f;
	private float currentY = 0f;

	private void Start()
	{
		if (lookAt == null)
		{
			if (GameObject.Find(findPlayerName) != null)
			{
				lookAt = GameObject.Find(findPlayerName).gameObject.transform;
			}
			else
			{
				Debug.LogWarning("Can't find Player GameObject automagically, need to be assigned manually in inspector");
			}
		}

		Cursor.lockState = CursorLockMode.Locked;
	}

	private void LateUpdate()
	{
		currentX += Input.GetAxis("Mouse X") * sensitivityX;
		currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

		currentY = Mathf.Clamp(currentY, minAngleY, maxAngleY);

		Vector3 dir = new Vector3(0f, 0f, -distance);
		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);

		transform.position = lookAt.position + rotation * dir;
		transform.LookAt(lookAt.position);
	}
}
