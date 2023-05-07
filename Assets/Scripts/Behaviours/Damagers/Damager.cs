using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    protected int _damage = 0;
    public int Damage { get => _damage; set => _damage = value; }
}
