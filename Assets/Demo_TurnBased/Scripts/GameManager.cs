using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject[] _players;
    private GameObject[] _enemies;
    private List<GameObject> _player;
    private List<GameObject> _enemy;

    private List<GameObject> _team;
    private int _index;

    public List<GameObject> Player
    {
        set => _player = value;
        get => _player;
    }

    public List<GameObject> Enemy
    {
        set => _enemy = value;
        get => _enemy;
    }

    public List<GameObject> Team
    {
        set => _team = value;
        get => _team;
    }

    public int Index
    {
        set => _index = value;
        get => _index;
    }

    public GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        _player = new List<GameObject>();
        _enemy = new List<GameObject>();
        
        foreach (var t in _players)
        {
            _player.Add(t);
        }

        foreach (var t in _enemies)
        {
            _enemy.Add(t);
        }

        _team = _player;
        _index = 0;
        
        go = _enemy[_index];
        
        Announce(go);
    }

    public void Announce(GameObject go)
    {
        if (_team.Count == 0)
        {
            if (Player.Count == 0)
            {
                Debug.Log("Enemy战胜");   
            }
            else
            {
                Debug.Log("Player战胜");
            }

            return;
        }
        
        _team[_index].SendMessage("Action", go, SendMessageOptions.DontRequireReceiver);
        _team[_index].GetComponent<ObjectControl>().CanMove = true;
    }
}
