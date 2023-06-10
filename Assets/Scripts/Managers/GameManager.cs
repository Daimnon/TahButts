using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private PlayerInputHandler _player;
    public PlayerInputHandler Player => _player;

    private int _currentArea = 0;

    private void Awake()
    {
        _instance = this;
    }

    public void UnlockNextArea()
    {
        if (_currentArea < 3)
        {
            CameraManager.Instance.UpdatePositionFactor();
            Vector2 newXBounds;
            newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToFullArea;
            newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToFullArea;
            _player.Controller.XBounds = newXBounds;
            _currentArea++;
        }
        else if (_currentArea == 3)
        {
            CameraManager.Instance.UpdatePositionFactor(true);
            Vector2 newXBounds;
            newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToLastArea;
            newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToLastArea;
            _player.Controller.XBounds = newXBounds;
        }
    }
    /*public void UnlockNextArea(float factor)
    {
        CameraManager.Instance.UpdatePositionFactor(factor);
        Vector2 newXBounds;
        newXBounds.x = _player.Controller.XBounds.x + factor;
        newXBounds.y = _player.Controller.XBounds.y + factor;
        _player.Controller.XBounds = newXBounds;
    }*/
    public void Quit()
    {
        Application.Quit();
    }
}
