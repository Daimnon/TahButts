using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCam;
    public Camera MainCam => _mainCam;

    public Transform player;                // Reference to the player's transform
    public Vector2 minBounds;               // Minimum bounds of the camera's position
    public Vector2 maxBounds;               // Maximum bounds of the camera's position
    public float followSpeed = 2.0f;         // Speed at which the camera follows the player
    public float xMargin = 0.5f;            // Minimum distance in the x-axis the player can move before the camera follows
    public float yMargin = 0.5f;            // Minimum distance in the y-axis the player can move before the camera follows

    void LateUpdate()
    {
        // Get the current camera position
        Vector3 currentPosition = _mainCam.transform.position;

        // Calculate the target position for the camera based on the player's position
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, currentPosition.z);

        // Calculate the x and y distances between the camera and the player
        float xDistance = Mathf.Abs(currentPosition.x - player.position.x);
        float yDistance = Mathf.Abs(currentPosition.y - player.position.y);

        // Move the camera towards the target position if the player is beyond the camera's margin
        if (xDistance > xMargin)
        {
            currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, followSpeed * Time.deltaTime);
        }

        if (yDistance > yMargin)
        {
            currentPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, followSpeed * Time.deltaTime);
        }

        // Clamp the camera's position within the game world's boundaries
        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y);

        // Update the camera's position
        _mainCam.transform.position = currentPosition;
    }
}
