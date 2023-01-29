using TMPro;
using UnityEngine.UI;

public class PanelView : ViewBase
{
    public TMP_InputField inputFieldAccount;
    public TMP_InputField inputFieldPassword;
    public Button btnCancel;
    public Button btnOk;
    public TMP_Text textAccount;
    public TMP_Text textPassword;

    private PanelViewModel _vm;

    private void Awake()
    {
        _vm = new PanelViewModel();
        _vm.PropertyChanged += (sender, e) =>
        {
            switch (e.PropertyName)
            {
                case "InputTextAccount":
                    textAccount.text = _vm.InputTextAccount;
                    break;
                case "InputTextPassword":
                    textPassword.text = _vm.InputTextPassword;
                    break;
            }
        };
    }

    private void Start()
    {
        inputFieldAccount.onValueChanged.AddListener(OnInputFieldAccountValueChanged);
        inputFieldPassword.onValueChanged.AddListener(OnInputFieldPasswordValueChanged);
    }

    private void OnInputFieldAccountValueChanged(string str)
    {
        _vm.InputTextAccount = str;
    }
    
    private void OnInputFieldPasswordValueChanged(string str)
    {
        _vm.InputTextPassword = str;
    }

    private void OnDestroy()
    {
        inputFieldAccount.onValueChanged.RemoveListener(OnInputFieldAccountValueChanged);
        inputFieldPassword.onValueChanged.RemoveListener(OnInputFieldPasswordValueChanged);
    }
}