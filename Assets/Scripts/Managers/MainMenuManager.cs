using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager _instance;
    public static MainMenuManager Instance => _instance;

    [SerializeField] private float _busFirstTargetX = 1930.0f, _busSecondTargetX = -2060.0f;
    [SerializeField] private Image _busImage;
    [SerializeField] private int _newGameBuildIndex = 1;
    [SerializeField] private float _timeToGetOnBus = 1.0f, _timeToQuit = 1.0f;
    [SerializeField] private float _busSpeed;
    
    private void Awake()
    {
        _instance = this;
    }

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameSequence());
    }
    public void QuitGame()
    {
        StartCoroutine(QuitGameAfterTime(_timeToQuit));
    }

    private IEnumerator StartNewGameSequence()
    {
        // player audio que
        // while _busImage is not at _busFirstTargetX move _busImage at _busSpeed on x axis
        while (_busImage.rectTransform.position.x > _busFirstTargetX)
        {
            Vector2 newPos = _busImage.rectTransform.position;
            newPos.x = _busImage.rectTransform.position.x - _busSpeed * Time.deltaTime;
            
            _busImage.rectTransform.position = newPos;
            yield return null;
        }
        
        // stop for Time and play another audio que
        yield return new WaitForSeconds(_timeToGetOnBus);

        // while _busImage is not at _busSecondTargetX move _busImage at _busSpeed on x axis
        while (_busImage.rectTransform.position.x > _busSecondTargetX)
        {
            Vector2 newPos = _busImage.rectTransform.position;
            newPos.x = _busImage.rectTransform.position.x -  _busSpeed * Time.deltaTime;

            _busImage.rectTransform.position = newPos;
            yield return null;
        }

        // change scene
        CustomSceneManager.ChangeScene(_newGameBuildIndex);
    }
    private IEnumerator QuitGameAfterTime(float sec)
    {
        yield return new WaitForSeconds(sec);
        CustomSceneManager.QuitGame();
    }
}