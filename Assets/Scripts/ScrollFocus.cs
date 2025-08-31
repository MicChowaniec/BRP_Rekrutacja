using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollFocus : MonoBehaviour
{
    public ScrollRect scrollRect;

    void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != null && current.transform.IsChildOf(scrollRect.content))
        {
            ScrollToShow(current.GetComponent<RectTransform>());
        }
    }

    void ScrollToShow(RectTransform target)
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        // lokalne pozycje
        Vector3 viewportLocalPos = viewport.localPosition;
        Vector3 childLocalPos = target.localPosition;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        // przesuniêcie wzglêdem contentu
        float offset = content.anchoredPosition.y;
        float targetY = -childLocalPos.y;

        // Je¿eli obiekt wychodzi poza górê
        if (targetY + target.rect.height > offset + viewportHeight)
        {
            offset = targetY + target.rect.height - viewportHeight;
        }
        // Je¿eli obiekt wychodzi poza dó³
        else if (targetY < offset)
        {
            offset = targetY;
        }

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, offset);
    }
}
