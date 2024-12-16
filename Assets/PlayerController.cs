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
    public bool isCharacterFlippedX;

    Vector2 _moveValue;
    Vector2 _lastMoveValue = new Vector2(0, 0);

    //int _comboTimer = 0;

    float _rollTimer;
    float _attackTimer;
    //int _attacksInCombo = 0;

    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _rollSpeed = 3f;
    [SerializeField] float _jumpForce = 10f;

    bool _isGrounded = false;

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
        };
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


        if (_animator.GetBool("isRolling") == false && _animator.GetBool("isAttacking") == false)
        {
            MovementHandler();
            IsCharacterFlipped();
        }

        RollHandler();

        if (_attackTimer > 0)
        {
            _attackTimer--;
        }
        else
        {
            _animator.SetBool("isAttacking", false);
        }

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
        
        if (_moveValue != new Vector2(0, 0))
        {
            _animator.SetBool("isRunning", true);
            _rb.linearVelocity = new Vector2(_moveValue.x * _moveSpeed, _rb.linearVelocityY);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    public bool IsCharacterFlipped()
    {
        if (_moveValue == new Vector2(1, 0))
        {
            _spriteRenderer.flipX = false;
            _playerCollider.offset = new Vector2(0.02832752f, _playerCollider.offset.y);
            isCharacterFlippedX = false;
        }
        if (_moveValue == new Vector2(-1, 0))
        {
            _spriteRenderer.flipX = true;
            _playerCollider.offset = new Vector2(-0.03f, _playerCollider.offset.y);
            isCharacterFlippedX = true;
        }

        return isCharacterFlippedX;
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
            _animator.SetBool("isRolling", false);
        }
    }

    private void TriggerLightAttack()
    {
        if (_isGrounded && _animator.GetBool("isRolling") == false)
        {
            Debug.Log("Light Attack");
            _animator.SetBool("isAttacking", true);
            _animator.SetBool("isRunning", false);

            _attackTimer = 60f;
        }
    }
}
