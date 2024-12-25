using System.Collections.Generic;
using UnityEngine;

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

    void OnEnable()
    {
        _localCollider = gameObject.AddComponent<PolygonCollider2D>();
        _localCollider.isTrigger = true;
        _localCollider.pathCount = 0; 
    }

    private void Update()
    {
        _colliders = new PolygonCollider2D[] { frame1, frame2, frame3, frame4 };
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

    public void chceckHitbox()
    {
        Physics2D.OverlapCollider(_localCollider, results);
    }
}
