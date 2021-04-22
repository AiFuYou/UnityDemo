using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

public class MainThread
{
    private static GameObject _mainThreadGameObject;
    private static Thread _mainThread;
    private static MainThreadComponent _taskExecutor;
    
    static MainThread()
    {
        var curThread = Thread.CurrentThread;
        if (curThread.ManagedThreadId > 1 || curThread.IsThreadPoolThread)
        {
            throw new Exception("Initialize the class in main thread please");
        }

        if (_mainThread == null)
        {
            _mainThread = curThread;
        }
        
        if (_mainThreadGameObject == null)
        {
            _mainThreadGameObject = new GameObject("MainThreadGameObject");
            _mainThreadGameObject.hideFlags = HideFlags.HideAndDontSave;
            _taskExecutor = _mainThreadGameObject.AddComponent<MainThreadComponent>();
            
            if (Application.isPlaying)
            {
                Object.DontDestroyOnLoad(_mainThreadGameObject);
            }
        }
        
        Debug.Log("MainThread()");
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Initialize()
    {
        Debug.Log("MainThread Initialize ok");
    }
    
    public static void RunTask(Action act)
    {
        if (IsMainThread())
        {
            act.Invoke();
        }
        else
        {
            var runner = new ActionRunner(act);
            _taskExecutor.AddAct(runner);
            runner.Wait();
        }
    }
    
    public static void AddTask(Action act)
    {
        if (IsMainThread())
        {
            act.Invoke();
        }
        else
        {
            var runner = new ActionRunner(act);
            _taskExecutor.AddAct(runner);
        }
    }

    private static bool IsMainThread()
    {
        return Thread.CurrentThread == _mainThread;
    }
}

[DefaultExecutionOrder(-100)]
internal class MainThreadComponent : MonoBehaviour
{
    private readonly List<ActionRunner> _listActRunner = new List<ActionRunner>();

    public void AddAct(ActionRunner ar)
    {
        lock (_listActRunner)
        {
            _listActRunner.Add(ar);
        }
    } 
    
    private void Update()
    {
        lock (_listActRunner)
        {
            while (_listActRunner.Count > 0)
            {
                var ar = _listActRunner[0];
                _listActRunner.RemoveAt(0);
                
                try
                {
                    ar.Run();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}

internal class ActionRunner
{
    private readonly ManualResetEventSlim _manualResetEventSlim = new ManualResetEventSlim(false);
    private Action _act;

    public ActionRunner(Action act)
    {
        _act = act;
    }

    public void Run()
    {
        try
        {
            _act?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        
        _manualResetEventSlim?.Set();
    }

    public void Wait()
    {
        _manualResetEventSlim?.Wait();
    }
}