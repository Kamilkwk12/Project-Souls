using UnityEngine;

public class EntityStatus : MonoBehaviour
{

    [SerializeField] float _health;
    [SerializeField] float _maxHealth = 100;
    Animator _animator;

    [SerializeField] Transform _healthbarValue;
    public GameObject healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int attackDmg)
    {
        if (_health - attackDmg >= 0) { 
            _health -= attackDmg;
        }

        if (_health - attackDmg < 0)
        {
            _health = 0;
            _animator.SetTrigger("Die");
        }
    }

    public void Death()
    {
        healthBar.SetActive(false);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void Update()
    {
        _healthbarValue.localScale = new Vector3(_health/_maxHealth, 1, 1);
    }
}
