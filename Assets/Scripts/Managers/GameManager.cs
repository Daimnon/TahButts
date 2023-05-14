using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private PlayerInputHandler _player;
    public PlayerInputHandler Player => _player;



    private void Awake()
    {
        _instance = this;
    }

    public void UnlockNextArea(float factor)
    {
        CameraManager.Instance.UpdatePositionFactor(factor);
        Vector2 newXBounds;
        newXBounds.x = _player.Controller.TargetXBounds.x + factor;
        newXBounds.y = _player.Controller.TargetXBounds.x + factor;
        _player.Controller.TargetXBounds = newXBounds;
    }
}
