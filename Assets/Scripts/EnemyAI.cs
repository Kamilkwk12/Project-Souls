using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Animator _animator;
    Rigidbody2D _rb;
    EnemyStatus _enemyStatus;
    GameObject _player;
    Vector3 _playerPosition;
    int _frames = 0;
    float _distanceToPlayer;
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] float _attackDistance = 60;

    void Start()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerPosition = _player.transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;
        _animator.SetFloat("playerDistance", _distanceToPlayer);
        _enemyStatus = GetComponent<EnemyStatus>();
    }


    void Update()
    {
        if (_enemyStatus.Health <= 0)
        {
            return;
        }

        _frames++;

        CheckForPlayer();

        if (_frames == 10)
        {
            _playerPosition = _player.transform.position;
            _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;
            _animator.SetFloat("playerDistance", _distanceToPlayer);
            _frames = 0;
        }

        if (_distanceToPlayer >= _attackDistance && _distanceToPlayer < 200)
        {
            _animator.SetBool("isWalking", true);
            if (transform.rotation.y == 0)
            {
                _rb.linearVelocity = new Vector2(_moveSpeed, _rb.linearVelocityY);
            }
            else
            {
                _rb.linearVelocity = new Vector2(_moveSpeed * -1, _rb.linearVelocityY);
            }
        }
        else { 
            _animator.SetBool("isWalking", false);
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocityY);

        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Opponent_LightAttack") == true) {
            _animator.ResetTrigger("Hit");
        }
    }

    private void CheckForPlayer()
    {

        if (_animator.GetBool("isAttacking") == true)
        {
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
