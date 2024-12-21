using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HitboxManager : MonoBehaviour
{

    private PolygonCollider2D _localCollider;

    private PolygonCollider2D[] _colliders;

    public PolygonCollider2D frame1;
    public PolygonCollider2D frame2;
    public PolygonCollider2D frame3;
    public PolygonCollider2D frame4;

    public List<Collider2D> results = new List<Collider2D>();

    public enum hitBoxes
    {
        frame1Box,
        frame2Box,
        frame3Box,
        frame4Box,
        clear
    }

    void Start()
    {
        _localCollider = gameObject.AddComponent<PolygonCollider2D>();
        _localCollider.isTrigger = true;
        _localCollider.pathCount = 0; 
    }

    private void Update()
    {
        _colliders = new PolygonCollider2D[] { frame1, frame2, frame3, frame4 };
    }

    private void OnTriggerEnter2D()
    {
        Physics2D.OverlapCollider(_localCollider.GetComponent<PolygonCollider2D>(), results);
        if (results.Count > 0) { 
            Debug.Log(results.Count);
            Debug.Log(results[0].gameObject.tag);
        }
    }

    private void OnTriggerExit2D()
    {
        results = new List<Collider2D>();
    }

    public void setHitBox(hitBoxes val)
    {
        if (val != hitBoxes.clear)
        {
            _localCollider.SetPath(0, _colliders[(int)val].GetPath(0));
            return;
        } 
        _localCollider.pathCount = 0;
    }
}
