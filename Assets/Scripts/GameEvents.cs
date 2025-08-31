using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameEvents
{
    public delegate void OnEnemyKilled(IEnemy enemy);
    public static OnEnemyKilled EnemyKilled;
    public delegate void AddPoints(int points);
    public static AddPoints PointsUpdate;
    public delegate void NewPanelOpened(List<GameObject> selectables);
    public static NewPanelOpened AddListToTheStack;
    public delegate void PanelClosed();
    public static PanelClosed RemoveListFromStack;
    public delegate void RemovedNulls();
    public static RemovedNulls RescanYourGrid;


}

