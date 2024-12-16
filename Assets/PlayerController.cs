using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D _rb;
    CapsuleCollider2D _playerCollider;
    CircleCollider2D _rollCollider;
    BoxCollider2D _boxCollider;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    List<Collider2D> results = new List<Collider2D>();

    PlayerInput _playerInput;
    InputAction _moveAction;

    Vector2 _moveValue;
    Vector2 _lastMoveValue = new Vector2(0, 0);

    //int _attackDuration = 0;
    //int _comboTimer = 0;
    //int _attacksInCombo = 0;

    [SerializeField] float _rollTimer;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _rollSpeed = 3f;
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] bool _isGrounded = false;
    [SerializeField] bool _canMove = true;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Jump"].performed += context =>
        {
            Jump();
        };

        _playerInput.actions["Roll"].performed += context =>
        {
            RollTrigger();
        };

        _playerInput.actions["Light Attack"].performed += context =>
        {
            TriggerLightAttack();
            IsComboPerformed();
        };
    }

    

    private void TriggerLightAttack()
    {
        if(_isGrounded && _animator.GetBool("isRolling") == false)
        {
            _animator.SetBool("isAttacking", true);
            _canMove = false;

        }
    }

    private void IsComboPerformed()
    {
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<CapsuleCollider2D>();
        _rollCollider = GetComponent<CircleCollider2D>();
        _boxCollider = GetComponent <BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _moveAction = GetComponent<PlayerInput>().actions["Move"];
    }

    void Update()
    {
        _isGrounded = IsPlayerGrounded();
        _moveValue = _moveAction.ReadValue<Vector2>();

        MovementHandler();

        RollHandler();


        //light-attack
        //float _attackValue = _attackAction.ReadValue<float>();

        //if (_isGrounded && _attackValue != 0 && _attackDuration == 0)
        //{
        //    _animator.SetBool("isAttacking", true);
        //    _attacksInCombo++;
        //    _comboTimer = 70;
        //    _attackDuration = 50;
        //}

        //if (_attackDuration > 0)
        //{
        //    _canMove = false;
        //    _attackDuration--;
        //}
        //else
        //{
        //    _canMove = true;
        //    _animator.SetBool("isAttacking", false);

        //    if (_animator.GetBool("Attack1") == true)
        //    {
        //        _animator.SetBool("Attack1", false);
        //    }
        //}

        //if (_comboTimer > 0)
        //{
        //    _comboTimer--;
        //}

        //if (_comboTimer > 0 && _attackValue != 0 && _attackDuration == 0)
        //{
        //    _animator.SetBool("isAttacking", true);
        //    _animator.SetBool("Attack1", true);
        //    _attackDuration = 35;
        //}

    }



    private bool IsPlayerGrounded()
    {
        Physics2D.OverlapCollider(_boxCollider, results);
        if (results.Count > 0)
        {
            Collider2D _groundGameObject = results.Where(ojb => ojb.gameObject.name == "Ground").SingleOrDefault();
            _animator.SetBool("isJumping", false);
            return true;
        } else
        {
            _animator.SetBool("isJumping", true);
            return false;
        }

    }

    private void MovementHandler()
    {
        //left-right movement
        if (_moveValue != new Vector2(0, 0) && _canMove)
        {
            _animator.SetBool("isRunning", true);
            _rb.linearVelocity = new Vector2(_moveValue.x * _moveSpeed, _rb.linearVelocityY);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }

        //flipping character
        if (_moveValue == new Vector2(1, 0))
        {
            _spriteRenderer.flipX = false;
            _playerCollider.offset = new Vector2(0.02832752f, _playerCollider.offset.y);
        }
        if (_moveValue == new Vector2(-1, 0))
        {
            _spriteRenderer.flipX = true;
            _playerCollider.offset = new Vector2(-0.03f, _playerCollider.offset.y);
        }
    }

    private void Jump()
    {
        Debug.Log("Jump");
        if (_isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpForce);
        }
    }

    private void RollTrigger()
    {
        if (_isGrounded && _animator.GetBool("isRunning"))
        {
            Debug.Log("Roll");
            _animator.SetBool("isRolling", true);
            _lastMoveValue = _moveValue;
            _canMove = false;
            _rollCollider.enabled = true;
            _playerCollider.enabled = false;
            _animator.SetBool("isJumping", false);
            _rollTimer = 45f;
        }
    }

    private void RollHandler()
    {
        if (_rollTimer > 0)
        {
            _rb.linearVelocity = _lastMoveValue * _rollSpeed;
            _rollTimer--;
        }
        else
        {
            _playerCollider.enabled = true;
            _rollCollider.enabled = false;
            _canMove = true;
            _animator.SetBool("isRolling", false);
        }
    }
}
