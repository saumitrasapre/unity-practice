using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions playerInputActions;
    private Camera mainCamera;

    public event EventHandler<OnMousePositionChangedEventArgs> OnMousePositionChanged;
    public event EventHandler OnFirePressed;
    public event EventHandler OnFireReleased;


    public class OnMousePositionChangedEventArgs
    {
        public Vector2 mousePosition;
    }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        mainCamera = Camera.main;
        playerInputActions.Player.Fire.started += Fire_started;
        playerInputActions.Player.Fire.canceled += Fire_canceled;
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose();
    }

    public void DisableGameInput()
    {
        playerInputActions.Player.Disable();
    }

    private void Fire_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnFireReleased != null)
        {
            OnFireReleased(this, EventArgs.Empty);
        }
    }

    private void Fire_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnFirePressed != null)
        {
            OnFirePressed(this, EventArgs.Empty);
        }
    }



    private void Update()
    {
        if (OnMousePositionChanged != null)
        {
            OnMousePositionChanged(this, new OnMousePositionChangedEventArgs
            {
                mousePosition = GetMousePosition()
            });
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector2 GetMousePosition()
    {
        Vector3 mousePosition = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        mousePosition.z = mainCamera.nearClipPlane;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }
}

