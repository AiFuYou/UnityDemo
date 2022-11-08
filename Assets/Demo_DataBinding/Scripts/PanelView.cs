using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelView : ViewBase
{
    public TMP_InputField inputFieldAccount;
    public TMP_InputField inputFieldPassword;
    public Button btnCancel;
    public Button btnOk;

    private PanelViewModel vm;
    private void Awake()
    {
        vm = new PanelViewModel();
        vm.PropertyChanged += (sender, e) =>
        {
            Debug.Log(sender.ToString());
            Debug.Log(e.PropertyName);
        };
    }

    private void Start()
    {
        vm.InputTextAccount = "123";
        vm.InputTextPassword = "abc";
    }
}