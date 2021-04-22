using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class ImgDownloadScene : MonoBehaviour
{
    public Text coroutineProgressText;
    public Text coroutineTimeText;
    public Button coroutineBtn;
    public Text asyncThreadProgressText;
    public Text asyncThreadTimeText;
    public Button asyncThreadBtn;
    public bool openUpdate;

    private void Update()
    {
        if (openUpdate)
        {
            var a = 1;
            for (var i = 0; i < 1000; i++)
            {
                ++a;
                PlayerPrefs.SetInt("test", a);
                PlayerPrefs.Save();
            }
        }
    }

    private void Start()
    {
        UpdateProgressAndTime("coroutine", 0, "0");
        UpdateProgressAndTime("asyncThread", 0, "0");
    }

    private Stopwatch _sw = new Stopwatch();
    private Random _rd = new Random();
    private Dictionary<int, bool> _imgRandomDictCoroutine = new Dictionary<int, bool>();
    private Dictionary<int, bool> _imgRandomDictAsyncThread = new Dictionary<int, bool>();
    private string _imgUrl = "https://avatars.githubusercontent.com/u/4463961?v=4";
    private string ImgUrlCoroutine
    {
        get
        {
            var rd = _rd.Next(0, 10000);
            if (_imgRandomDictCoroutine.ContainsKey(rd) && _imgRandomDictCoroutine[rd])
            {
                return ImgUrlCoroutine;
            }

            _imgRandomDictCoroutine[rd] = true;
            return _imgUrl + rd;
        }
    }

    private string ImgUrlAsyncThread
    {
        get
        {
            var rd = _rd.Next(0, 10000);
            if (_imgRandomDictAsyncThread.ContainsKey(rd) && _imgRandomDictAsyncThread[rd])
            {
                return ImgUrlAsyncThread;
            }

            _imgRandomDictAsyncThread[rd] = true;
            return _imgUrl + rd;
        }
    }

    public void OnBtnClk(string param)
    {
        _sw.Restart();
        switch (param)
        {
            case "coroutine":
                StartCoroutine(GetTexFromCoroutine());
                coroutineBtn.interactable = false;
                break;
            case "asyncThread":
                GetTexFromAsyncThread();
                asyncThreadBtn.interactable = false;
                break;
        }
    }
    
    private int _maxCount = 20;
    private int _curCountCoroutine = 0;
    private int _curCountAsyncThread = 0;
    
    IEnumerator GetTexFromCoroutine()
    {
        using var request = UnityWebRequest.Get(ImgUrlCoroutine);
        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            Debug.Log(request.error);
        }
        else
        {
            ++_curCountCoroutine;
            UpdateProgressAndTime("coroutine", _curCountCoroutine, _sw.ElapsedMilliseconds.ToString());
            
            if (_curCountCoroutine < _maxCount)
            {
                StartCoroutine(GetTexFromCoroutine());
            }
            else
            {
                _sw.Stop();
            }
        }
    }

    private async void GetTexFromAsyncThread()
    {
        var thread = new Thread(GetTexFromAsyncThread2);
        thread.Start();
    }

    private async void GetTexFromAsyncThread2()
    {
        var clientTex = new HttpClient {Timeout = TimeSpan.FromSeconds(5)};
        HttpResponseMessage responseTex = null;
        try
        {
            await Task.Run(async () =>
            {
                responseTex = await clientTex.GetAsync(ImgUrlAsyncThread);
                responseTex.EnsureSuccessStatusCode(); //用来抛异常的
                
                if (responseTex != null && responseTex.IsSuccessStatusCode)
                {
                    ++_curCountAsyncThread;
                    if (_curCountAsyncThread < _maxCount)
                    {
                        GetTexFromAsyncThread();
                    }
                    else
                    {
                        _sw.Stop();
                    }
                    
                    MainThread.AddTask(() =>
                    {
                        UpdateProgressAndTime("asyncThread", _curCountAsyncThread, _sw.ElapsedMilliseconds.ToString());
                    });
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            clientTex.Dispose();
            responseTex?.Dispose();
        }
    }
    
    private void UpdateProgressAndTime(string typ, int progress, string time)
    {
        switch (typ)
        {
            case "asyncThread":
                asyncThreadProgressText.text = "下载进度：" + progress;
                asyncThreadTimeText.text = "耗时：" + time + "ms";
                break;
            case "coroutine":
                coroutineProgressText.text = "下载进度：" + progress;
                coroutineTimeText.text = "耗时：" + time + "ms";
                break;
        }
    }
}
