using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private PlayerInputHandler _player;
    public PlayerInputHandler Player => _player;

    private float _timeBetweenStages = 4.0f;
    private int _currentStage = 0;

    private void Awake()
    {
        _instance = this;
    }

    public IEnumerator UnlockNextArea()
    {
        float _timeBetweenBlinks = 1.0f;
        StartCoroutine(UIManager.Instance.NextStageBlink(_timeBetweenBlinks));
        yield return new WaitForSeconds(_timeBetweenBlinks * 3 + _timeBetweenBlinks / 2 * 2);

        StartCoroutine(CameraManager.Instance.UpdatePositionFactorWithSmoothing(_timeBetweenStages));

        if (_currentStage < 3)
        {
            float endValue1 = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToFullArea;
            float endValue2 = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToFullArea;

            Vector2 newXBounds;

            float time = 0;
            float valueToChange1 = _player.Controller.XBounds.x;
            float valueToChange2 = _player.Controller.XBounds.y;

            float startValue1 = valueToChange1;
            float startValue2 = valueToChange2;

            while (time < _timeBetweenStages)
            {
                valueToChange1 = Mathf.Lerp(startValue1, endValue1, time / _timeBetweenStages);
                valueToChange2 = Mathf.Lerp(startValue2, endValue2, time / _timeBetweenStages);

                newXBounds.x = valueToChange1;
                newXBounds.y = valueToChange2;
                _player.Controller.XBounds = newXBounds;

                time += Time.deltaTime;
                yield return null;
            }
            valueToChange1 = endValue1;
            valueToChange2 = endValue2;

            newXBounds.x = valueToChange1;
            newXBounds.y = valueToChange2;
            _player.Controller.XBounds = newXBounds;

            //newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToFullArea;
            //newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToFullArea;
            _currentStage++;
            //CameraManager.Instance.UpdatePositionFactor();
        }
        else if (_currentStage == 3)
        {
            CameraManager.Instance.UpdatePositionFactor(true);
            Vector2 newXBounds;
            newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToLastArea;
            newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToLastArea;
            _player.Controller.XBounds = newXBounds;
        }
    }
    /*public IEnumerator UnlockNextArea()
    {
        StartCoroutine(UIManager.Instance.NextStageBlink(1, 3));
        yield return new WaitForSeconds(1*2 + 1.5f*2);
        if (_currentStage < 3)
        {
            CameraManager.Instance.UpdatePositionFactor();
            Vector2 newXBounds;
            newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToFullArea;
            newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToFullArea;
            _player.Controller.XBounds = newXBounds;
            _currentStage++;
        }
        else if (_currentStage == 3)
        {
            CameraManager.Instance.UpdatePositionFactor(true);
            Vector2 newXBounds;
            newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToLastArea;
            newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToLastArea;
            _player.Controller.XBounds = newXBounds;
        }
    }*/
    public void UnlockNextAreaUI()
    {
        StartCoroutine(UnlockNextArea());
    }
    public void Quit()
    {
        Application.Quit();
    }
}
