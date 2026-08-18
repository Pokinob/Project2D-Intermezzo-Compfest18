using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private Vector2 moveDirection = Vector2.zero;
    private bool interactPressed = false;
    private bool submitPressed = false;
    private bool selectPressed = false;
    private bool freezeInput = false;
    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public bool GetFreezeInput()
    {
        return freezeInput;
    }

    public void FreezeInput()
    {
        freezeInput = true;
    }

    public void UnfreezeInput()
    {
        freezeInput = false;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (freezeInput)
        {
            moveDirection = Vector2.zero;
            return;
        }
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = Vector2.zero;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public void SelectPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectPressed = true;
        }
        else if (context.canceled)
        {
            selectPressed = false;
        }
    }

    public bool GetSelectPressed()
    {
        return selectPressed;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }

}