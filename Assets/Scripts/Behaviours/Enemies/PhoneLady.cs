using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PhoneLady : Enemy
{
    [SerializeField] private float _playerPassDistance = 2.0f, _harmTime = 1.5f;
    [SerializeField] private bool _isTimerOn;
    [SerializeField] private IEnumerator _timer;

    private void Awake()
    {
        _timer = Timer();
    }
    private void Update()
    {
        /*if (Target.transform.position.x < transform.position.x - _playerPassDistance)
            GameManager.Instance.InvokeEnemyPass(this);*/
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
        if (!_isTimerOn || GameManager.Instance.Player.Controller.IsUsingHeadphones)
            yield break;

        yield return new WaitForSeconds(_harmTime);

        if (!_isTimerOn || GameManager.Instance.Player.Controller.IsUsingHeadphones)
            yield break;

        GameManager.Instance.Player.Controller.TakeDamage(1);
        _timer = null;
        _timer = Timer();
        StartCoroutine(_timer);
    }
    public void HarmPlayer()
    {
        _isTimerOn = true;
        StartCoroutine(_timer);

        //Omer - here I can add a gossip attitude kind of talk towards the Player 


    }
    public void StopHarmingPlayer()
    {
        StopCoroutine(_timer);
        _isTimerOn = false;
    }
}
