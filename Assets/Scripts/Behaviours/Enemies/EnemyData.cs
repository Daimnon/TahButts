using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] protected int _health = 5, _power = 1;
    public int Health { get => _health; set => _health = value; }
    public int Power { get => _power; set => _power = value; }
}
