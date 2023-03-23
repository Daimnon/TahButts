using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private Transform _spawn;
    public Transform Spawn => _spawn;

    private void Awake()
    {
        _instance = this;
    }
}
