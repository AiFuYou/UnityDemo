public class PanelViewModel : ViewModelBase
{
    private string _inputTextAccount;
    private string _inputTextPassword;

    public string InputTextAccount
    {
        get => _inputTextAccount;
        set
        {
            _inputTextAccount = value;
            OnPropertyChanged(this, "InputTextAccount");
        }
    }

    public string InputTextPassword
    {
        get => _inputTextPassword;
        set
        {
            _inputTextPassword = value;
            OnPropertyChanged(this, "InputTextPassword");
        }
    }
}
