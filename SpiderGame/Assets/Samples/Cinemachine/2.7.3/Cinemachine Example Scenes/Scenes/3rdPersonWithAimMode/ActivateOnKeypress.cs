using UnityEngine;
using System;

public class ActivateOnKeypress : MonoBehaviour
{
	public event Action<bool> ActivationFPSCam;

	public KeyCode ActivationKey = KeyCode.LeftControl;
	public int PriorityBoostAmount = 10;
	public GameObject Reticle;

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
					if (ActivationFPSCam != null)
					{
						ActivationFPSCam(true);
					}
				}
			}
			else if (boosted)
			{
				vcam.Priority -= PriorityBoostAmount;
				boosted = false;
				if (ActivationFPSCam != null)
					{
						ActivationFPSCam(false);
					}
			}
		}
		if (Reticle != null)
			Reticle.SetActive(boosted);
	}
}
