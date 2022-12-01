using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyPointer : MonoBehaviour
{
    [SerializeField] private LookAtConstraint lookAtConstraint;
    [SerializeField] private EnemyManagerNew enemyManager;
    //[SerializeField] private float waitTime = 1f;
    private SpriteRenderer _targetSpriteRenderer;
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    //private int _checks = 0;
    //private int _maxChecks = 5;
    
    void Start()
    {
        //StartCoroutine(nameof(MyJob));
        lookAtConstraint.constraintActive = true;
        mySpriteRenderer.enabled = false;
        //_mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void EnemyVisibilityChanged(List<EnemyNew> visible) //List<EnemyNew> inVisible, 
    {
        //Debug.Log(visible);
        if (visible.Count <= 0 && enemyManager.ActiveEnemies.Count > 0)
        {
            EnemyNew closestEnemy = null;
            var dis = 1000f;
            foreach (var e in enemyManager.ActiveEnemies)
            {
                
                var d = Vector2.Distance(transform.position, e.transform.position);
                if (d < dis)
                {
                    dis = d;
                    closestEnemy = e.GetComponent<EnemyNew>();
                }
                
            }
            lookAtConstraint.SetSource(0, new ConstraintSource {sourceTransform = closestEnemy.transform, weight = 1});
            mySpriteRenderer.enabled = true;
        }
        else
        {
            
            mySpriteRenderer.enabled = false;
        }
    }



    /*private IEnumerator MyJob()
    {
        
    }*/
    /*{
        yield return new WaitForSeconds(waitTime);
        if (_checks >= _maxChecks)
        {
            _checks = 0;
            FindClosestEnemy();
        }
        else if (enemyManager.ActiveEnemies.Count == 0)
        {
            _checks = 0;
        }
        else if (_targetSpriteRenderer && !_targetSpriteRenderer.isVisible)
        {
            _mySpriteRenderer.enabled = true;
            _checks++;
        }
        else
        {
            FindClosestEnemy();
        }

        StartCoroutine(nameof(MyJob));
    }

    /*private bool IsAnyEnemyOnScreen()
    {
        return enemyManager.ActiveEnemies.Any(enemy => enemy.GetComponent<EnemyNew>().isVisible);
    }#1#

    private void FindClosestEnemy()
    {

        GameObject closestEnemy = null;
        var dis = 1000f;
        foreach (var enemy in enemyManager.ActiveEnemies)
        {
            if (enemy.GetComponent<EnemyNew>().spriteRenderer.isVisible)
            {
                _mySpriteRenderer.enabled = false;
                return;
            }
            
            var d = Vector2.Distance(transform.position, enemy.transform.position);
            if (d < dis)
            {
                dis = d;
                closestEnemy = enemy;
            }
        }

        if (Math.Abs(dis - 1000f) > .01f) // This is my mildly clever alternative to an expensive null check
        {
            lookAtConstraint.SetSource(0, new ConstraintSource {sourceTransform = closestEnemy.transform, weight = 1});
            _mySpriteRenderer.enabled = true;
            _targetSpriteRenderer = closestEnemy.GetComponent<EnemyNew>().spriteRenderer;
        }
    }*/
}
