using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HitboxManager : MonoBehaviour
{

    private PolygonCollider2D _localCollider;

    private PolygonCollider2D[] _colliders;
    //flipX-false
    public PolygonCollider2D frame1;
    public PolygonCollider2D frame2;
    //flipX-true
    public PolygonCollider2D frame3;
    public PolygonCollider2D frame4;

    private PlayerController _playerController;
    public List<Collider2D> results = new List<Collider2D>();

    public enum hitBoxes
    {
        frame1Box,
        frame2Box,
        clear
    }

    void Start()
    {

        _playerController = GetComponent<PlayerController>();

        _localCollider = gameObject.AddComponent<PolygonCollider2D>();
        _localCollider.isTrigger = true;
        _localCollider.pathCount = 0; 
    }

    private void Update()
    {
        if (_playerController.isCharacterFlippedX == false)
        {
            _colliders = new PolygonCollider2D[] { frame1, frame2 };
        }
        else
        {
            _colliders = new PolygonCollider2D[] { frame3, frame4 };
        }
    }

    private void OnTriggerEnter2D()
    {
        Physics2D.OverlapCollider(_localCollider, results);
        if (results.Count > 0) { 
            Debug.Log(results.Count);
            Debug.Log(results[0].gameObject.tag);
        }
    }

    public void setHitBox(hitBoxes val)
    {
        if (val != hitBoxes.clear)
        {
            _localCollider.SetPath(0, _colliders[(int)val].GetPath(0));
            _localCollider.offset = new Vector2(0, 0);

            if (_playerController.isCharacterFlippedX == true)
            {
                _localCollider.offset = new Vector2 (-0.15f, 0);
            }
            return;
        } 
        _localCollider.pathCount = 0;
    }
}
