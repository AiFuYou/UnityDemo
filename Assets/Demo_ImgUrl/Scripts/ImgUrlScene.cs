using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = System.Random;

public class ImgUrlScene : MonoBehaviour
{
    private string _imgUrl = "https://avatars.githubusercontent.com/u/4463961?v=4";
    private string ImgUrl
    {
        get
        {
            var rd = new Random();
            return _imgUrl + rd.Next(0, 10000);
        }
    }

    public Image imageWWW;
    public Image imageUnityWebRequest;
    public Image imageUnityWebRequestTexture;
    public Image imageHttpClient;

    public void OnBtnClk(string param)
    {
        switch (param)
        {
            case "www":
                StartCoroutine(GetTexFromWWW());
                break;
            case "unitywebrequest":
                StartCoroutine(GetTexFromUnityWebRequest());
                break;
            case "unitywebrequesttexture":
                StartCoroutine(GetTexFromUnityWebRequestTexture());
                break;
            case "httpclient":
                GetTexFromHttpClient();
                break;
        }
    }

    private async void GetTexFromHttpClient()
    {
        var clientTex = new HttpClient {Timeout = TimeSpan.FromSeconds(5)};
        HttpResponseMessage responseTex = null;
        try
        {
            await Task.Run(async () =>
            {
                Debug.Log("1");
                
                responseTex = await clientTex.GetAsync(ImgUrl);

                Debug.Log("2");
                responseTex.EnsureSuccessStatusCode(); //用来抛异常的
                var responseBody = await responseTex.Content.ReadAsByteArrayAsync();
                Debug.Log(responseBody);

                Debug.Log("3");
                if (responseTex != null && responseTex.IsSuccessStatusCode)
                {
                    //此步骤为在异步线程中调用Unity主线程，因为UnityEngine的大部份API只能在Unity主线程中调用
                    //可下载gitDemo，在Demo中查看MainThread源码
                    MainThread.RunTask(() =>
                    {
                        {
                            Debug.Log("4");
                            var tex = new Texture2D(100, 100);
                            tex.LoadImage(responseBody);

                            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                new Vector2(0.5f, 0.5f));
                            imageHttpClient.sprite = sprite;
                            imageHttpClient.SetNativeSize();
                        }
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

    IEnumerator GetTexFromUnityWebRequestTexture()
    {
        Debug.Log("1");
        using var request = UnityWebRequestTexture.GetTexture(ImgUrl);
        yield return request.SendWebRequest();
        
        Debug.Log("2");
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("3");
            var texture = DownloadHandlerTexture.GetContent(request);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            imageUnityWebRequestTexture.sprite = sprite;
            imageUnityWebRequestTexture.SetNativeSize();
            
            Debug.Log("4");
        }
    }
    
    IEnumerator GetTexFromUnityWebRequest()
    {
        Debug.Log("1");
        using var request = UnityWebRequest.Get(ImgUrl);
        yield return request.SendWebRequest();
        
        Debug.Log("2");
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.result);
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("3");
            var texture = new Texture2D(200, 200);
            texture.LoadImage(request.downloadHandler.data);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            imageUnityWebRequest.sprite = sprite;
            imageUnityWebRequest.SetNativeSize();
            
            Debug.Log("4");
        }
    }

    IEnumerator GetTexFromWWW()
    {
        Debug.Log("1");
        //WWW在后续的版本中已被弃用，不建议再继续使用
        //可以查看WWW的内部实际上是UnityWebRequest
        var imgUrlWWW = new WWW(ImgUrl);
        yield return imgUrlWWW;

        Debug.Log("2");
        if (imgUrlWWW.error != null)
        {
            Debug.Log("error\t" + imgUrlWWW.error);
            yield return null;
        }
        
        var sp = Sprite.Create(imgUrlWWW.texture, new Rect(0, 0, imgUrlWWW.texture.width, imgUrlWWW.texture.height), Vector2.zero);
        imageWWW.sprite = sp;
        imageWWW.SetNativeSize();
        Debug.Log("3");
    }
}
