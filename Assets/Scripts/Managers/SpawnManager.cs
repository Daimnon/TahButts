using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance => _instance;

    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _areaOne, _areaTwo, _areaThree, _areaFour;

    [SerializeField] private List<Enemy> _spawnedEnemyList;
    public List<Enemy> SpawnedEnemyList => _spawnedEnemyList;

    [SerializeField] private int _areaCount = 4;

    [SerializeField] private Transform _spawn;
    public Transform PlayerSpawn => _spawn;

    private Transform[][] _allAreas; 

    private void Awake()
    {
        _instance = this;
        _allAreas = new Transform[][] { _areaOne, _areaTwo, _areaThree, _areaFour };
    }
    private void Start()
    {
        // should do the spawn logic in method down below
    }

    public GameObject InstantiateEnemy(int typeNum, Vector3 pos)
    {
        GameObject enemy = Instantiate(_enemyPrefabs[typeNum], pos, _enemyPrefabs[typeNum].transform.rotation);
        _spawnedEnemyList.Add(enemy.GetComponent<Enemy>());
        return enemy;
    }
    public GameObject InstantiateEnemy(int typeNum, Transform parent)
    {
        GameObject enemy = Instantiate(_enemyPrefabs[typeNum], parent);
        _spawnedEnemyList.Add(enemy.GetComponent<Enemy>());
        enemy.transform.SetParent(null);
        Destroy(parent);
        return enemy;
    }
    public List<Enemy> InstantiateEnemiesOfType(int typeNum, int amount, List<Vector3> positions)
    {
        List<Enemy> newEnemies = new List<Enemy>(amount);
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefabs[typeNum], positions[i], _enemyPrefabs[typeNum].transform.rotation);
            newEnemies.Add(enemy.GetComponent<Enemy>());
        }

        return newEnemies;
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < _areaCount - 1; i++)
        {

        }
        
    }
}
