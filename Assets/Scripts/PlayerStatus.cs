using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : MonoBehaviour
{

    [SerializeField] int _maxHealth = 100;
    int _health;
    [SerializeField] int _maxStamina = 50;
    int _stamina;

    public int AttackDamage = 15;

    Animator _animator;

    public Slider healthBar;
    public Slider staminaBar;
    float _staminaTimer = 0;
    [SerializeField] float _staminaCooldown = 2;
    bool _isStaminaResetActive = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
        _stamina = _maxStamina;
        healthBar.maxValue = _maxHealth;
        staminaBar.maxValue = _maxStamina;
    }

    void Update()
    {
        healthBar.value = _health;
        staminaBar.value = _stamina;

        if ( _stamina < _maxStamina && _staminaTimer <= 0 && !_isStaminaResetActive) {
            _staminaTimer = _staminaCooldown;
            _isStaminaResetActive = false;
        }

        if (_staminaTimer > 0) { 
            _staminaTimer -= Time.deltaTime;
            _isStaminaResetActive = true;
        }

        if (_staminaTimer < 0 && _isStaminaResetActive && _stamina < _maxStamina) {
            _stamina += 1;
        } else
        {
            _isStaminaResetActive = false;
        }

    }

    public void TakeDamage(int attackDmg)
    {
        _animator.SetTrigger("Hurt");
        if (_health - attackDmg <= 0)
        {
            _health = 0;
            return;
        }

        if (_health - attackDmg > 0)
        {
            _health -= attackDmg;
        }

        
    }

    public void TakeStamina(int cost)
    {
        if (_stamina - cost <= 0)
        {
            _stamina = 0;
            return;
        }

        if (_stamina - cost > 0)
        {
            _stamina -= cost;
        }
    }

    public void DeactivateStaminaReset()
    {
        _staminaTimer = 0;
        _isStaminaResetActive = false;
    }
}
