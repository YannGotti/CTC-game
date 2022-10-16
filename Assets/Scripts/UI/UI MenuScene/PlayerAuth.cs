using UnityEngine;

public class PlayerAuth : MonoBehaviour
{
    [SerializeField] GameObject _useranamePanel;
    [SerializeField] TMPro.TMP_InputField _inputUsername;
    [SerializeField] TMPro.TMP_Text _validationMessage;

    public void EnableUsernamePanel() => _useranamePanel.SetActive(true);
    public void DisableUsernamePanel() => _useranamePanel.SetActive(false);

    public void SelectUsernameField()
    {
        string username = _inputUsername.text;
        var SqlConnector = GetComponent<MySqlConnector>();


        if (!ValidateUsername(username, SqlConnector)) return;

        SqlConnector.InsertUserData(username);

        GetComponent<SceneController>().OpenGame();
        GetComponent<SceneLoadedManager>().LoadScene();
    }

    private bool ValidateUsername(string username, MySqlConnector SqlConnector)
    {
        if (username.Length > 20)
        {
            _validationMessage.text = "���� ��� �� ������ ���� ������ 15 ��������!";
            return false;
        }

        if (username.Length < 4)
        {
            _validationMessage.text = "���� ��� �� ������ ���� ������ 4 ��������!";
            return false;
        }

        if (username.Contains('.') || username.Contains(',') || username.Contains('&') || username.Contains('!') || username.Contains('?'))
        {
            _validationMessage.text = "���� ��� �� ������ ��������� �������!";
            return false;
        }

        if (!SqlConnector.IsMacAdress(username, SqlConnector.SelectLocalMacAdress()))
        {
            _validationMessage.text = "���� ��� � ��� ������ �� ���������, ��� �� ��� �������!";
            return false;
        }

        return true;
    }

    

}
