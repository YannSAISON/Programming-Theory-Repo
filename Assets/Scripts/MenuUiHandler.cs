using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUiHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInput;

    public void ChangeScene()
    {
        if (MainManager.Instance.PlayerName.Length == 0)
            return;
        
        SceneManager.LoadScene(1);
    }

    public void SetPlayerName(string newName)
    {
        if (_nameInput.text.Length == 0)
            return;
        MainManager.Instance.PlayerName = _nameInput.text;
    }
}
