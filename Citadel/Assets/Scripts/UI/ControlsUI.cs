﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VHS;

public class ControlsUI : MonoBehaviour
{
    public ButtonParent forwardButton;
    public ButtonParent leftButton;
    public ButtonParent backwardsButton;
    public ButtonParent rightButton;
    public ButtonParent crouchButton;
    public ButtonParent sprintButton;
    public ButtonParent jumpButton;
    public ButtonParent useButton;
    public ButtonParent reloadButton;
    public ButtonParent shootButton;
    public ButtonParent quickCycleButton;
    public ButtonParent mapButton;
    public ButtonParent smoothingButton;
    public Slider sensitivitySlider;

    [HideInInspector]
    public static ControlsUI instance; //Singleton

    //[HideInInspector]
    public string newInput;
    [HideInInspector]
    public ButtonParent currentInput;
    [HideInInspector]
    public List<ButtonParent> uiElements;
    //[HideInInspector]
    public bool currentlyWaiting;
    private Coroutine waitingForInputCoroutine;
    private bool skipWait = false;

    private KeyCode[] keyCodes;

    /// <summary>
    /// Define Singleton
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

        uiElements = new List<ButtonParent>();
        uiElements.Add(forwardButton);
        uiElements.Add(leftButton);
        uiElements.Add(backwardsButton);
        uiElements.Add(rightButton);
        uiElements.Add(crouchButton);
        uiElements.Add(sprintButton);
        uiElements.Add(jumpButton);
        uiElements.Add(useButton);
        uiElements.Add(reloadButton);
        uiElements.Add(shootButton);
        uiElements.Add(quickCycleButton);
        uiElements.Add(mapButton);
        uiElements.Add(smoothingButton);

        for (int i = 0; i < uiElements.Count; i++)
        {
            uiElements[i].GetComponent<Button>().onClick.AddListener(UpdateInput);
        }

        smoothingButton.GetComponent<Button>().onClick.AddListener(SkipWait);

        yield return new WaitUntil(() => (GameVars.instance && GameVars.instance.saveManager.hasReadData));
        UpdateButtonNames();
    }

    void Update()
    {
        if (currentlyWaiting)
        {
            if (!skipWait)
            {
                if (Input.inputString != null)
                {
                    foreach (KeyCode keyCode in keyCodes)
                    {
                        if (Input.GetKey(keyCode))
                        {
                            newInput = keyCode.ToString();
                            currentlyWaiting = false;
                            GotInput();
                        }
                    }
                }
            }
            else
            {
                skipWait = false;
                currentlyWaiting = false;
                GotInput();
            }
        }
    }

    public void UpdateInput()
    {
        if (!currentlyWaiting)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            for (int i = 0; i < uiElements.Count; i++)
            {
                if (uiElements[i].isCurrentlyActive)
                {
                    currentInput = uiElements[i];
                }
            }

            currentlyWaiting = true;
        }
    }

    public void SkipWait()
    {
        skipWait = true;
    }

    public void UpdateButtonNames()
    {
        forwardButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_FORWARD.ToUpper();
        leftButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_LEFT.ToUpper();
        backwardsButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_BACKWARDS.ToUpper();
        rightButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_RIGHT.ToUpper();
        crouchButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_CROUCH.ToUpper();
        sprintButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_SPRINT.ToUpper();
        jumpButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_JUMP.ToUpper();
        useButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_USE.ToUpper();
        reloadButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_RELOAD.ToUpper();
        shootButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_SHOOT.ToUpper();
        quickCycleButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_QUICKCYCLE.ToUpper();
        mapButton.transform.GetChild(0).GetComponent<Text>().text = GameVars.instance.saveManager.INPUT_MAP.ToUpper();
        if(GameVars.instance.saveManager.SMOOTHING == true)
        {
            smoothingButton.transform.GetChild(0).GetComponent<Text>().text = "ENABLED";
        }
        else
        {
            smoothingButton.transform.GetChild(0).GetComponent<Text>().text = "DISABLED";
        }
    }

    public void GotInput()
    {      
        if (!currentlyWaiting)
        {
            if (newInput != "Escape")
            {
                if (currentInput == forwardButton)
                {
                    GameVars.instance.saveManager.INPUT_FORWARD = newInput;
                }
                else if (currentInput == leftButton)
                {
                    GameVars.instance.saveManager.INPUT_LEFT = newInput;
                }
                else if (currentInput == backwardsButton)
                {
                    GameVars.instance.saveManager.INPUT_BACKWARDS = newInput;
                }
                else if (currentInput == rightButton)
                {
                    GameVars.instance.saveManager.INPUT_RIGHT = newInput;
                }
                else if (currentInput == crouchButton)
                {
                    GameVars.instance.saveManager.INPUT_CROUCH = newInput;
                }
                else if (currentInput == sprintButton)
                {
                    GameVars.instance.saveManager.INPUT_SPRINT = newInput;
                }
                else if (currentInput == jumpButton)
                {
                    GameVars.instance.saveManager.INPUT_JUMP = newInput;
                }
                else if (currentInput == useButton)
                {
                    GameVars.instance.saveManager.INPUT_USE = newInput;
                }
                else if (currentInput == reloadButton)
                {
                    GameVars.instance.saveManager.INPUT_RELOAD = newInput;
                }
                else if (currentInput == quickCycleButton)
                {
                    GameVars.instance.saveManager.INPUT_QUICKCYCLE = newInput;
                }
                else if (currentInput == shootButton)
                {
                    GameVars.instance.saveManager.INPUT_SHOOT = newInput;
                }
                else if (currentInput == mapButton)
                {
                    GameVars.instance.saveManager.INPUT_MAP = newInput;
                }
                else if (currentInput == smoothingButton)
                {
                    if(GameVars.instance.saveManager.SMOOTHING == true)
                    {
                        GameVars.instance.saveManager.SMOOTHING = false;
                    }
                    else
                    {
                        GameVars.instance.saveManager.SMOOTHING = true;
                    }

                    if(InteractionController.instance)
                    {
                        InteractionController.instance.transform.GetChild(0).GetComponent<CameraController>().UpdateSmoothing();
                    }
                }
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UpdateButtonNames();
        }
    }
}
