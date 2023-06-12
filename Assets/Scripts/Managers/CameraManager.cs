using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance => _instance;

    [SerializeField] private Camera _mainCam;
    public Camera MainCam => _mainCam;

    [SerializeField] private Transform _playerTr; // Reference to the player's transform

    [SerializeField] private Vector2 _xRange = new(119.65f, 155.7f), _yRange = new(0f, 2.3f);
    public Vector2 XRange => _xRange;
    public Vector2 YRange => _yRange;

    [SerializeField] private Vector2 _offsets = Vector2.zero;
    [SerializeField] private float _distanceToFullArea = 36.05f;
    [SerializeField] private float _distanceToLastArea = 18.025f;

    [SerializeField] private float _followSpeed = 2.0f; // Speed at which the camera follows the player
    [SerializeField] private float _xMargin = 8.0f, _yMargin = 8.0f; // Minimum distance in the x-axis and y-axis the player can move before the camera follows

    public float DistanceToFullArea => _distanceToFullArea;
    public float DistanceToLastArea => _distanceToLastArea;

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
        Vector3 targetPosition = new Vector3(_playerTr.position.x + _offsets.x, _playerTr.position.y + _offsets.y, currentPosition.z);

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

    public void UpdatePositionFactor()
    {
        _xRange.x -= _distanceToFullArea;
        _xRange.y -= _distanceToFullArea;
    }
    public void UpdatePositionFactor(bool useLastArea)
    {
        _xRange.x -= _distanceToLastArea;
        _xRange.y -= _distanceToLastArea;
    }
    public IEnumerator UpdatePositionFactorWithSmoothing(float timeBetweenStages)
    {
        float endValue1 = _xRange.x - _distanceToFullArea;
        float endValue2 = _xRange.y - _distanceToFullArea;

        Vector2 xTempRange;

        float time = 0;
        float valueToChange1 = _xRange.x;
        float valueToChange2 = _xRange.y;

        float startValue1 = valueToChange1;
        float startValue2 = valueToChange2;

        while (time < timeBetweenStages)
        {
            valueToChange1 = Mathf.Lerp(startValue1, endValue1, time / timeBetweenStages);
            valueToChange2 = Mathf.Lerp(startValue2, endValue2, time / timeBetweenStages);

            xTempRange.x = valueToChange1;
            xTempRange.y = valueToChange2;
            _xRange = xTempRange;

            time += Time.deltaTime;
            yield return null;
        }
        xTempRange.x = endValue1;
        xTempRange.y = endValue2;
        _xRange = xTempRange;
    }
}
