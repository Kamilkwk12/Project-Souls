using System.Threading;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    [SerializeField] public float Health;
    [SerializeField] float _maxHealth = 100;
    Animator _animator;

    public int AttackDamage = 10;

    [SerializeField] Transform _healthbarValue;
    public GameObject healthBar;

    public bool CanBeHit = true;
    public bool isDead = false;
    void Start()
    {
        Health = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    
    private void Update()
    {
        _healthbarValue.localScale = new Vector3(Health / _maxHealth, 1, 1);

        EnemyFlip();

    }

    public void TakeDamage(int attackDmg)
    {
        if (!CanBeHit)
        {
            return;
        }

        if (Health - attackDmg <= 0 && isDead == false)
        {
            Health = 0;
            _animator.SetTrigger("Die");
            return;
        }

        if (Health - attackDmg > 0)
        {
            Health -= attackDmg;
            _animator.SetTrigger("Hit");
        }
    }

    public void Death()
    {
        healthBar.SetActive(false);
        GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Horizontal;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.layer = 11;
        isDead = true;
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
