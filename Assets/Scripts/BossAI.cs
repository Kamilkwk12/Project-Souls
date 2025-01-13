using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BossAI : MonoBehaviour
{

    public enum AttackType {  
        Top, 
        Fire, 
        Combo, 
        Fire_Combo
    };

    [SerializeField] float _moveSpeed;
    float _distanceToPlayer;
    Vector3 _playerPosition;
    GameObject _player;
    int _frames;


    Animator _animator;
    Rigidbody2D _rb;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerPosition = _player.transform.position;
        _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _playerPosition = _player.transform.position;
        _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;

        Debug.Log(_distanceToPlayer);


        if (_distanceToPlayer > 100) {
            _animator.SetBool("isRunning", true);
            _rb.linearVelocity = new Vector2(_moveSpeed, 0);
            
        } else
        {
            _rb.linearVelocity = new Vector2(0, 0);
            _animator.SetBool("isRunning", false);
        }

    }
}
