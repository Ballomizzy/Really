using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Useful UI Elements")]
    [SerializeField]
    private TextMeshProUGUI timerTextUI;
    [SerializeField]
    private TextMeshProUGUI levelTextUI;
    [SerializeField]
    private Slider levelDifficulty;
    [SerializeField]
    public Image panicBar;
    [SerializeField]
    public GameObject panicBarUI;
    [SerializeField]
    public Image healthBarUI;
    [SerializeField]
    public TextMeshProUGUI enemyAmount;

    [Space]
    [Header("UI States")]
    [SerializeField]
    private List<UIStateClass> UIStates = new List<UIStateClass>();

    private string youtubeLink = "https://www.youtube.com/channel/UCeN8B9-Hp33c545sIOlQ8bQ";

    [System.Serializable]
    private class UIStateClass
    {
        public string name;
        public GameObject UIGameObject;
    }

    public enum UIState
    {
        MainMenuUI,
        GameUI,
        WinUI,
        LoseUI
    }
    private UIState currentUIState;

    public void SwitchUI(UIState newUIState)
    {
        currentUIState = newUIState;

        for(int i = 0; i < UIStates.Count; i++)
        {
            if (UIStates[i].name == currentUIState.ToString())
            {
                UIStates[i].UIGameObject.SetActive(true);
            }
            else
                UIStates[i].UIGameObject.SetActive(false);
        }
    }

    public void GoToUI(string _name)
    {
        for (int i = 0; i < UIStates.Count; i++)
        {
            if (UIStates[i].name == _name)
            {
                UIStates[i].UIGameObject.SetActive(true);
            }
            else
                UIStates[i].UIGameObject.SetActive(false);
        }
        SceneManager.LoadScene("Game");
    }

    public void UpdateTimerUI(float newTime)
    {
        timerTextUI.text = ((int)newTime).ToString();
    }

    public void UpdateLevelText(int newLevel)
    {
        levelTextUI.text = "LEVEL " + newLevel.ToString();
    }

    private void FlashTimer()
    {
        //FlashTimer Anim
    }

    public int GetSelectedLevel()
    {
        return (int)levelDifficulty.value;
    }
    
    public void Exit()
    {
        Application.Quit();
    }
    
    public void OpenMikeYT()
    {
        Application.OpenURL(youtubeLink);
    }

}
