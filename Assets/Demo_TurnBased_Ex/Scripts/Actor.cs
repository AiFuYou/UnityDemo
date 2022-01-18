using UnityEngine;

public class Actor : MonoBehaviour
{
    public const int Status_Idle = 0;//待机
    public const int Status_MoveToTarget = 1;//走向敌人
    public const int Status_Attack = 2;//攻击
    public const int Status_GoBack = 3;//回到初始点

    private Vector3 _oriPos;//原始位置
    private Vector3 _oriRot;//原始角度
    private int _attack;//攻击力
    private int _hp;//血量
    private int _status;//当前状态

    private GameObject _targetGo;
    private BattleManager _bm;

    public void SetTargetGo(GameObject go)
    {
        _targetGo = go;
        _status = Status_MoveToTarget;
        Debug.Log(gameObject.name + " 设置攻击对象 " + _targetGo.name + " 并向其移动 ");
    }
    
    void Awake()
    {
        _bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        _oriPos = transform.position;
        _attack = 50;
        _hp = 100;
        _status = Status_Idle;
    }

    void Update()
    {
        switch (_status)
        {
            case Status_MoveToTarget:
                MoveToTargetPos();
                break;
            case Status_GoBack:
                GoBack();
                break;
            case Status_Attack :
                Attack();
                break;
        }
    }

    private void Idle()
    {
        Debug.Log(gameObject.name + " 播放待机动画 ");
        
        var t = transform;
        t.position = _oriPos;
        t.eulerAngles = _oriRot;
    }
    
    private void Attack()
    {
        Debug.Log(gameObject.name + " 播放攻击动画");
        Debug.Log(gameObject.name + " 攻击 " + _targetGo.name + " 完成 ");

        _targetGo.GetComponent<Actor>().Damage(_attack);
        _status = Status_GoBack;
        Debug.Log(gameObject.name + " 返回 ");
    }

    private void Damage(int damage)
    {
        if (_hp > 0)
        {
            Debug.Log(gameObject.name + " 当前血量： " + _hp);
            Debug.Log(gameObject.name + " 掉血量： " + damage);
            
            _hp -= damage;
            if (_hp <= 0)
            {
                Debug.Log(gameObject.name + " 死亡 ");
                _bm.Die(gameObject);
            }
            else
            {
                Debug.Log(gameObject.name + " 受伤 ");
                Debug.Log(gameObject.name + " 剩余血量 " + _hp);
            }
        }
    }

    private void MoveToTargetPos()
    {
        if (Vector3.Distance(transform.position, _targetGo.transform.position) > 1)//1为与敌人的距离
        {
            var tmpPos = _targetGo.transform.position;
            tmpPos.y = transform.position.y;
            transform.LookAt(tmpPos);
            transform.Translate(Vector3.forward * Time.deltaTime * 5);
        }
        else
        {
            _status = Status_Attack;
        }
    }

    private void GoBack()
    {
        if (Vector3.Distance(transform.position, _oriPos) > 0.1)//0.1为与原始位置的距离
        {
            var tmpPos = _oriPos;
            tmpPos.y = transform.position.y;
            transform.LookAt(tmpPos);
            transform.Translate(Vector3.forward * Time.deltaTime * 5);
        }
        else
        {
            _status = Status_Idle;
            Idle();
            _bm.ChangeNext();
        }
    }
}
