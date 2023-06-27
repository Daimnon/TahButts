using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : Damager
{
    protected const string EnemyTag = "Enemy";
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag(_enemyTag))
    //    {
    //        Debug.Log($"Hit {collision.collider.name}");
    //        Enemy enemy = collision.collider.GetComponent<Enemy>();
    //        enemy.TakeDamage(_damage);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(EnemyTag))
        {
            Debug.Log($"Hit {other.name}");
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.AnimController.SetTrigger("GotPunched");
            enemy.TakeDamage(_damage);
            //StartCoroutine(ShakeOnHit(enemy.AnimController, 0.1f));
        }
    }

    private IEnumerator ShakeOnHit(Animator animController, float timeBetweenSake)
    {
        AnimatorStateInfo stateInfo = animController.GetCurrentAnimatorStateInfo(0);
        Vector3 startingPos = animController.transform.position;
        bool isMovingLeft = true;

        while (stateInfo.IsTag("Hurt") && animController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            stateInfo = animController.GetCurrentAnimatorStateInfo(0);
            
            if (isMovingLeft)
            {
                animController.transform.position = new (startingPos.x - 1, startingPos.y, startingPos.z);
                isMovingLeft = false;
            }
            else
            {
                animController.transform.position = new(startingPos.x + 1, startingPos.y, startingPos.z);
                isMovingLeft = true;
            }

            yield return new WaitForSeconds(timeBetweenSake);
        }
    }
}
