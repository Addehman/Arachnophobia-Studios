using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private Transform cameraTarget;
	[SerializeField] private Transform targetToRotate;
	[SerializeField] private float mouseRotationSpeed = 10f;
	[SerializeField] private float defaultMouseRotationSpeed = 10f;
	[SerializeField] private float gamepadRotationSpeed = 15f;
	[SerializeField] private float defaultGamepadRotationSpeed = 15f;
	[SerializeField] private float smoothTime = 10f;
	[SerializeField] private float minZoom = 0.1f;
	[SerializeField] private float maxZoom = 0.5f;
	// [SerializeField] private TMP_InputField mosueSensiInputField;
	// [SerializeField] private TMP_InputField gamepadSensiInputField;
	[SerializeField] private Slider mouseSensiSlider;
	[SerializeField] private Slider gamepadSensiSlider;
	[SerializeField] private TMP_Text mouseInputValueText;
	[SerializeField] private TMP_Text gamepadInputValueText;
	
	private CinemachineVirtualCamera aimCamera;
	private CinemachineVirtualCamera cameraToZoom;
	private CinemachineComponentBase zoomCameraComponentBase;
	private CinemachineComponentBase aimCameraComponentBase;
	//private HookWeb hookWeb;
	//private ClimbWeb climbWeb;
	private SpiderMovement spiderMovement;
	private SpringJointWeb springJointWeb;
	private Transform cameraParent;
	private Vector3 currentVelocity;
	private float cameraInputX;
	private float cameraInputY;
	private float lerpSpeed = 10f;
	private float mouseSensiMod;
	private float gamepadSensiMod;
	private bool doLockCameraInput = false;
	private string mouseSensitivityModValue;
	private string gamepadSensitivityModValue;


	private void Awake()
	{
		cameraParent = transform.parent;
		spiderMovement = FindObjectOfType<SpiderMovement>();
		//hookWeb = FindObjectOfType<HookWeb>();
		//hookWeb.LockTPCameraRotation += LockCameraInput;
		//hookWeb.RecenterCamera += RecenterCamera;
		//climbWeb = FindObjectOfType<ClimbWeb>();
		//climbWeb.CameraStartRotation += BeginClimbRotation;
		//climbWeb.RecenterCamera += RecenterCamera;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.RecenterCamera += RecenterCamera;
		// springJointWeb.SwitchToSwingCamera += LockCameraInput;
	}
	
	private void Start()
	{
		InitializeSensitivitySettings();

		cameraToZoom = GetComponent<CinemachineVirtualCamera>();
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<CinemachineVirtualCamera>();

		zoomCameraComponentBase = cameraToZoom.GetCinemachineComponent(CinemachineCore.Stage.Body);
		if (zoomCameraComponentBase is CinemachineFramingTransposer)
		{
			(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = 0.3f;
		}

		aimCameraComponentBase = aimCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim);
	}

	private void InitializeSensitivitySettings()
	{
		mouseSensiMod = PlayerPrefs.GetFloat(mouseSensitivityModValue, 0f);
		gamepadSensiMod = PlayerPrefs.GetFloat(gamepadSensitivityModValue, 0f);
		mouseInputValueText.text = mouseSensiMod.ToString();
		gamepadInputValueText.text = gamepadSensiMod.ToString();
		mouseSensiSlider.value = mouseSensiMod;
		gamepadSensiSlider.value = gamepadSensiMod;
	}
	
	private void Update()
	{
		if (doLockCameraInput == false)
		{
			CamControl();
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			RecenterCamera();
		}
	}

	private void CamControl()
	{
		if (doLockCameraInput == false)
		{
			Vector3 gamepadInput = new Vector3(Input.GetAxis("RightStickX"), 0f, Input.GetAxis("RightStickY"));
			if (gamepadInput.sqrMagnitude > 0f)
			{
				cameraInputX += Input.GetAxis("CameraInputX") * (gamepadRotationSpeed + gamepadSensiMod) * 10f * Time.deltaTime;
				cameraInputY += Input.GetAxis("CameraInputY") * (gamepadRotationSpeed + gamepadSensiMod) * 10f * Time.deltaTime;
				if (aimCameraComponentBase is CinemachinePOV)
				{
					(aimCameraComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = false;
				}
			}
			else
			{
				cameraInputX += Input.GetAxisRaw("CameraInputX") * (mouseRotationSpeed + mouseSensiMod) * 10f * Time.deltaTime;
				cameraInputY -= Input.GetAxisRaw("CameraInputY") * (mouseRotationSpeed + mouseSensiMod) * 10f * Time.deltaTime;
				if (aimCameraComponentBase is CinemachinePOV)
				{
					(aimCameraComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = true;
				}
			}
			// cameraParent.localRotation = Quaternion.Euler(cameraInputY, cameraInputX, 0f);
			// transform.localRotation = Quaternion.identity;
			cameraInputY = Mathf.Clamp(cameraInputY, -10, 80);
			cameraTarget.localRotation = Quaternion.Euler(cameraInputY, cameraInputX, 0f);
			targetToRotate.localRotation = Quaternion.Euler(0f, cameraInputX, 0f);


			if (zoomCameraComponentBase is CinemachineFramingTransposer)
			{
				float cameraDistance = (zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Zoom");
				float zoomValue = Mathf.Clamp(cameraDistance, minZoom, maxZoom);
				
				(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = zoomValue;
				// (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset = 
			}
		}
		else
		{
			print ("setting cameraTarget Up");
			cameraTarget.parent.transform.up = Vector3.up;
		}
		
	}

	public void LockCameraInput(bool isActive)
	{
		// Lock CameraInput:
		doLockCameraInput = isActive;
	}

	public void RecenterCamera()
	{
		cameraInputX = 0f;
		cameraInputY = 0f;

		// transform.rotation = Quaternion.identity;
	}

	// public void MouseSensiInputFieldOnValueChanged()
	// {
	// 	mouseSensiMod = float.Parse(mosueSensiInputField.text);
	// 	mouseSensiSlider.value = mouseSensiMod;
	// 	gamepadRotationSpeed = defaultMouseRotationSpeed + mouseSensiMod;
	// 	SetFloatPlayerPrefs(mouseSensitivityModValue, mouseSensiMod);
	// }

	public void MouseSensiSliderOnValueChanged()
	{
		mouseSensiMod = mouseSensiSlider.value;
		mouseInputValueText.text = mouseSensiSlider.value.ToString();
		gamepadRotationSpeed = defaultMouseRotationSpeed + mouseSensiMod;
		SetFloatPlayerPrefs(mouseSensitivityModValue, mouseSensiMod);
	}

	// public void GamepadSensiInputFieldOnValueChanged()
	// {
	// 	gamepadSensiMod = float.Parse(gamepadSensiInputField.text);
	// 	gamepadSensiSlider.value = gamepadSensiMod;
	// 	gamepadRotationSpeed = defaultGamepadRotationSpeed + gamepadSensiMod;
	// 	SetFloatPlayerPrefs(gamepadSensitivityModValue, gamepadSensiMod);
	// }

	public void GamepadSensiSliderOnValueChanged()
	{
		gamepadSensiMod = gamepadSensiSlider.value;
		gamepadInputValueText.text = gamepadSensiSlider.value.ToString();
		gamepadRotationSpeed = defaultGamepadRotationSpeed + gamepadSensiMod;
		SetFloatPlayerPrefs(gamepadSensitivityModValue, gamepadSensiMod);
	}

	public void MouseResetSensiToDefault()
	{
		mouseSensiMod = 0f;
		mouseSensiSlider.value = 0f;
		SetFloatPlayerPrefs(mouseSensitivityModValue, mouseSensiMod);
	}

	public void GamepadResetSensiToDefault()
	{
		gamepadSensiMod = 0f;
		gamepadSensiSlider.value = 0f;
		SetFloatPlayerPrefs(gamepadSensitivityModValue, gamepadSensiMod);
	}

	private void SetFloatPlayerPrefs(string keyname, float value)
	{
		PlayerPrefs.SetFloat(keyname, value);
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetFloat(mouseSensitivityModValue, mouseSensiMod);
		PlayerPrefs.SetFloat(gamepadSensitivityModValue, gamepadSensiMod);
		//hookWeb.LockTPCameraRotation -= LockCameraInput;
		//climbWeb.CameraStartRotation -= BeginClimbRotation;
		//climbWeb.RecenterCamera -= RecenterCamera;
		springJointWeb.RecenterCamera -= RecenterCamera;
	}
}
