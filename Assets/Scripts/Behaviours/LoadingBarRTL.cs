using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarRTL : MonoBehaviour
{
    [SerializeField] private Image _loadingBarFill;
    [SerializeField] private float _fillDuration = 1.0f;

    private Vector3 _originalScale = Vector3.one;
    private void Start()
    {
        StartCoroutine(FillBar());
    }

    private IEnumerator FillBar()
    {
        float time = 0;
        Vector3 startPos = _loadingBarFill.transform.localScale;

        while (time < _fillDuration)
        {
            _loadingBarFill.transform.localScale = Vector3.Lerp(startPos, _originalScale, time / _fillDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _loadingBarFill.transform.localScale = _originalScale;
    }
}
