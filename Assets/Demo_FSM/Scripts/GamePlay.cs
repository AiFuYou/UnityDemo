using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.State.TranslateState(PlayerEvent.EVENT_JUMP);
        }
    }

    public void OnPlayerOnGround()
    {
        player.State.TranslateState(PlayerEvent.EVENT_RUN);
    }
}


