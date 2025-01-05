using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public PlayerStatus PlayerStatus;

    Rigidbody2D _rb;
    CapsuleCollider2D _playerCollider;
    CircleCollider2D _rollCollider;
    BoxCollider2D _boxCollider;
    Animator _animator;
    List<Collider2D> results = new List<Collider2D>();

    PlayerInput _playerInput;
    InputAction _moveAction;

    Vector2 _moveValue;
    Vector2 _lastMoveValue = new Vector2(0, 0);


    public float RollDuration = 0.35f;
    float _rollTimer;
    public float AttackDuration = 0.35f;

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
        _animator = GetComponent<Animator>();
        PlayerStatus = GetComponent<PlayerStatus>();

        _playerInput.actions["Jump"].performed += context =>
        {
            PlayerStatus.TakeStamina(5);
            Jump();
            PlayerStatus.DeactivateStaminaReset();
        };

        _playerInput.actions["Roll"].performed += context =>
        {
            PlayerStatus.TakeStamina(10);
            RollTrigger();
            PlayerStatus.DeactivateStaminaReset();
        };

        _playerInput.actions["Light Attack"].performed += context =>
        {
            PlayerStatus.TakeStamina(15);
            PlayerStatus.DeactivateStaminaReset();
            _animator.SetTrigger("Attack");
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isAttacking", true);
        };
    }


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<CapsuleCollider2D>();
        _rollCollider = GetComponent<CircleCollider2D>();
        _boxCollider = GetComponent <BoxCollider2D>();

        _moveAction = GetComponent<PlayerInput>().actions["Move"];
    }

    void Update()
    {
        _isGrounded = IsPlayerGrounded();
        _moveValue = _moveAction.ReadValue<Vector2>();


        if (_animator.GetBool("isRolling") == false && _animator.GetBool("isAttacking") == false)
        {
            MovementHandler();
            CharacterFlip();
        }

        RollHandler();

    }
    

    private bool IsPlayerGrounded()
    {
        Physics2D.OverlapCollider(_boxCollider, results);
        if (results.Count > 0)
        {
            //Collider2D _groundGameObject = results.Where(ojb => ojb.gameObject.CompareTag("Ground")).SingleOrDefault();
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

    public void CharacterFlip()
    {
        if (_moveValue == new Vector2(1, 0))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (_moveValue == new Vector2(-1, 0))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
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
            _rollCollider.enabled = true;
            _playerCollider.enabled = false;
            _animator.SetBool("isJumping", false);
            _rollTimer = RollDuration;
        }
    }

    private void RollHandler()
    {
        if (_rollTimer > 0)
        {
            _rb.linearVelocity = _lastMoveValue * _rollSpeed;
            _rollTimer -= Time.deltaTime;
        }
        else
        {
            _playerCollider.enabled = true;
            _rollCollider.enabled = false;
            _animator.SetBool("isRolling", false);
        }
    }
}
