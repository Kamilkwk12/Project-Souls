using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BossAI : MonoBehaviour
{
    //AttackTypes:
    //0 - none
    //1 - attack
    //2 - normal combo attack
    //3 - fire attack
    //4 - fire combo

    BossStatus _bossStatus;

    [SerializeField] float _moveSpeed;
    float _distanceToPlayer;
    Vector3 _playerPosition;
    GameObject _player;

    public enum AttackType
    {
        None,
        Attack = 10, 
        Combo = 15,
        FireAttack = 25, 
        FireCombo = 20,
    }

    public int AttackDmg;

    [SerializeField] float _distanceToAttack;

    public int AttackNumber = 0;

    Animator _animator;
    Rigidbody2D _rb;
    void Start()
    {
        _bossStatus = GetComponent<BossStatus>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerPosition = _player.transform.position;
        _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _animator.SetInteger("AttackType", 0);
    }

    void Update()
    {
        _playerPosition = _player.transform.position;
        _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;

        _animator.SetFloat("Distance", _distanceToPlayer);
        
        CheckForPlayer();

        if (_distanceToPlayer >= _distanceToAttack - 20 && _animator.GetBool("isAttacking") == false)
        {
            _animator.SetBool("isRunning", true);
            if (transform.rotation.y == 0)
            {
                _rb.linearVelocity = new Vector2(_moveSpeed, 0);
            }
            else
            {
                _rb.linearVelocity = new Vector2(_moveSpeed * -1, 0);
            }

        }
        else
        {
            _rb.linearVelocity = new Vector2(0, 0);
            _animator.SetBool("isRunning", false);
        }

        if (_distanceToPlayer < _distanceToAttack && _animator.GetBool("isRunning") == false && _animator.GetBool("isAttacking") == false)
        {

            if (AttackNumber == 2 && _bossStatus.SecondStage == false)
            {
                AttackDmg = (int)AttackType.Combo;
                _animator.SetTrigger("Combo1");
                _animator.SetInteger("AttackType", 2);
                AttackNumber = 0;
                return;
            }

            AttackDmg = (int)AttackType.Attack;
            _animator.SetTrigger("Attack");
            _animator.SetInteger("AttackType", 1);


        }
    }

    private void CheckForPlayer()
    {

        if (_animator.GetBool("isAttacking") == true) {
            return;
        }

        if (_player.GetComponent<Transform>().position.x - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
