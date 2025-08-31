using System;
using TMPro;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    private int Points = 0;


    [SerializeField]private TextMeshProUGUI m_TextMeshProUGUI;
    private void OnEnable()
    {
        GameEvents.PointsUpdate += UpdatePoints;
    }
    private void OnDisable()
    {
        GameEvents.PointsUpdate -= UpdatePoints;
    }

    public void UpdatePoints(int _points)
    {
        Points += _points;
        m_TextMeshProUGUI.text = "Points: " + Points;
    }
}
