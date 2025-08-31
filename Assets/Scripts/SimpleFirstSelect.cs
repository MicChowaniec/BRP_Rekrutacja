using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleFirstSelect : MonoBehaviour
{
    [SerializeField] private GameObject firstSelect;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelect);
    }
}
