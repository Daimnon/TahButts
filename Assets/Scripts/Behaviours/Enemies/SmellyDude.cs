using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellyDude : Enemy
{
    [SerializeField] private float _playerPassDistance = 2.0f, _harmTime = 1.5f;
    [SerializeField] private IEnumerator _timer;

    private void Awake()
    {
        _timer = Timer();
    }
    private void Update()
    {
        if (Target.transform.position.x < transform.position.x - _playerPassDistance)
            GameManager.Instance.InvokeEnemyPass(this);
    }
    protected override void Interacting()
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayerInsight()
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayerNotInsight()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_harmTime);

        GameManager.Instance.Player.Controller.TakeDamage(1);
        _timer = null;
        _timer = Timer();
        StartCoroutine(_timer);
    }
    public void HarmPlayer()
    {
        StartCoroutine(_timer);
    }
    public void StopHarmingPlayer()
    {
        StopCoroutine(_timer);
    }
}
