using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;


    [Header("General")]
    [SerializeField] private PlayerInputHandler _player;
    public PlayerInputHandler Player => _player;

    [SerializeField] private Transform _loadingScreenTr;
    public Transform LoadingScreenTr => _loadingScreenTr;

    [SerializeField] private Vector3 _loadingAddedCoords;
    [SerializeField] private float _enterLevelTime, _exitLevelTime, _loadingTransitionDuration;

    [Header("Stages")]
    [SerializeField] private int[] _enemiesToDefeatByStage;
    public int[] EnemiesToDefeatByStage => _enemiesToDefeatByStage;

    [SerializeField] private List<Enemy> _stageOne, _stageTwo, _stageThree, _stageFour;
    [SerializeField] private float[] _stageMaxX;
    [Range(0, 3)][SerializeField] private int _stageCount = 0;

    private List<List<Enemy>> _allStagesEnemies;
    public List<List<Enemy>> AllStagesEnemies => _allStagesEnemies;

    private int _currentStage = 0;
    public int CurrentStage => _currentStage;

    private float _timeBetweenStages = 4.0f;

    public Action<Enemy> OnEnemyDeath;

    private delegate void State();
    private State _stageState;

    #region Monobehavior Callbacks
    private void Awake()
    {
        _instance = this;
        _enemiesToDefeatByStage = new int[] { 0, 0, 0, 0 };
        _stageState = FirstStage;
    }
    private void Start()
    {
        _allStagesEnemies = new List<List<Enemy>> { _stageOne, _stageTwo, _stageThree, _stageFour };

        for (int i = 0; i < _allStagesEnemies.Count; i++)
        {
            for (int j = 0; j < _allStagesEnemies[i].Count; j++)
            {
                if (_allStagesEnemies[i][j] is Ars || _allStagesEnemies[i][j] is Homeless)
                {
                    _enemiesToDefeatByStage[i]++;
                    Debug.Log($"Stage: {i}, Enemy: {_allStagesEnemies[i][j]}, Enemies to defeat in this stage: {_enemiesToDefeatByStage[i]}");
                }
            }
        }

        OnEnemyDeath += DelistEnemy;
        StartCoroutine(PlayerLoadingScreen(false));
    }
    private void Update()
    {
        _stageState.Invoke();
    }
    private void OnDisable()
    {
        OnEnemyDeath -= DelistEnemy;
    }
    #endregion

    #region Stage States
    private void FirstStage()
    {
        if (_enemiesToDefeatByStage[0] <= 0 && _player.transform.position.x < _stageMaxX[0])
        {
            _stageState = SecondStage;
            _currentStage++;
        }
    }
    private void SecondStage()
    {
        if (_enemiesToDefeatByStage[1] <= 0 && _player.transform.position.x < _stageMaxX[1])
        {
            _stageState = ThirdStage;
            _currentStage++;
        }
    }
    private void ThirdStage()
    {
        if (_enemiesToDefeatByStage[2] <= 0 && _player.transform.position.x < _stageMaxX[2])
        {
            _stageState = FourthStage;
            _currentStage++;
        }
    }
    private void FourthStage()
    {

    }
    #endregion

    #region Events
    public void InvokeEnemyDeath(Enemy enemy)
    {
        if (enemy != null)
        {
            OnEnemyDeath?.Invoke(enemy);
        }
    }
    #endregion

    private void DelistEnemy(Enemy enemy)
    {
        if (_allStagesEnemies[_currentStage].Contains(enemy))
        {
            for (int i = 0; i < _allStagesEnemies[_currentStage].Count; i++)
            {
                if (_allStagesEnemies[_currentStage][i] == enemy)
                {
                    Debug.Log($"Removing: {_allStagesEnemies[_currentStage][i]}");
                    _allStagesEnemies[_currentStage].RemoveAt(i);

                    foreach (Enemy remainingEnemy in _allStagesEnemies[_currentStage]) // debug
                        Debug.Log($"{remainingEnemy.name}");

                    break;
                }
            }
        }
    }

    public IEnumerator PlayerLoadingScreen(bool isLeavingLevel)
    {
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
            //CustomSceneManager.BackToMainMenu();
        }
    }
    public void CallLoadingScreen(bool isLeavingLevel)
    {
        StartCoroutine(PlayerLoadingScreen(isLeavingLevel));
    }

    public IEnumerator UnlockNextArea()
    {
        float _timeBetweenBlinks = 1.0f;
        StartCoroutine(UIManager.Instance.NextStageBlink(_timeBetweenBlinks));
        yield return new WaitForSeconds(_timeBetweenBlinks * 3 + _timeBetweenBlinks / 2 * 2);

        StartCoroutine(CameraManager.Instance.UpdatePositionFactorWithSmoothing(_timeBetweenStages));

        if (_currentStage < 3)
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
            _currentStage++;
            //CameraManager.Instance.UpdatePositionFactor();
        }
        else if (_currentStage == 3)
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
    }

    public void Quit()
    {
        Application.Quit();
    }
}
