using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [SerializeField] private GameObject _healthIcon;
    [SerializeField] private Transform _healthLayout;
    [SerializeField] private Image _nextStageBg, _nextStageImg;

    [SerializeField] private GameObject _pauseMenu;
    public GameObject PauseMenu => _pauseMenu;


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
        GameObject heartToRemove = _currentHearts[_currentHearts.Count - 1];
        _currentHearts.RemoveAt(_currentHearts.Count - 1);
        Destroy(heartToRemove);
    }
    public void AddHeart()
    {
        GameObject heart = Instantiate(_healthIcon, _healthLayout);
        _currentHearts.Add(heart);
    }

    public IEnumerator NextStageBlink(float time)
    {
        _nextStageBg.gameObject.SetActive(true);

        _nextStageImg.enabled = true;
        yield return new WaitForSeconds(time);

        _nextStageImg.enabled = false;
        yield return new WaitForSeconds(time / 2);

        _nextStageImg.enabled = true;
        yield return new WaitForSeconds(time);

        _nextStageImg.enabled = false;
        yield return new WaitForSeconds(time / 2);

        _nextStageImg.enabled = true;
        yield return new WaitForSeconds(time);

        _nextStageBg.gameObject.SetActive(false);
    }

    /*public IEnumerator NextStageBlink(float time, int numberOfBlinksDiv2)
    {
        if (numberOfBlinksDiv2 % 2 != 0)
            numberOfBlinksDiv2--;

        _nextStageBg.gameObject.SetActive(true);
        for (int i = 0; i < numberOfBlinksDiv2; i++)
        {
            if (i % 2 == 0)
            {
                _nextStageImg.enabled = true;
                yield return new WaitForSeconds(time);
            }
            else
            {
                _nextStageImg.enabled = false;
                yield return new WaitForSeconds(time / 2);
            }
        }
        yield return new WaitForSeconds(time);

        _nextStageBg.gameObject.SetActive(false);
    }*/
}
