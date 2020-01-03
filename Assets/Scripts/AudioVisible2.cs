using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisible2 : MonoBehaviour
{
    AudioSource audio;//声源
    float[] samples = new float[128];//存放频谱数据的数组长度
    LineRenderer linerenderer;//画线
    public GameObject cube;//cube预制体
    Transform[] cubeTransform;//cube预制体的位置
    Vector3 cubePos;//中间位置，用以对比cube位置与此帧的频谱数据
	// Use this for initialization
	void Start () 
    {
        GameObject tempCube;
        audio = GetComponent<AudioSource>();//获取声源组件
        linerenderer = GetComponent<LineRenderer>();//获取画线组件
        linerenderer.positionCount = samples.Length;//设定线段的片段数量
        cubeTransform = new Transform[samples.Length];//设定数组长度
        //将脚本所挂载的gameobject向左移动，使得生成的物体中心正对摄像机
        transform.position = new Vector3(-samples.Length * 0.5f, transform.position.y, transform.position.z);
        //生成cube，将其位置信息传入cubeTransform数组，并将其设置为脚本所挂载的gameobject的子物体
        for (int i = 0; i < samples.Length; i++)
        {
            tempCube=Instantiate(cube,new Vector3(transform.position.x+i,transform.position.y,transform.position.z),Quaternion.identity);
            cubeTransform[i] = tempCube.transform;
            cubeTransform[i].parent = transform;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        //获取频谱
        audio.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        //循环
        for (int i = 0; i < samples.Length; i++)
        {
            //根据频谱数据设置中间位置的的y的值，根据对应的cubeTransform的位置设置x、z的值
            //使用Mathf.Clamp将中间位置的的y限制在一定范围，避免过大
            //频谱时越向后越小的，为避免后面的数据变化不明显，故在扩大samples[i]时，乘以50+i * i*0.5f
            cubePos.Set(cubeTransform[i].position.x, Mathf.Clamp(samples[i] * ( 50+i * i*0.5f), 0, 100), cubeTransform[i].position.z);
            //画线，为使线不会与cube重合，故高度减一
            linerenderer.SetPosition(i, cubePos-Vector3.up);
            //当cube的y值小于中间位置cubePos的y值时，cube的位置变为cubePos的位置
            if (cubeTransform[i].position.y < cubePos.y)
            {
                cubeTransform[i].position = cubePos;
                
            }
            //当cube的y值大于中间位置cubePos的y值时，cube的位置慢慢向下降落
            else if (cubeTransform[i].position.y > cubePos.y)
            {
                cubeTransform[i].position -= new Vector3(0, 0.5f, 0);
            }
        }
	}
}
