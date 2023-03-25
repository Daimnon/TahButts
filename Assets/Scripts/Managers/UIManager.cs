using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;


    [SerializeField] private GameObject _healthIcon;
    [SerializeField] private Transform _healthLayout;

    private List<GameObject> _currentHearts;

    private void Awake()
    {
        _instance = this;
    }

    public void InitializePlayerHealth(PlayerInputHandler player)
    {
        _currentHearts = new();

        for (int i = 0; i < player.Data.Health; i++)
        {
            GameObject heart = Instantiate(_healthIcon, _healthLayout);
            _currentHearts.Add(heart);
        }
    }
    public void RemoveHeart()
    {
        _currentHearts.RemoveAt(_currentHearts.Count -1);
    }
    public void AddHeart()
    {
        GameObject heart = Instantiate(_healthIcon, _healthLayout);
        _currentHearts.Add(heart);
    }
}
