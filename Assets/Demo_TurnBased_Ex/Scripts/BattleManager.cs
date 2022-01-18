using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<GameObject> _players;
    private List<GameObject> _enemies;
    private List<GameObject> _team;
    private int _curIdx;

    // Start is called before the first frame update
    void Awake()
    {
        _players = GameObject.FindGameObjectsWithTag("Player").ToList();
        _enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        
        _team = _players;
    }

    private void Start()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        var targetGo = _team == _players ? _enemies[0] : _players[0];
        _team[_curIdx].GetComponent<Actor>().SetTargetGo(targetGo);
    }

    public void ChangeNext()
    {
        if (_curIdx < _team.Count - 1)//换下一个队友攻击
        {
            _curIdx++;
        }
        else//换另一个队伍攻击
        {
            _curIdx = 0;
            _team = _team == _players ? _enemies : _players;
        }

        if (_team.Count <= 0)
        {
            Debug.Log(_team == _players ? "Cube获胜！" : "Sphere获胜！");
            return;
        }
        
        StartBattle();
    }

    public void Die(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            _players.Remove(go);
        }
        else
        {
            _enemies.Remove(go);
        }
        
        Destroy(go);
    }
}
