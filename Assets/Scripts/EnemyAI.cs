using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Animator _animator;
    GameObject _player;
    Vector3 _playerPosition;
    int _frames = 0;
    float _distanceToPlayer;

    public int AttackDamage = 10;
    void Start()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindGameObjectsWithTag("Player")[0];
        _playerPosition = _player.transform.position;

        _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;
        _animator.SetFloat("playerDistance", _distanceToPlayer);
    }


    void Update()
    {
        _frames++;

        if (_frames == 20)
        {
            _playerPosition = _player.transform.position;
            _distanceToPlayer = Vector3.Distance(_playerPosition, transform.position) * 100;
            _animator.SetFloat("playerDistance", _distanceToPlayer);
            _frames = 0;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Opponent_LightAttack") == true) {
            _animator.ResetTrigger("Hit");
        }
    }
}
