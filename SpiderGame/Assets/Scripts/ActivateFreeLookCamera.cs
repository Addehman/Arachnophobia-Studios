using UnityEngine;

public class ActivateFreeLookCamera : MonoBehaviour
{
	public KeyCode ActivationKey = KeyCode.LeftAlt;
	public int PriorityBoostAmount = 10;

	Cinemachine.CinemachineVirtualCameraBase vcam;
	bool boosted = false;

	void Start()
	{
		vcam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
	}

	void Update()
	{
		if (vcam != null)
		{
			if (Input.GetKey(ActivationKey))
			{
				if (!boosted)
				{
					vcam.Priority += PriorityBoostAmount;
					boosted = true;
				}
			}
			else if (boosted)
			{
				vcam.Priority -= PriorityBoostAmount;
				boosted = false;
			}
		}
	}
}
