using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputController : MonoBehaviour
{
    private PlayerInputControls _playerInputControls;

    [SerializeField] private BoolReference _canTakeInput;
    [SerializeField] private Vector2Reference _moveInput;
    [SerializeField] private Vector2Reference _mousePosition;
    [SerializeField] private BoolReference _leftMouseInput;
    [SerializeField] private BoolReference _rightMouseInput;
    [SerializeField] private BoolReference _leftMouseHoldInput;
    [SerializeField] private BoolReference _rightMouseHoldInput;

    private void OnEnable()
    {
        _playerInputControls ??= new PlayerInputControls();
        
        _playerInputControls.Enable();

        _playerInputControls.Gameplay.Movement.performed += OnMovement;
        _playerInputControls.Gameplay.Movement.canceled += OnMovement;
        
        _playerInputControls.Gameplay.MousePosition.performed += OnUpdateMousePosition;
        _playerInputControls.Gameplay.MousePosition.canceled += OnUpdateMousePosition;
        
        _playerInputControls.Gameplay.MouseLeftClick.started += OnLeftMouseClick;
        _playerInputControls.Gameplay.MouseLeftClick.performed += OnLeftMouseClick;
        _playerInputControls.Gameplay.MouseLeftClick.canceled += OnLeftMouseClick;
        _playerInputControls.Gameplay.MouseRightClick.performed += OnRightMouseClick;
        _playerInputControls.Gameplay.MouseRightClick.canceled += OnRightMouseClick;
    }
    
    private void OnDisable()
    {
        _playerInputControls.Gameplay.Movement.performed -= OnMovement;
        _playerInputControls.Gameplay.Movement.canceled -= OnMovement;
        
        _playerInputControls.Gameplay.MousePosition.performed -= OnUpdateMousePosition;
        _playerInputControls.Gameplay.MousePosition.canceled -= OnUpdateMousePosition;
        
        _playerInputControls.Gameplay.MouseLeftClick.started -= OnLeftMouseClick;
        _playerInputControls.Gameplay.MouseLeftClick.performed -= OnLeftMouseClick;
        _playerInputControls.Gameplay.MouseLeftClick.canceled -= OnLeftMouseClick;
        _playerInputControls.Gameplay.MouseRightClick.performed -= OnRightMouseClick;
        _playerInputControls.Gameplay.MouseRightClick.canceled -= OnRightMouseClick;

        _playerInputControls.Disable();
    }

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 temp = ctx.ReadValue<Vector2>();
        
        // TODO Just keep original _moveInput for Controller Input
        temp.x = (temp * Vector2.right).normalized.x;
        temp.y = (temp * Vector2.up).normalized.y;

        _moveInput.Value = _canTakeInput.Value ? temp : Vector2.zero;
    }
    
    private void OnUpdateMousePosition(InputAction.CallbackContext ctx)
    {
        Vector2 temp = ctx.ReadValue<Vector2>();
        _mousePosition.Value = temp;
    }
    
    private void OnRightMouseClick(InputAction.CallbackContext ctx)
    {
        // _rightMouseHoldInput is currently not in use
        if (ctx.performed)
        {
            _rightMouseInput.Value = true;
        }
        else if (ctx.canceled)
        {
            _rightMouseInput.Value = false;
        }
    }
    
    private void OnLeftMouseClick(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                switch (ctx.interaction)
                {
                    case TapInteraction:
                        // Tap Started
                        _leftMouseInput.Value = true;
                        break;
                    case SlowTapInteraction:
                        // Hold Started
                        _leftMouseHoldInput.Value = true;
                        break;
                }
                break;
            case InputActionPhase.Performed:
            case InputActionPhase.Canceled:
                switch (ctx.interaction)
                {
                    case TapInteraction:
                        // Tap Performed / Ended
                        _leftMouseInput.Value = false;
                        break;
                    case SlowTapInteraction:
                        // Hold Performed / Ended
                        _leftMouseHoldInput.Value = false;
                        break;
                }
                break;
        }
    }
}
