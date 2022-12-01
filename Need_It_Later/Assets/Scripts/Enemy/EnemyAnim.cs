using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class EnemyAnim : MonoBehaviour
    {
        public UnityEvent<GameObject> Strike;
        public UnityEvent StrikeStop;
        public UnityEvent AttackEnded;

        [SerializeField] private EnemyNew owningEnemy;
        [SerializeField] private Collider2D _hurtBox;
        private EnemyManagerNew _enemyManager;


        private void Start()
        {
            _enemyManager = FindObjectOfType<EnemyManagerNew>();
        }

        public void StrikeAttack()
        {
            Collider2D[] hit = new Collider2D[3]; // = Physics2D.OverlapCapsuleAll(_hurtBox.bounds.center, _hurtBox.bounds.size, CapsuleDirection2D.Horizontal, 0);
            _hurtBox.GetContacts(hit);
            GameObject hitPlayer = null;
            foreach (var col in hit)
            {
                if (col && col.gameObject.CompareTag("Player"))
                {
                    hitPlayer = col.gameObject;
                }
            }
            Strike.Invoke(hitPlayer);
            
            //Strike.Invoke(FindObjectOfType<PlayerAim>().gameObject);
            //GetComponentInParent<CapsuleCollider2D>().enabled = true;
        }
    
        public void StrikeEnd()
        {
            StrikeStop.Invoke();
            //GetComponentInParent<CapsuleCollider2D>().enabled = false;
        }
        
        public void AttackEnd()
        {
            AttackEnded.Invoke();
            //GetComponentInParent<EnemyNew>()._canAttack = true;;
        }
        
        private void OnBecameVisible()
        {
            //Debug.Log("Visible");
            _enemyManager.VisibleEnemies.Add(owningEnemy);
            owningEnemy._enemyPointer.EnemyVisibilityChanged(_enemyManager.VisibleEnemies);
        }

        private void OnBecameInvisible()
        {
            _enemyManager.VisibleEnemies.Remove(owningEnemy);
            //Debug.Log("Invisible");
            /*for (var i = 0; i < _enemyManager.VisibleEnemies.Count; i++)
            {
                if (ReferenceEquals(owningEnemy, _enemyManager.VisibleEnemies[i]))
                {
                    _enemyManager.VisibleEnemies.RemoveAt(i);
                    //owningEnemy._enemyPointer.EnemyVisibilityChanged(_enemyManager.VisibleEnemies);
                    break;
                }
            }*/
            owningEnemy._enemyPointer.EnemyVisibilityChanged(_enemyManager.VisibleEnemies);

        }
    }
}
