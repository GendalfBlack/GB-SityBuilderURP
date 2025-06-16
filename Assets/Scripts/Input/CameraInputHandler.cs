using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputHandler : MonoBehaviour
{
    /// <summary> Input action reference for camera movement. </summary>
    [SerializeField] private InputActionReference moveAction;
    /// <summary> Input action reference for camera rotation to the left. </summary>
    [SerializeField] private InputActionReference rotationActionLeft;
    /// <summary> Input action reference for camera rotation to the right. </summary>
    [SerializeField] private InputActionReference rotationActionRight;

    private void OnEnable()
    {
        // Enable the input actions when the script is enabled
        moveAction.action.Enable();
        rotationActionLeft.action.Enable();
        rotationActionRight.action.Enable();
    }
    private void OnDisable()
    {
        // Disable the input actions when the script is disabled
        moveAction.action.Disable();
        rotationActionLeft.action.Disable();
        rotationActionRight.action.Disable();
    }
    void Update()
    {
        // Read the input values from the actions
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        // Check if the camera is currently rotating
        if (moveInput != Vector2.zero)
        {
            MainCameraScript.MoveCamera(moveInput);
            // If the camera is moving, we don't want to process rotation actions
            return;
        }
        // Check if the rotation actions are triggered
        if (rotationActionLeft.action.triggered || rotationActionRight.action.triggered) 
        {
            MainCameraScript.StepRotate(rotationActionLeft.action.triggered ? false : true);
            return;
        }
    }
}
