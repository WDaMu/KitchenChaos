using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler onProcessPerformed;
    public event EventHandler onPickAndPlacePerformed;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("GameInput instance already exists");
            Destroy(gameObject);
        }

        // 初始化InputSystem
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Process.performed += PlayerInputActions_ProcessPerformed;
        playerInputActions.Player.PickAndPlace.performed += PlayerInputActions_PickAndPlacePerformed;
    }

    private void PlayerInputActions_PickAndPlacePerformed(InputAction.CallbackContext context)
    {
        onPickAndPlacePerformed?.Invoke(this, EventArgs.Empty);   
    }

    private void PlayerInputActions_ProcessPerformed(InputAction.CallbackContext context)
    {
        onProcessPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovementInputNormalized()
    {
        Vector3 inputDir = Vector3.zero;

        Vector2 moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputDir.x = moveInput.x;
        inputDir.z = moveInput.y;
        
        return inputDir.normalized;
    }
}
