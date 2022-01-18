using System;
using UnityEngine;

public class ObjectControl : BaseObj
{
    private Vector3 _oriPos;
    private Vector3 _oriRot;
    private Vector3 _tmpPos;
    private bool _canMove;

    private GameManager _gm;
    
    public bool CanMove
    {
        set => _canMove = value;
        get => _canMove;
    }

    private Vector3 _targetPos;
    private float _dis;

    public int HP = 100;

    private void Awake()
    {
        _dis = 1;
        var t = transform;
        _oriPos = t.position;
        _oriRot = t.eulerAngles;
        _tmpPos = Vector3.zero;
        _canMove = false;
        _targetPos = Vector3.zero;
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (_canMove)
        {
            MoveTo(_targetPos, _dis);
        }   
    }

    protected override void Damage(int damage)
    {
        if (HP > 0)
        {
            HP -= damage;

            if (HP <= 0)
            {
                Debug.Log("死亡");
                HP = 0;

                if (gameObject.CompareTag("Player"))
                {
                    _gm.Player.Remove(gameObject);
                }
                else
                {
                    _gm.Enemy.Remove(gameObject);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("受伤");
            }
        }
    }

    protected override void MoveTo(Vector3 go, float dis)
    {
        if (Vector3.Distance(transform.position, go) > dis)
        {
            _tmpPos = go;
            _tmpPos.y = transform.position.y;
            transform.LookAt(_tmpPos);
            transform.Translate(Vector3.forward * Time.deltaTime * 5);
        }
        else
        {
            _canMove = false;
            if (dis > 0.1f)
            {
                //攻击
                Attack();
                GoBackParameter();
            }
            else
            {
                //返回到原始位置
                GoBack();
                ChangeNext();
            }
        }
    }

    private void Attack()
    {
        if (_gm.go != null)
        {
            _gm.go.GetComponent<ObjectControl>().Damage(50);
        }
    }

    private void GoBack()
    {
        var t = transform;
        t.position = _oriPos;
        t.eulerAngles = _oriRot;
        _canMove = false;
    }

    private void GoBackParameter()
    {
        _canMove = true;
        _targetPos = _oriPos;
        _dis = 0.1f;
    }

    private void ChangeNext()
    {
        //同队换人
        if (_gm.Index < _gm.Team.Count - 1)
        {
            _gm.Index++;
            _dis = 1;
            _gm.Announce(_gm.go);
        }
        else
        {
            ChangeTeam();
        }
    }

    private void Action(GameObject go)
    {
        _targetPos = go.transform.position;
    }

    private void ChangeTeam()
    {
        _gm.Index = 0;
        _dis = 1;

        if (_gm.Team == _gm.Player)
        {
            _gm.Team = _gm.Enemy;
            _gm.go = _gm.Player[0];
            
        } else if (_gm.Team == _gm.Enemy)
        {
            _gm.Team = _gm.Player;
            _gm.go = _gm.Enemy[0];
        }
        
        _gm.Announce(_gm.go);
    }
}
