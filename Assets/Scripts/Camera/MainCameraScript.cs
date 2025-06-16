
using UnityEngine;
using UnityEngine.Windows;

public class MainCameraScript : MonoBehaviour
{
    /// <summary> Camera instance for the main camera in the scene. </summary>
    private static Camera instance;

    /// <summary> Weight applied to the camera movement input. </summary>
    private const float INPUT_WEIGHT = 0.1f;

    /// <summary> Target angle for the camera rotation, initialized to 45 degrees. </summary>
    private static float targetAngle = 45f;
    /// <summary> Current angle of the camera, initialized to 45 degrees. </summary>
    private static float currentAngle = 45f;
    /// <summary> Speed at which the camera rotates, in degrees per second. </summary>
    private static float rotationSpeed = 90f;
    /// <summary> Radius of the camera's orbit around the pivot point. Recalculates on initialize. </summary>
    private static float radius = 16.25f;
    /// <summary> Height of the camera's orbit above the pivot point. </summary>
    private static float orbitHeight = 8f;
    /// <summary>
    /// The world-space point around which the camera orbits. 
    /// This acts as the dynamic focus of the isometric camera system,
    /// simulating a satellite or kite-like orbital anchor.
    /// </summary>
    public static Vector3 pivot = new Vector3(0, 0, 0);
    /// <summary> Speed at which the camera moves towards the target position. </summary>
    public static float CameraSpeed = 5f;
    /// <summary> Flag to indicate if the camera is currently rotating. </summary>
    private static bool IsRotating { get; set; } = false;

    /// <summary> Camera instance property to access the main camera in the scene. </summary>
    public static Camera InstanceCamera
    {
        get
        {
            if(!isInstance("MainCameraScript instance is null. Returning null."))
                return null;
            return instance;
        }
    }

    /// <summary> Reference to a manually assigned camera if Camera.main is not used. </summary>
    [SerializeField] public Camera CameraReference;
    /// <summary>
    /// Checks if the camera instance is valid and logs a warning if it is null.
    /// </summary>
    /// <param name="warning"> The warning message to log if the instance is null.</param>
    /// <returns> Returns true if the camera instance is valid, false otherwise and prints warning.</returns>
    private static bool isInstance(string warning)
    {
        if (instance == null)
        {
            Debug.LogWarning(warning);
            return false;
        }
        return true;
    }

    private void Awake()
    {
        // Ensure that the camera instance is set to the main camera or a specified reference
        if (Camera.main == null && CameraReference == null)
        {
            Debug.LogError("No main camera found in the scene and settings.");
            return;
        }
        instance = Camera.main == null ? CameraReference : Camera.main;
    }
    
    #region Initialization
    private void Start()
    {
        // Check if the instance is valid before proceeding
        if (!isInstance("MainCameraScript camera instance is null at Start."))
            return;

        // Set the initial position and rotation of the camera
        ResetCamera();
    }

    /// <summary>
    /// Reset the camera to its default position and rotation. 
    /// (By default, it is positioned above the pivot point and looking down at it.)
    /// </summary>
    public static void ResetCamera()
    {
        if (!isInstance("Cannot reset camera rotation, instance is null."))
            return;

        instance.transform.position = new Vector3(10, 8, 10);

        instance.transform.rotation = Quaternion.Euler(23.5f, -135f, 0f);

        // Calculate radius based on the pivot point and camera position
        Vector3 flatOffset = new Vector3(
            pivot.x - instance.transform.position.x, 
            0, 
            pivot.z - instance.transform.position.z);
        radius = flatOffset.magnitude;
    }
    #endregion

    #region Camera Control Methods
    /// <summary> Rotate the camera by a fixed angle (90 degrees) in the specified direction. </summary>
    /// <param name="clockwise">If true, rotates clockwise; if false, rotates counter-clockwise.</param>
    public static void StepRotate(bool clockwise)
    {
        // Adjust the target angle based on the rotation direction on fixed increments
        targetAngle += clockwise ? 90f : -90f;
        // Set flag to indicate that the camera is currently rotating
        IsRotating = true;
    }

    /// <summary> Move the camera based on input from the user. </summary>
    /// <param name="input">A Vector2 representing the input direction (x for right/left, y for forward/backward).</param>
    public static void MoveCamera(Vector2 input)
    {
        // Check if the instance is valid before proceeding
        if (!isInstance("Cannot move camera, instance is null."))
            return;
        // If camera is currently rotating, do not apply movement input
        if (IsRotating)
            return;

        // Relative forward and right vectors based on the camera's current orientation
        Vector3 forward = instance.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = instance.transform.right;
        right.y = 0;
        right.Normalize();

        // Apply input to the pivot point
        Vector3 delta = (right * input.x + forward * input.y) * INPUT_WEIGHT;
        pivot += delta;
    }
    #endregion

    void Update()
    {
        // Check if the instance is valid before proceeding
        if (!isInstance("Cannot update camera, instance is null."))
            return;

        // Smoothly rotate the camera around the pivot point
        if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) <= 0.01f)
        {
            currentAngle = targetAngle;
            IsRotating = false;
        }
        else
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        }

        // Calculate the new position of the camera based on the current angle and radius
        float angleRad = currentAngle * Mathf.Deg2Rad;

        Vector3 localOffset = new Vector3(
            Mathf.Sin(angleRad) * radius,
            orbitHeight,
            Mathf.Cos(angleRad) * radius
        );

        // Set the camera's position and rotation
        Vector3 desiredPosition = pivot + localOffset;

        // Smoothly move the camera to the desired position
        if (IsRotating)
        {
            instance.transform.position = Vector3.Lerp(instance.transform.position, desiredPosition, Time.deltaTime * CameraSpeed);
        }
        // If not rotating, set the position directly
        else
        {
            instance.transform.position = desiredPosition;
        }

        // Ensure the camera is always looking at the pivot point
        Vector3 lookDirection = pivot - instance.transform.position;
        instance.transform.rotation = Quaternion.LookRotation(lookDirection.normalized); 
    }
}
