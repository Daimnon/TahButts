using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private AudioSource _musicTemp;

    [Header("General")]
    [SerializeField] private PlayerInputHandler _player;
    public PlayerInputHandler Player => _player;

    [SerializeField] private Transform _loadingScreenTr;
    public Transform LoadingScreenTr => _loadingScreenTr;

    [SerializeField] private Vector3 _loadingAddedCoords;
    [SerializeField] private float _enterLevelTime, _exitLevelTime, _loadingTransitionDuration;
    [SerializeField] private bool _isTesting;

    [Header("Stages")]
    [SerializeField] private float[] _stageMaxX = new float[] { 131.5f, 167.55f, 203.6f};
    [SerializeField] private List<Enemy> _enemyToDefeat;
    [SerializeField] private List<Enemy> _stageOne, _stageTwo, _stageThree, _stageFour;
    [Range (0, 1)][SerializeField] private float _timeBetweenBlinks = 0.5f;
    [Range(0, 3)][SerializeField] private int _stageCount = 4;

    private List<List<Enemy>> _allStagesEnemies;
    public List<List<Enemy>> AllStagesEnemies => _allStagesEnemies;

    private int _currentStage = 0;
    public int CurrentStage => _currentStage;

    private bool _isLevelPlaying = false;
    public bool IsLevelPlaying => _isLevelPlaying;

    private float _timeBetweenStages = 4.0f;

    public Action<Enemy> OnEnemyDeath;
    public Action<Enemy> OnEnemyPass;

    private delegate void State();
    private State _stageState;

    #region Monobehavior Callbacks
    private void Awake()
    {
        _instance = this;

        if (_isTesting)
            return;

        /*_allStagesEnemies = new List<List<Enemy>>();
        _stageState = FirstStage;*/
    }
    private void Start()
    {
        if (_isTesting)
            return;

        /*_allStagesEnemies = new List<List<Enemy>> { _stageOne, _stageTwo, _stageThree, _stageFour };*/

        OnEnemyDeath += DelistEnemy;
        OnEnemyPass += DelistEnemy;
        StartCoroutine(PlayerLoadingScreen(false));

        _musicTemp.volume = 0.25f;
    }
    private void Update()
    {
        if (_isTesting)
            return;

        /*_stageState.Invoke();*/
    }
    private void OnDisable()
    {
        if (_isTesting)
            return;

        OnEnemyDeath -= DelistEnemy;
        OnEnemyPass -= DelistEnemy;
    }
    #endregion

    #region Stage States
    /*private void FirstStage()
    {
        if (_stageOne.Count <= 0 && _player.transform.position.x < _stageMaxX[3])
        {
            _stageState = SecondStage;
            _currentStage++;
            StartCoroutine(UnlockNextArea());
            Debug.Log("Stage: 01 completed");
        }
    }
    private void SecondStage()
    {
        if (_stageTwo.Count <= 0 && _player.transform.position.x < _stageMaxX[2])
        {
            _stageState = ThirdStage;
            _currentStage++;
            StartCoroutine(UnlockNextArea());
            Debug.Log("Stage: 02 completed");
        }
    }
    private void ThirdStage()
    {
        if (_stageThree.Count <= 0 && _player.transform.position.x < _stageMaxX[1])
        {
            _stageState = FourthStage;
            _currentStage++;
            StartCoroutine(UnlockNextArea());
            Debug.Log("Stage: 03 completed");
        }
    }
    private void FourthStage()
    {
        if (_stageThree.Count <= 0 && _player.transform.position.x < _stageMaxX[0])
        {
            UIManager.Instance.EndPopUp.SetActive(true);
            UIManager.Instance.EndWin.SetActive(true);
            Debug.Log("Stage: 04 completed");
        }
    }*/
    #endregion

    #region Events
    public void InvokeEnemyDeath(Enemy enemy)
    {
        if (enemy != null)
        {
            OnEnemyDeath?.Invoke(enemy);
        }
    }
    public void InvokeEnemyPass(Enemy enemy)
    {
        if (enemy != null)
        {
            OnEnemyPass?.Invoke(enemy);
        }
    }
    #endregion

    private void DelistEnemy(Enemy enemy)
    {
        if (_enemyToDefeat.Contains(enemy))
        {
            for (int i = 0; i < _enemyToDefeat.Count; i++)
            {
                if (_enemyToDefeat[i] == enemy)
                {
                    Debug.Log($"Removing: {_enemyToDefeat[i]}");
                    _enemyToDefeat.RemoveAt(i);

                    foreach (Enemy remainingEnemy in _enemyToDefeat) // debug
                        Debug.Log($"{remainingEnemy.name}");

                    break;
                }
            }
        }
    }

    public IEnumerator PlayerLoadingScreen(bool isLeavingLevel)
    {
        if (_isTesting)
            yield break;

        float time = 0;
        //float speed = 0;

        if (!isLeavingLevel)
        {
            Vector3 startPos = _loadingScreenTr.position;
            Vector3 targetPos = _loadingScreenTr.position - _loadingAddedCoords;
            yield return new WaitForSeconds(_enterLevelTime);

            while(time < _loadingTransitionDuration)
            {
                //float lerpFactor = Mathf.Clamp01(time / (_loadingTransitionDuration / speed));
                _loadingScreenTr.position = Vector3.Lerp(startPos, targetPos, time / _loadingTransitionDuration);
                time += Time.deltaTime;
                //speed += Time.deltaTime;
                yield return null;
            }
            _loadingScreenTr.position = targetPos;
            //_isLevelPlaying = true;
            UIManager.Instance.TutorialPanel.SetActive(true);
        }
        else
        {
            Vector3 startPos = _loadingScreenTr.position;
            Vector3 targetPos = _loadingScreenTr.position + _loadingAddedCoords;

            while (time < _loadingTransitionDuration)
            {
                _loadingScreenTr.position = Vector3.Lerp(startPos, targetPos, time / _loadingTransitionDuration);
                time += Time.deltaTime;
                yield return null;
            }
            _loadingScreenTr.position = targetPos;

            yield return new WaitForSeconds(_enterLevelTime);
            CustomSceneManager.BackToMainMenu();
        }
    }
    public void CallLoadingScreen(bool isLeavingLevel)
    {
        StartCoroutine(PlayerLoadingScreen(isLeavingLevel));
    }
    public void ContinueToGame()
    {
        _isLevelPlaying = true;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        UIManager.Instance.PauseMenu.SetActive(false);
        Debug.Log("Game Unpaused.");
    }
    /*public IEnumerator UnlockNextArea()
    {
        StartCoroutine(UIManager.Instance.NextStageBlink(_timeBetweenBlinks));
        yield return new WaitForSeconds(_timeBetweenBlinks * 2);

        StartCoroutine(CameraManager.Instance.UpdatePositionFactorWithSmoothing(_timeBetweenStages));

        if (_currentStage < _stageCount-1)
        {
            float endValue1 = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToFullArea;
            float endValue2 = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToFullArea;

            Vector2 newXBounds;

            float time = 0;
            float valueToChange1 = _player.Controller.XBounds.x;
            float valueToChange2 = _player.Controller.XBounds.y;

            float startValue1 = valueToChange1;
            float startValue2 = valueToChange2;

            while (time < _timeBetweenStages)
            {
                valueToChange1 = Mathf.Lerp(startValue1, endValue1, time / _timeBetweenStages);
                valueToChange2 = Mathf.Lerp(startValue2, endValue2, time / _timeBetweenStages);

                newXBounds.x = valueToChange1;
                newXBounds.y = valueToChange2;
                _player.Controller.XBounds = newXBounds;

                time += Time.deltaTime;
                yield return null;
            }
            valueToChange1 = endValue1;
            valueToChange2 = endValue2;

            newXBounds.x = valueToChange1;
            newXBounds.y = valueToChange2;
            _player.Controller.XBounds = newXBounds;

            //newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToFullArea;
            //newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToFullArea;
            //_currentStage++;
            Debug.Log(_currentStage);
            //CameraManager.Instance.UpdatePositionFactor();
        }
        else if (_currentStage == _stageCount-1)
        {
            CameraManager.Instance.UpdatePositionFactor(true);
            Vector2 newXBounds;
            newXBounds.x = _player.Controller.XBounds.x - CameraManager.Instance.DistanceToLastArea;
            newXBounds.y = _player.Controller.XBounds.y - CameraManager.Instance.DistanceToLastArea;
            _player.Controller.XBounds = newXBounds;
        }
    }
    public void UnlockNextAreaUI()
    {
        StartCoroutine(UnlockNextArea());
    }*/

    public void Quit()
    {
        Application.Quit();
    }
}
