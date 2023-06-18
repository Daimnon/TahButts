using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance => _instance;

    // area one:
    //      grendmas - (0-5)
    //
    // area two:
    //      grendmas - (0-3)
    //      ars (4)
    //
    // area four:
    //      grendmas - (0-1)
    //      ars (2)

    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _areaOne, _areaTwo, _areaThree, _areaFour;
    [SerializeField] private int _areaCount = 4;

    [SerializeField] private List<Enemy> _spawnedEnemyList;
    public List<Enemy> SpawnedEnemyList => _spawnedEnemyList;
    
    [SerializeField] private Transform _spawn;
    public Transform PlayerSpawn => _spawn;

    [SerializeField] private int[] _enemiesToDefeatByArea;
    public int[] EnemiesToDefeatByArea => _enemiesToDefeatByArea;

    private void Awake()
    {
        _instance = this;
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
