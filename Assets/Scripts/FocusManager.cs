using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FocusManager : MonoBehaviour
{
    private Stack<List<GameObject>> selectablesStack = new();
    private bool isProcessing = false;
    private bool pendingRescan = false;
    private const float RescanDelay = 0.03f;

    private void EnableTopElement()
    {
        if (isProcessing)
        {
            pendingRescan = true;
            return;
        }

        isProcessing = true;

        RemoveDestroyedElements();

        if (selectablesStack.Count == 0)
        {
            isProcessing = false;
            return;
        }

        var topList = selectablesStack.Peek();

        GameObject firstNonNull = null;
        foreach (var it in topList)
        {
            if (it != null)
            {
                firstNonNull = it;
                break;
            }
        }
        if (firstNonNull != null)
            EnsureParentsActive(firstNonNull);

        foreach (var item in topList)
        {
            if (item != null && !item.activeSelf)
                SafeSetActive(item, true);
        }

        GameObject current = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
        bool keepCurrent = false;
        if (current != null)
        {
            foreach (var item in topList)
            {
                if (ReferenceEquals(item, current) && current.activeInHierarchy)
                {
                    keepCurrent = true;
                    break;
                }
            }
        }

        if (!keepCurrent)
        {
            foreach (var item in topList)
            {
                if (item != null && item.activeInHierarchy)
                {
                    if (EventSystem.current != null)
                        EventSystem.current.SetSelectedGameObject(item);
                    break;
                }
            }
        }

        var preserve = new HashSet<GameObject>();
        foreach (var g in topList)
            if (g != null) preserve.Add(g);

        var lists = selectablesStack.ToArray();
        for (int i = 0; i < lists.Length; i++)
        {
            var list = lists[i];
            if (list == topList) continue;

            foreach (var go in list)
            {
                if (go == null) continue;
                if (preserve.Contains(go)) continue;
                if (go.activeSelf)
                    SafeSetActive(go, false);
            }
        }

        if (!IsInvoking(nameof(InvokeRescan)))
            Invoke(nameof(InvokeRescan), RescanDelay);

        isProcessing = false;
    }

    private void EnsureParentsActive(GameObject child)
    {
        var t = child.transform.parent;
        var toActivate = new Stack<Transform>();
        while (t != null)
        {
            toActivate.Push(t);
            t = t.parent;
        }

        while (toActivate.Count > 0)
        {
            var tr = toActivate.Pop();
            if (tr != null && !tr.gameObject.activeSelf)
                SafeSetActive(tr.gameObject, true);
        }
    }

    private void InvokeRescan()
    {
        GameEvents.RescanYourGrid?.Invoke();

        if (pendingRescan)
        {
            pendingRescan = false;
            if (!IsInvoking(nameof(DelayedEnableTopElement)))
                Invoke(nameof(DelayedEnableTopElement), RescanDelay);
        }
    }

    private void DelayedEnableTopElement()
    {
        EnableTopElement();
    }

    private void RemoveDestroyedElements()
    {
        if (selectablesStack.Count == 0) return;

        var lists = selectablesStack.ToArray();
        for (int i = 0; i < lists.Length; i++)
        {
            var list = lists[i];
            if (list == null) continue;
            list.RemoveAll(item => item == null);
        }
    }

    private void AddToFocusStack(List<GameObject> selectableObjects)
    {
        if (selectableObjects == null) return;

        selectableObjects.RemoveAll(x => x == null);

        if (selectableObjects.Count == 0)
        {
            if (!IsInvoking(nameof(InvokeRescan)))
                Invoke(nameof(InvokeRescan), RescanDelay);
            return;
        }

        if (selectablesStack.Count > 0)
        {
            var top = selectablesStack.Peek();
            if (ReferenceEquals(top, selectableObjects))
            {
                EnableTopElement();
                return;
            }

            if (top != null && top.Count == selectableObjects.Count)
            {
                bool same = true;
                for (int i = 0; i < top.Count; i++)
                {
                    if (!ReferenceEquals(top[i], selectableObjects[i]))
                    {
                        same = false;
                        break;
                    }
                }
                if (same) return;
            }
        }

        selectablesStack.Push(selectableObjects);

        if (isProcessing)
        {
            pendingRescan = true;
            return;
        }

        EnableTopElement();
    }

    private void RemoveFromFocusStack()
    {
        if (selectablesStack.Count > 0)
            selectablesStack.Pop();

        if (isProcessing)
        {
            pendingRescan = true;
            return;
        }

        EnableTopElement();
    }

    private void OnEnable()
    {
        GameEvents.AddListToTheStack += AddToFocusStack;
        GameEvents.RemoveListFromStack += RemoveFromFocusStack;
    }

    private void OnDisable()
    {
        GameEvents.AddListToTheStack -= AddToFocusStack;
        GameEvents.RemoveListFromStack -= RemoveFromFocusStack;
    }

    private void SafeSetActive(GameObject go, bool value)
    {
        if (go == null) return;

        if (go.activeSelf == value) return;

        try
        {
            go.SetActive(value);
        }
        catch (System.InvalidOperationException) { }
        catch (UnityEngine.UnityException) { }
    }
}
