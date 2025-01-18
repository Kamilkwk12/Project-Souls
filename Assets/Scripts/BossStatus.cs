using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : MonoBehaviour
{

    int _health;
    [SerializeField] int _maxHealth;

    [SerializeField] public Slider HealthBar;

    public bool CanBeHit = true;
    public bool isDead = false;

    public bool SecondStage = false;

    public int AttackDamage = 25;
    Animator _animator;
    void Start()
    {
        _health = _maxHealth;
        HealthBar.maxValue = _health;
        HealthBar.value = _health;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        HealthBar.value = _health;

        if (_health < _maxHealth / 2)
        {
            SecondStage = true;
        }

    }

    public void TakeDamage(int damage) {

        if (!CanBeHit) {
            return;
        }

        if (_health - damage <= 0 && isDead == false)
        {
            _health = 0;
            Death();
            return;
        }

        if (_health - damage > 0)
        {
            _health -= damage;
        }
    }

    private void Death()
    {
        _animator.SetTrigger("Die");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.layer = 11;
        GetComponent<BossAI>().enabled = false;
        isDead = true;
    }
}
