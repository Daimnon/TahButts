using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _healthIcon;
    [SerializeField] private Transform _healthLayout;

    public void InitializePlayerHealth(PlayerInputHandler player)
    {
        for (int i = 0; i < player.Data.Health; i++)
        {
            // instantiate
        }
        
    }
}
