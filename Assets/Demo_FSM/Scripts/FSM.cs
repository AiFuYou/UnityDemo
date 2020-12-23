/*
 * 
 *      Title:
 *          状态机/状态管理库
 *         
 *      Description:
 *          多种状态切换,比传统的 全局参数+if/switch来切换状态 更整洁和易扩展
 *          1.简易状态控制:通过添加状态名称,回调方法. 调用改变状态方法来回调函数和参数.
 *          2.状态机控制,通过添加几种转换的模式,再通过转换行为来找到转换后的状态并执行方法.
 *      Date:
 *          2018年1月18日 12:48:13
 *        
 *      Modify Recoder:
 *          1.0
 *        
 */

using UnityEngine;
using System.Collections.Generic;

public class XYState
{
    #region 参数
    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="param"></param>
    public delegate void XYCallbackFunc(params object[] param);
    /// <summary>
    /// 状态名称
    /// </summary>
    public string State { get; private set; }

    /// <summary>
    /// 存储状态回调参数的字典
    /// </summary>
    private readonly Dictionary<string, XYSimpleStateModel> _simpleStateDict = new Dictionary<string, XYSimpleStateModel>();

    /// <summary>
    /// 存储状态机转换StateModel用字典
    /// </summary>
    private readonly Dictionary<string, XYTranslationStateModel> _translateStateDict = new Dictionary<string, XYTranslationStateModel>();

    #endregion


    #region Model
    /// <summary>
    /// 记录状态名称,和转换XYTranslation对象
    /// </summary>
    class XYTranslationStateModel
    {
        private string _name;

        public XYTranslationStateModel(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 记录状态名和转换状态对象的字典
        /// </summary>
        public readonly Dictionary<string, XYTranslateModel> TranslationDict = new Dictionary<string, XYTranslateModel>();
    }

    /// <summary>
    /// 简易回调数据模型
    /// </summary>
    public class XYSimpleStateModel
    {
        public string State;
        public XYCallbackFunc CallbackFunc;
        public XYSimpleStateModel(string state, XYCallbackFunc callBackFunc)
        {
            State = state;
            CallbackFunc = callBackFunc;
        }
    }

    /// <summary>
    /// 状态转换数据模型
    /// </summary>
    public class XYTranslateModel
    {
        public string FromState;
        public string Name;
        public string ToState;
        public XYCallbackFunc OnTranslationCallback;// 回调函数

        public XYTranslateModel(string fromState,string name,string toState,XYCallbackFunc onTranslationCallback)
        {
            FromState = fromState;
            Name = name;
            ToState = toState;
            OnTranslationCallback = onTranslationCallback;
        }
    }


    #endregion


    #region 状态控制方法
    /// <summary>
    /// 添加一个新状态
    /// </summary>
    /// <param name="name">状态名称</param>
    public void AddState(string name)
    {
        _translateStateDict[name] = new XYTranslationStateModel(name);
    }

    /// <summary>
    /// 设定最开始的状态
    /// </summary>
    /// <param name="name"></param>
    public void StartState(string name)
    {
        State = name;
    }

    /// <summary>
    /// 添加一种状态,和回调方法
    /// </summary>
    /// <param name="state">状态名称</param>
    /// <param name="callbackFunc">回调方法</param>
    /// <param name="param">回调参数</param>
    public void AddSimpleState(string state, XYCallbackFunc callbackFunc)
    {
        _simpleStateDict[state] = new XYSimpleStateModel(state, callbackFunc);
    }

    /// <summary>
    /// 添加一种转换状态监听
    /// </summary>
    /// <param name="fromState">原来是那种状态</param>
    /// <param name="name">转换行为名称</param>
    /// <param name="toState">转换为那种状态</param>
    /// <param name="callbackFunc">回调方法</param>
    public void AddTranslateState(string fromState,string name,string toState, XYCallbackFunc callbackFunc)
    {
        _translateStateDict[fromState].TranslationDict[name] = new XYTranslateModel(fromState, name, toState, callbackFunc);
    }


    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="state">状态名</param>
    /// <param name="param">参数</param>
    public void ChangeState(string state,params object[] param)
    {
        if (_simpleStateDict.ContainsKey(state) == false)
        {
            Debug.LogError("未找到这个状态");
            return;
        }
        XYSimpleStateModel simple = _simpleStateDict[state];
        simple.CallbackFunc(param);
        State = state;
    }

    /// <summary>
    /// 通过传入事件名,自动转换状态
    /// </summary>
    /// <param name="name">转换事件名称</param>
    /// <param name="param">参数</param>
    public void TranslateState(string name,params object[] param)
    {
        if (State == null)
        {
            Debug.LogError("未赋值初始状态");
            return;
        }
        if (!_translateStateDict[State].TranslationDict.ContainsKey(name))
        {
              Debug.LogError("未找到这个转换状态");
            return;
        }
        XYTranslateModel tempTranslation = _translateStateDict[State].TranslationDict[name];
        tempTranslation.OnTranslationCallback(param);
        State = tempTranslation.ToState;
       
    }
    #endregion

    /// <summary>
    /// 清理这个实例的存储
    /// </summary>
    public void Clear()
    {
        _translateStateDict.Clear();
    }

}//class_end
