using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GamePlay gamePlay;
    
    private bool _isRunning;
    private Vector3 _playerRotation = Vector3.zero;
    private Quaternion _quaternion = Quaternion.Euler(0, 0, 0);
    private readonly float _playerRotationZSpeed = 5f;
    private readonly Vector2 _jumpV = new Vector2(0, 30);
    private FSM _playerState = new FSM();
    private void Start()
    {
        _playerState.AddState(PlayerState.Run);
        _playerState.AddState(PlayerState.Jump);
        _playerState.AddState(PlayerState.DoubleJump);
        
        _playerState.AddTranslateState(PlayerEvent.EVENT_JUMP, PlayerState.Run, PlayerState.Jump, o =>
        {
            Jump();
        });
        
        _playerState.AddTranslateState(PlayerEvent.EVENT_JUMP, PlayerState.Jump, PlayerState.DoubleJump, o =>
        {
            DoubleJump();
        });
        
        _playerState.AddTranslateState(PlayerEvent.EVENT_RUN, PlayerState.Jump, PlayerState.Run, o =>
        {
            Run();
        });

        _playerState.AddTranslateState(PlayerEvent.EVENT_RUN, PlayerState.DoubleJump, PlayerState.Run, o =>
        {
            Run();
        });
        
        _playerState.StartState(PlayerState.Jump);
        State = _playerState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            _playerRotation.z -= _playerRotationZSpeed;
            _quaternion.eulerAngles = _playerRotation;
            transform.rotation = _quaternion;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gamePlay.OnPlayerOnGround();
    }

    public void Jump()
    {
        _isRunning = false;
        GetComponent<Rigidbody2D>().velocity = _jumpV;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Run()
    {
        _isRunning = true;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void DoubleJump()
    {
        Jump();
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public FSM State
    {
        get => _playerState;
        private set => _playerState = value;
    }
}

public class PlayerState
{
    public const string Run = "Run";
    public const string Jump = "Jump";
    public const string DoubleJump = "DoubleJump";
}

public class PlayerEvent
{
    public const string EVENT_JUMP = "EVENT_JUMP";
    public const string EVENT_RUN = "EVENT_RUN";
}
