using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstSelected : MonoBehaviour
{
    [SerializeField] private bool firstScreen = false;
    [SerializeField] private List<Selectable> staticButtons;

    private List<GameObject> cachedList = new List<GameObject>();

    private bool scanScheduled = false;
    private int scanAttempts = 0;
    private const int MaxScanAttempts = 6;
    private const int StabilityFramesRequired = 3;
    private const int MaxStabilityWaitFrames = 20;
    private const float RetryDelaySeconds = 0.08f;

    private void OnEnable()
    {
        ScheduleScan();
        GameEvents.RescanYourGrid += ScheduleScan;
    }

    private void OnDisable()
    {
        GameEvents.RemoveListFromStack?.Invoke();
        GameEvents.RescanYourGrid -= ScheduleScan;
    }

    private void ScheduleScan()
    {
        if (scanScheduled) return;
        scanScheduled = true;
        scanAttempts = 0;
        StartCoroutine(DelayedScanCoroutine());
    }

    private IEnumerator DelayedScanCoroutine()
    {
        scanAttempts++;
        int stableFrames = 0;
        int prevCount = -1;
        int framesWaited = 0;

        while (true)
        {
            var selectables = GetComponentsInChildren<Selectable>(includeInactive: false);
            int currentCount = selectables != null ? selectables.Length : 0;

            if (currentCount == prevCount)
                stableFrames++;
            else
                stableFrames = 0;

            prevCount = currentCount;
            framesWaited++;

            if (stableFrames >= StabilityFramesRequired || framesWaited >= MaxStabilityWaitFrames)
                break;

            yield return null;
        }

        BuildAndSendCachedList();
        scanScheduled = false;

        if (cachedList.Count == 0 && scanAttempts < MaxScanAttempts)
        {
            yield return new WaitForSeconds(RetryDelaySeconds);
            ScheduleScan();
        }
    }

    private void BuildAndSendCachedList()
    {
        cachedList.Clear();

        var selectables = GetComponentsInChildren<Selectable>(includeInactive: false);
        if (selectables != null)
        {
            foreach (var sel in selectables)
            {
                if (sel == null) continue;
                if (sel.transform == this.transform) continue;
                cachedList.Add(sel.gameObject);
            }
        }

        if (staticButtons != null)
        {
            foreach (var s in staticButtons)
            {
                if (s == null) continue;
                if (!cachedList.Contains(s.gameObject))
                    cachedList.Add(s.gameObject);
            }
        }

        if (cachedList.Count > 0)
        {
            GameEvents.AddListToTheStack?.Invoke(cachedList);
        }
        else
        {
            Debug.LogWarning($"[FirstSelected] Brak Selectable (attempt {scanAttempts}/{MaxScanAttempts}) w {gameObject.name}");
        }
    }
}
