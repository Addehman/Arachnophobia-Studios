using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStates : MonoBehaviour
{
    public GameObject howToMoveUI;
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

    State currentState = State.HowToMove;
    enum State
    {
        HowToMove,
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
    }

    void Update()
    {
        if (currentState == State.HowToMove)
        {
            howToMoveUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentState = State.HowToJump;
            }
        }

        else if (currentState == State.HowToJump)
        {
            howToMoveUI.SetActive(false);

            howToJumpUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return))
            {
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
                staminaInfoUI.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentState = State.HowToPickUp;
            }
        }

        else if (currentState == State.HowToPickUp)
        {
            apple.SetActive(true);
            howToSprintUI.SetActive(false);
            staminaInfoUI.SetActive(false);

            howToPickUp.SetActive(true);
            IntractButtonUI.SetActive(true);

            if (pickUpObject.numberOfItemsPickedUp == 1)
            {
                inventory.toggleInventory.SetActive(true);

                currentState = State.HowToInventory;
            }
        }

        else if (currentState == State.HowToInventory)
        {
            howToPickUp.SetActive(false);
            howToOpenInventoryGUI.SetActive(true);
            inventoryInfoGUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                inventory.toggleInventory.SetActive(true);
                stickyNote.stickyNote.SetActive(true);

                currentState = State.HowToStickyNote;
            }
        }

        else if (currentState == State.HowToStickyNote)
        {
            howToOpenInventoryGUI.SetActive(false);
            inventoryInfoGUI.SetActive(false);

            questGUI.SetActive(true);
            howToQuestGUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentState = State.HowToQuest;
            }
        }


        else if (currentState == State.HowToQuest)
        {
            howToQuestGUI.SetActive(false);

            questButton.SetActive(true);
            completeQuestGUI.SetActive(true);

            if (tutorialQuest.isFinished == true)
            {
                inventory.toggleInventory.SetActive(true);
                stickyNote.stickyNote.SetActive(true);

                checkInfoGUI.SetActive(true);
            }

            if (tutorialQuest.isFinished == true && Input.GetKeyDown(KeyCode.Return))
            {
                completeQuestGUI.SetActive(false);
                currentState = State.howToZoom;
            }
        }

        else if (currentState == State.howToZoom)
        {
            questButton.SetActive(false);
            checkInfoGUI.SetActive(false);

            howToZoomUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentState = State.howToToggleCamera;
            }
        }

        else if (currentState == State.howToToggleCamera)
        {
            howToZoomUI.SetActive(false);

            howToToggleCameraUI.SetActive(true);

            if (Input.GetMouseButtonUp(1))
            {
                currentState = State.HowToWeb;
            }
        }

        else if (currentState == State.HowToWeb)
        {
            howToToggleCameraUI.SetActive(false);

            howToWeb.SetActive(true);

            if (springJointWeb.isSwingingWeb)
            {
                continueTutorialUI.SetActive(true);
                currentState = State.TutorialCompleted;
            }
        }

        else if (currentState == State.TutorialCompleted)
        {
            howToWeb.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                continueTutorialUI.SetActive(false);
            }
        }
    }
}
