using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Movement _movement;
    private PickUpContext _pickUpContext;
    private ActionContext _actionContext;
    private SuperContext _superContext;

    private void Start()
    {
        _movement = GameplayController.Instance.PlayerObject.GetComponent<Movement>();
        _pickUpContext = GetComponent<PickUpContext>();
        _actionContext = GetComponent<ActionContext>();
        _superContext = GetComponent<SuperContext>();
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            _movement.Horizontal(direction.x);
            _movement.Vertical(direction.y);
        }
        else if (context.canceled)
        {
            _movement.Horizontal(0);
            _movement.Vertical(0);
        }
    }

    public void PickUpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _pickUpContext.PickUp();
        }        
    }

    public void ActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _actionContext.Action();
        }        
    }

    public void SuperInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _superContext.Super();
        }       
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameplayController.Instance.OpenPauseMenu(!GameplayController.Instance.Paused);
        }
    }
}
