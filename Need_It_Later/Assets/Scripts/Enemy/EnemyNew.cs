using System.Collections;
using Health;
using Item;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Enemy
{
public class EnemyNew : MonoBehaviour
{
    [Foldout("Misc Parameters", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private float _speed;
    [SerializeField] [Range(0f, 100f)] private float _dropChance;

    [Foldout("Attack Parameters", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private float _attackDelay; // in seconds
    [SerializeField] public float _attackDamage;

    [Foldout("Drops", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private ItemScriptableObject[] _dropPool;

    [Foldout("Events", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private UnityEvent OnHitPlayer;
    [SerializeField] private UnityEvent OnDropItem;
    
    [Foldout("Object References", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private Transform _tf;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] internal SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private EnemyManagerNew _enemyManager;
    internal EnemyPointer _enemyPointer;

    private Transform _target;
    internal bool _canAttack = true;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Walking = Animator.StringToHash("Walking");

    public void Init(Transform target)
    {
        _target = target;
    }
    
    /*private void Update()
    {
        var position = _tf.position;
        // _rb.velocity = (_target.position - position).normalized * _speed;
        _spriteRenderer.sortingOrder = (int)(position.y * -128);
    }*/
    
    private void FixedUpdate()
    {
        var p = navMeshAgent.nextPosition;
        var myPos = _rb.position;
        _animator.SetBool(Walking, navMeshAgent.velocity.magnitude > .01f);
        if (p.x > myPos.x)
        {
            //_spriteRenderer.flipX = false;
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        else if (p.x < myPos.x)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            //_spriteRenderer.flipX = true;
        }

    }

    private void Start()
    {
        if (!_target)
        {
            _target = FindObjectOfType<PlayerAim>().gameObject.transform;
        }

        _animator.gameObject.GetComponent<EnemyAnim>().Strike.AddListener(Attack);
        StartCoroutine(PlayerSearch());
        _enemyPointer = FindObjectOfType<EnemyPointer>();
        _enemyManager = FindObjectOfType<EnemyManagerNew>();
    }

    private IEnumerator PlayerSearch()
    {
        navMeshAgent.destination = _target.position;
        if (Vector2.Distance(_target.position, transform.position) > 5f)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(.3f);
        }
        StartCoroutine(PlayerSearch());
    }

    /*private void FixedUpdate()
    {
        _rb.MovePosition(Vector2.MoveTowards(_tf.position, _target.position, Time.fixedDeltaTime));
    }*/

    private void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("trigger entered");
        if (_canAttack && col.gameObject.tag.Equals("Player"))
        {
            _canAttack = false;
            _animator.SetTrigger(Attack1); //Attack(other.gameObject);
        }
    }
    
    private void Attack(GameObject target)
    {
        if (target != null)
        {
            Debug.Log("Attacking");
            
            target.GetComponent<EntityHealth>().TakeDamage(_attackDamage);
            OnHitPlayer.Invoke();
        }

        StartCoroutine(nameof(AttackLogic));

    }

    IEnumerator AttackLogic()
    {
        yield return new WaitForSeconds(_attackDelay);
        _canAttack = true;
    }

    public void DropItem()
    {
        if (Random.Range(0f, 100f) > _dropChance) return;

        var cumulativeChance = 0;
        var chanceTable = new int[_dropPool.Length];
        for (int i = 0; i < _dropPool.Length; i++)
        {
            cumulativeChance += _dropPool[i].Rarity;
            chanceTable[i] = cumulativeChance;
        }
        var roll = Random.Range(0, cumulativeChance + 1);
        for (int i = chanceTable.Length - 1; i >= 0; i--)
        {
            if (roll >= chanceTable[i])
            {
                var item = Instantiate(_itemPrefab, _tf.position, Quaternion.identity);
                item.GetComponent<Item.Item>().Init(_dropPool[i]);
                return;
            }
        }
    }
}
}
