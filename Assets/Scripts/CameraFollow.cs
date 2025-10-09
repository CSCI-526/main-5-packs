using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target that the camera will follow (usually the player).
    public Transform target;

    // The smoothing speed for the camera follow. 
    // Smaller values result in a slower, smoother follow.
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    // The initial offset between the camera and the target.
    private Vector3 offset;

    // A variable to store the initial Y-axis position of the camera.
    private float initialY;

    void Start()
    {
        // Check if a target has been assigned.
        if (target == null)
        {
            Debug.LogError("CameraFollow script: Target not set!");
            return;
        }
        
        // Calculate and store the initial offset between the camera and the target.
        offset = transform.position - target.position;

        // Record the starting Y position of the camera.
        initialY = transform.position.y;
    }
    
    // LateUpdate is called once per frame, after all Update functions have been completed.
    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera.
            Vector3 desiredPosition = target.position + offset;

            // Lock the Y-axis to its initial position.
            desiredPosition.y = initialY;

            // Smoothly interpolate from the camera's current position to the desired position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the calculated smoothed position to the camera.
            transform.position = smoothedPosition;
        }
    }
}