using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
    private TMP_Dropdown _dropdown;
    
    [SerializeField]
    private Image teamLogo;
    
    [SerializeField]
    private Sprite spriteYb;
    
    [SerializeField]
    private Sprite spriteFcb;
    
    [SerializeField]
    private Sprite spriteFcz;
    
    [SerializeField]
    private PlayerInformation playerInformation;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputField.onValueChanged.AddListener(ChangedInputValue);
        _dropdown.onValueChanged.AddListener(ChangedDropdownValue);
        startButton.interactable = false;
        startButton.onClick.AddListener(GoToPlacementScreen);
    }

    private void ChangedInputValue(string value)
    {
        startButton.interactable = value.Length > 0;
    }
    
    private void ChangedDropdownValue(int i)
    {
        if (_dropdown.captionText.text == "YB")
        {
            teamLogo.sprite = spriteYb;
        } else if (_dropdown.captionText.text == "FCB")
        {
            teamLogo.sprite = spriteFcb;
        } else if (_dropdown.captionText.text == "FCZ")
        {
            teamLogo.sprite = spriteFcz;
        }
    }

    private void GoToPlacementScreen()
    {
        playerInformation.PlayerName = _inputField.text;

        Team userTeam = Team.YB;
        if (_dropdown.captionText.text == "YB")
        {
            userTeam = Team.YB;
        } else if (_dropdown.captionText.text == "FCB")
        {
            userTeam = Team.FCB;
        } else if (_dropdown.captionText.text == "FCZ")
        {
            userTeam = Team.FCZ;
        }
        
        playerInformation.team = userTeam;
        SceneManager.LoadScene("ARScene");
    }
}
