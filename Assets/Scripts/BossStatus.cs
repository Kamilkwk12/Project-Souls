using UnityEngine;
using UnityEngine.UI;

public class BossStatus : MonoBehaviour
{

    int _health;
    [SerializeField] int _maxHealth;

    [SerializeField] Slider _healthBar;

    public bool CanBeHit = true;
    public bool SecondStage = false;

    public int AttackDamage = 25;

    void Start()
    {
        _health = _maxHealth;
        _healthBar.maxValue = _health;
        _healthBar.value = _health;

    }

    void Update()
    {
        _healthBar.value = _health;
    }

    public void TakeDamage(int damage) {

        if (!CanBeHit) {
            return;
        }

        if (_health - damage <= 0)
        {
            _health = 0;
            return;
        }

        if (_health - damage > 0)
        {
            _health -= damage;
        }

    }
}
