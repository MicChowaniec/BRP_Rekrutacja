using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{

    #region singleton

    public static GUIController Instance;

    private void Awake()
    {
        DisableOnStartObject.SetActive(false);
        Instance = this;
    }

    #endregion

    [SerializeField] private GameObject DisableOnStartObject;
    [SerializeField] private RectTransform ViewsParent;
    [SerializeField] private GameObject InGameGuiObject;
    [SerializeField] private PopUpView PopUp;
    [SerializeField] private PopUpScreenBlocker ScreenBlocker;
    [SerializeField] private List<GameObject> StaticButtons = new();
    private void Start()
    {
      
            if (ScreenBlocker) ScreenBlocker.InitBlocker();
    }

    private void ActiveInGameGUI(bool active)
    {
        foreach (var sb in StaticButtons)
        {
          
                sb.SetActive(active);
            
        }
        EventSystem.current.SetSelectedGameObject(FindFirstObjectByType<Selectable>().gameObject);
        InGameGuiObject.SetActive(active);
    }

    public void ShowPopUpMessage(PopUpInformation popUpInfo)
    {
        PopUpView newPopUp = Instantiate(PopUp, ViewsParent) as PopUpView;
        newPopUp.ActivePopUpView(popUpInfo);
    }

    public void ActiveScreenBlocker(bool active, PopUpView popUpView)
    {
        if (active) ScreenBlocker.AddPopUpView(popUpView);
        else ScreenBlocker.RemovePopUpView(popUpView);
    }


    #region IN GAME GUI Clicks

    public void InGameGUIButton_OnClick(UiView viewToActive)
    {
        viewToActive.ActiveView(() => ActiveInGameGUI(true));

        ActiveInGameGUI(false);
        GameControlller.Instance.IsPaused = true;
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
    
    #endregion
}