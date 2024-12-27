using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    [SerializeField] float _health;
    [SerializeField] float _maxHealth = 100;
    Animator _animator;

    [SerializeField] Transform _healthbarValue;
    public GameObject healthBar;

    void Start()
    { 
        _health = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int attackDmg)
    {
        if (_health - attackDmg <= 0)
        {
            _health = 0;
            _animator.SetTrigger("Die");
            return;
        }

        if (_health - attackDmg > 0) { 
            _health -= attackDmg;
            _animator.SetTrigger("Hit");
        }

    }

    public void Death()
    {
        healthBar.SetActive(false);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }


    private void Update()
    {

        //Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).IsName("Opponent_LightAttack"));

        _healthbarValue.localScale = new Vector3(_health/_maxHealth, 1, 1);
    }
}
