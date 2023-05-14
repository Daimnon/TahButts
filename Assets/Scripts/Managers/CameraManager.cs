using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance => _instance;

    [SerializeField] private Camera _mainCam;
    public Camera MainCam => _mainCam;

    [SerializeField] private Transform _playerTr; // Reference to the player's transform

    [SerializeField] private Vector2 _xRange = new(9.0f, 13.5f), _yRange = new(-2.5f, 2.5f);
    public Vector2 XRange => _xRange;
    public Vector2 YRange => _yRange;

    [SerializeField] private float _positionFactor = 0;
    public float PositionFactor => _positionFactor;

    [SerializeField] private float _followSpeed = 2.0f; // Speed at which the camera follows the player
    [SerializeField] private float _xMargin = 8.0f, _yMargin = 8.0f; // Minimum distance in the x-axis and y-axis the player can move before the camera follows

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;
    }
    private void LateUpdate()
    {
        // Get the current camera position
        Vector3 currentPosition = _mainCam.transform.position;

        // Calculate the target position for the camera based on the player's position
        Vector3 targetPosition = new (_playerTr.position.x, _playerTr.position.y, currentPosition.z);

        // Calculate the x and y distances between the camera and the player
        float xDistance = Mathf.Abs(currentPosition.x - _playerTr.position.x);
        float yDistance = Mathf.Abs(currentPosition.y - _playerTr.position.y);

        // Move the camera towards the target position if the player is beyond the camera's margin
        if (xDistance > _xMargin)
            currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x, _followSpeed * Time.deltaTime);
        
        if (yDistance > _yMargin)
            currentPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, _followSpeed * 2.5f * Time.deltaTime);

        // Clamp the camera's position within the game world's boundaries
        currentPosition.x = Mathf.Clamp(currentPosition.x, _xRange.x, _xRange.y);
        currentPosition.y = Mathf.Clamp(currentPosition.y, _yRange.x, _yRange.y);

        // Update the camera's position
        _mainCam.transform.position = currentPosition;
    }

    public void UpdatePositionFactor(float newFactor)
    {
        _positionFactor = newFactor;
        _xRange.x += _positionFactor;
        _xRange.y += _positionFactor;
    }
}
