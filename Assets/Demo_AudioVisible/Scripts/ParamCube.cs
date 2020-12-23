using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour {

    public int band;
    public float startScale, scaleMultiplier;

    Material material;
    void Start () {
        material = GetComponent<MeshRenderer>().materials[0];
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (float.IsNaN(AudioVisible._audioBandBuffer[band])) return;
        transform.localScale = new Vector3(transform.localScale.x, (AudioVisible._audioBandBuffer[band]) * scaleMultiplier + startScale, transform.localScale.z);
        Color color = new Color(AudioVisible._audioBandBuffer[band], AudioVisible._audioBandBuffer[band], AudioVisible._audioBandBuffer[band]);
        material.SetColor("_EmissionColor",color);    
    }
}