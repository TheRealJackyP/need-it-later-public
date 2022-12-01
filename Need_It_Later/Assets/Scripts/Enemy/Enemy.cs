using System;
using System.Threading.Tasks;
using Health;
using Item;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Enemy
{
public class Enemy : MonoBehaviour
{
    [Foldout("Misc Parameters", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private float _speed;
    [SerializeField] [Range(0f, 100f)] private float _dropChance;
    
    [Foldout("Attack Parameters", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private int _attackDelay; // in milliseconds
    [SerializeField] private float _attackDamage;

    [Foldout("Drops", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private ItemScriptableObject[] _dropPool;

    [Foldout("Events", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private UnityEvent OnHitPlayer;
    [SerializeField] private UnityEvent OnDropItem;
    
    [Foldout("Object References", foldEverything = true, styled = true, readOnly = false)]
    [SerializeField] private Transform _tf;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _itemPrefab;

    private Transform _target;
    private bool _canAttack = true;
    
    public void Init(Transform target)
    {
        _target = target;
    }
    
    private void Update()
    {
        var position = _tf.position;
        // _rb.velocity = (_target.position - position).normalized * _speed;
        _spriteRenderer.sortingOrder = (int)(position.y * -128);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(Vector2.MoveTowards(_tf.position, _target.position, Time.fixedDeltaTime));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log("trigger entered");
        if (_canAttack && other.tag.Equals("Player")) Attack(other.gameObject);
    }
    
    private async void Attack(GameObject target)
    {
        Debug.Log("Attacking");
        _canAttack = false;
        target.GetComponent<EntityHealth>().TakeDamage(_attackDamage);
        OnHitPlayer.Invoke();
        await Task.Delay(_attackDelay);
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
