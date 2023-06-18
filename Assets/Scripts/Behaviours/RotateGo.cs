using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGo : MonoBehaviour
{
    [Range(10, 1000)][SerializeField] private float _rotateAmount;
    [SerializeField] private bool _isInverted;

    private void Update()
    {
        Rotate(_isInverted);
    }

    private void Rotate(bool isInverted)
    {
        if (isInverted)
            transform.Rotate(0, 0, _rotateAmount * Time.deltaTime);
        else
            transform.Rotate(0, 0, -_rotateAmount * Time.deltaTime);
    }
}
