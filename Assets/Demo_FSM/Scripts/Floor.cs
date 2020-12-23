using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private float _floorMoveSpeed = 0.01f;
    private bool _isMoving;
    
    private Material _floorMat;
    private Vector2 _offsetMat = Vector2.zero;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    // Start is called before the first frame update
    void Start()
    {
        _floorMat = GetComponent<SpriteRenderer>().material;
        _isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            _offsetMat.x += _floorMoveSpeed;
            if (_offsetMat.x >= 1)
            {
                _offsetMat.x = 0;
            }

            _floorMat.SetTextureOffset(MainTex, _offsetMat);
        }
    }
}
