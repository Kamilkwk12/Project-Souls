using System.Threading;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    [SerializeField] float _health;
    [SerializeField] float _maxHealth = 100;
    Animator _animator;

    [SerializeField] Transform _healthbarValue;
    public GameObject healthBar;

    public bool CanBeHit = true;

    void Start()
    { 
        _health = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    
    private void Update()
    {
        _healthbarValue.localScale = new Vector3(_health / _maxHealth, 1, 1);

        EnemyFlip();

    }

    public void TakeDamage(int attackDmg)
    {
        if (!CanBeHit)
        {
            return;
        }

        if (_health - attackDmg <= 0)
        {
            _health = 0;
            _animator.SetTrigger("Die");
            return;
        }

        if (_health - attackDmg > 0)
        {
            _health -= attackDmg;
            _animator.SetTrigger("Hit");
        }
    }

    public void Death()
    {
        healthBar.SetActive(false);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void EnemyFlip()
    {
        if (transform.rotation.y != 0)
        {
            return;
        }
        healthBar.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
