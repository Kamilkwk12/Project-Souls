using System;
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
    SpriteRenderer _spriteRenderer;
    Animator _animator;


    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _rollAction;
    InputAction _attackAction;

    Vector2 _lastMoveValue;
    int _dodgeCount = 0;

    int _attackDuration = 0;
    int _comboTimer = 0;
    int _attacksInCombo = 0;

    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _rollSpeed = 3f;
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] bool _isGrounded = false;
    [SerializeField] bool _canMove = true;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<CapsuleCollider2D>();
        _rollCollider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();


        _moveAction = GetComponent<PlayerInput>().actions.FindAction("Move");
        _jumpAction = GetComponent<PlayerInput>().actions.FindAction("Jump");
        _rollAction = GetComponent<PlayerInput>().actions.FindAction("Roll");
        _attackAction = GetComponent<PlayerInput>().actions.FindAction("LightAttack");
    }

    void Update()
    {
        Vector2 _moveValue = _moveAction.ReadValue<Vector2>();
        float _jumpValue = _jumpAction.ReadValue<float>();
        float _rollValue = _rollAction.ReadValue<float>();
        
        //flipping character
        if (_moveValue == new Vector2(1, 0)) {
            _spriteRenderer.flipX = false;
            _playerCollider.offset = new Vector2(0.02832752f, _playerCollider.offset.y);
        }
        if (_moveValue == new Vector2(-1, 0))
        {
            _spriteRenderer.flipX = true;
            _playerCollider.offset = new Vector2(-0.03f, _playerCollider.offset.y);
        }

        //left-right movement
        if (_moveValue != new Vector2(0, 0) && _canMove) {
            _animator.SetBool("isRunning", true);
            _rb.linearVelocity = new Vector2(_moveValue.x * _moveSpeed, _rb.linearVelocityY);
        } else {
            _animator.SetBool("isRunning", false);
        }

        //jump
        if (_isGrounded && _jumpValue != 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpForce);
        }

        //roll 
        if (_isGrounded && _animator.GetBool("isRunning") && _rollValue != 0 && _dodgeCount == 0) {
            _animator.SetBool("isRolling", true);
            _lastMoveValue = _moveValue;
            _canMove = false;
            _rollCollider.enabled = true;
            _playerCollider.enabled = false;
            _dodgeCount = 45;
            _animator.SetBool("isJumping", false);
        }

        if (_dodgeCount > 0)
        {
            _rb.linearVelocity = _lastMoveValue * _rollSpeed;
            _dodgeCount--;
        } else
        {
            _playerCollider.enabled = true;
            _rollCollider.enabled = false;
            _canMove = true;
            _animator.SetBool("isRolling", false);
        }


        //light-attack
        float _attackValue = _attackAction.ReadValue<float>();




        if (_isGrounded && _attackValue != 0 && _attackDuration == 0)
        {
            _animator.SetBool("isAttacking", true);
            _attacksInCombo++;
            _comboTimer = 70;
            _attackDuration = 50;
        }

        if (_attackDuration > 0)
        {
            _canMove = false;
            _attackDuration--;
        }
        else
        {
            _canMove = true;
            _animator.SetBool("isAttacking", false);

            if (_animator.GetBool("Attack1") == true)
            {
                _animator.SetBool("Attack1", false);
            }
        }

        if (_comboTimer > 0) { 
            _comboTimer--;
        }

        if (_comboTimer > 0 && _attackValue != 0 && _attackDuration == 0)
        {
            _animator.SetBool("isAttacking", true);
            _animator.SetBool("Attack1", true);
            _attackDuration = 35;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _isGrounded = true;
            _animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _isGrounded = false;
            _animator.SetBool("isJumping", true);
        }
    }

}
