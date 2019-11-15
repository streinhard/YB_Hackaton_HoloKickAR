using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private TMP_InputField _inputField;

    [SerializeField]
    private PlayerInformation playerInformation;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputField.onValueChanged.AddListener(ChangedInputValue);
        startButton.interactable = false;
        startButton.onClick.AddListener(GoToPlacementScreen);
    }

    private void ChangedInputValue(string value)
    {
        startButton.interactable = value.Length > 0;
    }

    private void GoToPlacementScreen()
    {
        playerInformation.PlayerName = _inputField.text;
        SceneManager.LoadScene("ARScene");
    }
}
