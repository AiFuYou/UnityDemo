using System.Globalization;
using UnityEngine;

public class FpsShow : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float _lastInterval;
    private int _frames;
    private float _fps;
    private GUIStyle _fpsStyle;
    void Start(){
        //设置帧率
        Application.targetFrameRate = 60;
        _lastInterval = Time.realtimeSinceStartup;
        _frames = 0;
        _fpsStyle = new GUIStyle {fontSize = 100};
    }
    
    // Update is called once per frame  
    void Update(){
        ++_frames;
        var timeNow = Time.realtimeSinceStartup;
        if (timeNow >= _lastInterval + updateInterval)
        {
            _fps = _frames / (timeNow - _lastInterval);
            _frames = 0;
            _lastInterval = timeNow;
        }
    }
    
    void OnGUI(){
        GUI.Label(new Rect(50, 50, 100, 100), _fps.ToString(CultureInfo.InvariantCulture), _fpsStyle);
    }
}
