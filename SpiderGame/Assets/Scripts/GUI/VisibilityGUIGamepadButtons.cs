using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityGUIGamepadButtons : MonoBehaviour
{
	[SerializeField] private GameObject gamepadHint;

	private bool doOnceGamepad = false;
	private bool doOnceMouse = false;


	private void Update()
	{
		Vector3 gamepadInput = new Vector3(Input.GetAxis("LeftStickX"), 0f, Input.GetAxis("LeftStickY"));
		Vector3 mouseInput = new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));
		if ((gamepadInput.sqrMagnitude > 0f || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("joystick button 1")) && doOnceGamepad == false)
		{
			gamepadHint.SetActive(true);
			doOnceMouse = false;
			doOnceGamepad = true;
		}
		else if ((mouseInput.sqrMagnitude > 0f || Input.anyKeyDown) && doOnceMouse == false)
		{
			gamepadHint.SetActive(false);
			doOnceGamepad = false;
			doOnceMouse = true;
		}
	}
}
