using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStates : MonoBehaviour
{
	public GameObject howToMoveUI;
	public GameObject howToLookUI;
	public GameObject howToJumpUI;
	public GameObject howToWallWalkingUI;
	public GameObject howToSprintUI;
	public GameObject staminaUI;
	public GameObject staminaInfoUI;
	public GameObject howToPickUp;
	public GameObject IntractButtonUI;
	public GameObject apple;
	public GameObject howToOpenInventoryGUI;
	public GameObject inventoryInfoGUI;
	public GameObject howToQuestGUI;
	public GameObject questGUI;
	public GameObject questButton;
	public GameObject completeQuestGUI;
	public GameObject howToQuestHint;
	public GameObject checkInfoGUI;
	public GameObject howToZoomUI;
	public GameObject howToToggleCameraUI;
	public GameObject howToWeb;
	public GameObject continueTutorialUI;

	public TutorialQuest tutorialQuest;
	public PickUpObject pickUpObject;
	public Inventory inventory;
	public StickyNote stickyNote;
	public ToggleCameras toggleCameras;
	public SpringJointWeb springJointWeb;

	bool pressedContinue = false;
	bool hasHowToStickNoteStarted = false;
	bool hasFinishedTutorialQuest = false;

	State currentState = State.HowToMove;
	enum State
	{
		HowToMove,
		HowToLook,
		HowToJump,
		HowToWalkWall,
		HowToSprint,
		HowToPickUp,
		HowToInventory,
		HowToStickyNote,
		HowToQuest,
		howToZoom,
		howToToggleCamera,
		HowToWeb,
		TutorialCompleted
	}


	void Start()
	{
		currentState = State.HowToMove;
		hasHowToStickNoteStarted = false;
		hasFinishedTutorialQuest = false;
	}

	void Update()
	{
		if (currentState == State.HowToMove)
		{
			howToMoveUI.SetActive(true);

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				currentState = State.HowToLook;
			}
		}

		else if (currentState == State.HowToLook)
		{
			howToMoveUI.SetActive(false);

			howToLookUI.SetActive(true);

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				currentState = State.HowToJump;
			}
		}

		else if (currentState == State.HowToJump)
		{
			howToLookUI.SetActive(false);

			howToJumpUI.SetActive(true);

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				currentState = State.HowToSprint;
			}
		}

	/*    else if (currentState == State.HowToWalkWall)
		{
			howToJumpUI.SetActive(false);

			howToWallWalkingUI.SetActive(true);

			if (Input.GetKeyDown(KeyCode.Return))
			{
				currentState = State.HowToSprint;
			}
		}*/

		else if (currentState == State.HowToSprint)
		{
			//   howToWallWalkingUI.SetActive(false);
			howToJumpUI.SetActive(false);

			howToSprintUI.SetActive(true);
			staminaUI.SetActive(true);

			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				ClickingSound.clickSound();
				staminaInfoUI.SetActive(true);
			}

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				currentState = State.HowToPickUp;
			}
		}

		else if (currentState == State.HowToPickUp)
		{
			if (apple != null)
			{
				apple.SetActive(true);
			}

			howToSprintUI.SetActive(false);
			staminaInfoUI.SetActive(false);

			howToPickUp.SetActive(true);
			IntractButtonUI.SetActive(true);

			if (pickUpObject.numberOfItemsPickedUp == 1)
			{
				ClickingSound.clickSound();
				inventory.toggleInventory.SetActive(true);

				currentState = State.HowToInventory;
			}
		}

		else if (currentState == State.HowToInventory)
		{
			howToPickUp.SetActive(false);
			howToOpenInventoryGUI.SetActive(true);
			inventoryInfoGUI.SetActive(true);
			if (inventory.inventoryIsActive == true)
			{
				inventory.EnableInventory();
			}
			else
			{
				inventory.DisableInventory();
			}

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				inventory.toggleInventory.SetActive(true);
				stickyNote.stickyNote.SetActive(true);

				currentState = State.HowToStickyNote;
			}
		}

		else if (currentState == State.HowToStickyNote)
		{
			howToOpenInventoryGUI.SetActive(false);
			questGUI.SetActive(true);

			if (hasHowToStickNoteStarted == false)
			{
				inventoryInfoGUI.SetActive(false);
				howToQuestGUI.SetActive(true);

				hasHowToStickNoteStarted = true;
			}

			if (Input.GetButtonDown("Cancel"))
			{
				howToQuestGUI.SetActive(false);

				ClickingSound.clickSound();
				// currentState = State.HowToQuest;
				currentState = State.howToZoom;
			}
		}

		else if (currentState == State.howToZoom)
		{
			howToZoomUI.SetActive(true);

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				currentState = State.howToToggleCamera;
			}
		}

		else if (currentState == State.howToToggleCamera)
		{
			howToZoomUI.SetActive(false);

			howToToggleCameraUI.SetActive(true);

			if (Input.GetButtonDown("Aim"))
			{
				ClickingSound.clickSound();
				currentState = State.HowToWeb;
			}
		}

		else if (currentState == State.HowToWeb)
		{
			howToToggleCameraUI.SetActive(false);

			howToWeb.SetActive(true);

			if (springJointWeb.isSwingingWeb)
			{
				howToWeb.SetActive(false);

				ClickingSound.clickSound();
				
				// currentState = State.TutorialCompleted;
				currentState = State.HowToQuest;
			}
		}

		else if (currentState == State.HowToQuest)
		{

			questButton.SetActive(true);
			completeQuestGUI.SetActive(true);
			howToQuestHint.SetActive(true);

			if (tutorialQuest.isFinished == true && hasFinishedTutorialQuest == false)
			{
				ClickingSound.clickSound();
				inventory.toggleInventory.SetActive(true);
				stickyNote.stickyNote.SetActive(true);

				checkInfoGUI.SetActive(true);
				howToQuestHint.SetActive(false);


				hasFinishedTutorialQuest = true;
			}

			if (tutorialQuest.isFinished == true && Input.GetButtonDown("Cancel"))
			{
				questButton.SetActive(false);
				checkInfoGUI.SetActive(false);
				continueTutorialUI.SetActive(true);

				ClickingSound.clickSound();
				completeQuestGUI.SetActive(false);
				currentState = State.TutorialCompleted;
				howToQuestHint.SetActive(false);
			}
		}

		else if (currentState == State.TutorialCompleted)
		{

			if (Input.GetButtonDown("Cancel"))
			{
				ClickingSound.clickSound();
				continueTutorialUI.SetActive(false);
			}
		}
	}
}
