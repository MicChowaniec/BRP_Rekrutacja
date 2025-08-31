using System;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    [Header("UI VIEW elements")]
    [SerializeField] private bool UnpauseOnClose = false;
    [SerializeField] private bool CloseOnNewView = true;
    [SerializeField] private Button BackButon;

    private UiView _parentView;

    public virtual void Awake()
    {
        if (BackButon != null)
        {
            BackButon.onClick.AddListener(() => DisableView_OnClick(this));
        }
        else
        {
            Debug.LogWarning($"[UiView] BackButon is NOT assigned in {gameObject.name}");
        }
    }

    public void ActiveView_OnClick(UiView viewToActive)
    {
        if (viewToActive == null) return;

        viewToActive.SetParentView(this);
        viewToActive.ActiveView();
        ActiveView(!CloseOnNewView);
    }

    private void DisableView_OnClick(UiView viewToDisable)
    {
        viewToDisable?.DisableView();
    }

    public void DestroyView_OnClick(UiView viewToDisable)
    {
        viewToDisable?.DestroyView();
    }

    public void SetParentView(UiView parentView)
    {
        _parentView = parentView;
    }

    public void ActiveView(bool active)
    {
        gameObject.SetActive(active);
    }

    public void ActiveView(Action onBackButtonAction = null)
    {
        if (onBackButtonAction != null && BackButon != null)
        {
            BackButon.onClick.AddListener(() => onBackButtonAction());
        }

        if (!gameObject.activeSelf)
        {
            ActiveView(true);
        }
    }

    public void DisableView()
    {
        _parentView?.ActiveView();

        if (UnpauseOnClose && GameControlller.Instance != null)
        {
            GameControlller.Instance.IsPaused = false;
        }

        ActiveView(false);
    }

    public void DestroyView()
    {
        _parentView?.ActiveView();
        Destroy(gameObject);
    }

    public void DisableBackButton()
    {
        if (BackButon != null)
            BackButon.gameObject.SetActive(false);
    }

    public Button GetBackButton()
    {
        return BackButon;
    }
}
