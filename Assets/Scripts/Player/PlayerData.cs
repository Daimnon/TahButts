using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private GameObject _hitColliderGO, _headPhonesPrefab, _shieldPrefab;
    public GameObject HitColliderGO => _hitColliderGO;
    public GameObject HeadPhonesPrefab => _headPhonesPrefab;
    public GameObject ShieldPrefab => _shieldPrefab;

    [SerializeField] private Transform _hitColliderTr;
    public Transform HitColliderTr => _hitColliderTr;

    [SerializeField] private int _lives = 3, _health = 5, _power = 1;
    public int Lives { get => _lives; set => _lives = value; }
    public int Health { get => _health; set => _health = value; }
    public int Power { get => _power; set => _power = value; }
}
