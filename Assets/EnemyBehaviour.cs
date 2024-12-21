using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    HitboxManager _hitboxManager;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    PolygonCollider2D _polygonCollider;

    void Start()
    {
        GameObject _player = GameObject.FindGameObjectsWithTag("Player")[0];
        _polygonCollider = _player.GetComponent<PolygonCollider2D>();
        _hitboxManager = _player.GetComponent<HitboxManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(_hitboxManager.results);
        if (_hitboxManager.results.Count != 0 && _hitboxManager.results[0].gameObject.GetComponent<PolygonCollider2D>() == _polygonCollider)
        {
            _animator.SetTrigger("Hit");
        }
    }

}
