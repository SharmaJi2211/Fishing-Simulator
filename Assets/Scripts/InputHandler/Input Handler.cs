using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInputAction inputActions; 
    
    void Awake()
    {
        inputActions = new PlayerInputAction();
    }
    
    void Update()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        GameEvents.MoveInput(moveInput);
    }

    void OnEnable()
    {
        // Enabling the Script
        inputActions.Player.Enable();

        // Subscribing to the event, means if the lmb is pressed the event it subscribed to fires once
        inputActions.Player.Cast.performed += OnCast;

        // Reeling in
        inputActions.Player.Reel.performed += OnReelDown;
        inputActions.Player.Reel.canceled += OnReelUp; 

        // Interaction
        inputActions.Player.Interact.performed += OnInteracted;
    }

    void OnDisable()
    {
        // Disabling the Script
        inputActions.Player.Disable();

        // LMB
        inputActions.Player.Cast.performed -= OnCast;

        inputActions.Player.Reel.performed -= OnReelDown;
        inputActions.Player.Reel.canceled -= OnReelUp;

        inputActions.Player.Interact.performed -= OnInteracted;
    }


    // If event is performed call the desired function
    public void OnCast(InputAction.CallbackContext context)
    {
        GameEvents.CastInput();
    }

    public void OnReelDown(InputAction.CallbackContext context)
    {
        GameEvents.ReelHoldDown();
    }

    public void OnReelUp(InputAction.CallbackContext context)
    {
        GameEvents.ReelHoldUp();
    }

    public void OnInteracted(InputAction.CallbackContext context)
    {
        InteractionEvent.InteractPressed();
    }
}