using UnityEngine;
public enum Weakness
{
    sword,
    bow
}
public class SoulEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject InteractionPanelObject;
    [SerializeField] private GameObject ActionsPanelObject;
    [SerializeField] private SpriteRenderer EnemySpriteRenderer;

    private SpawnPoint _enemyPosition;

    private Weakness Weakness;
    



    public void SetupEnemy(Sprite sprite, SpawnPoint spawnPoint)
    {
       
        //It should be probably predefined, but at this point I leave it as it is.
        Weakness = (Weakness)Random.Range(0,2);
        EnemySpriteRenderer.sprite = sprite;
        _enemyPosition = spawnPoint;
        gameObject.SetActive(true);
    }

    public SpawnPoint GetEnemyPosition()
    {
        return _enemyPosition;
    }

    public GameObject GetEnemyObject()
    {
        return this.gameObject;
    }

    private void ActiveCombatWithEnemy()
    {
        ActiveInteractionPanel(false);
        ActiveActionPanel(true);
    }

    private void ActiveInteractionPanel(bool active)
    {
        InteractionPanelObject.SetActive(active);
    }

    private void ActiveActionPanel(bool active)
    {
        ActionsPanelObject.SetActive(active);
    }

    private void UseBow()
    {
        int reward = 10;
        if(Weakness==Weakness.bow)
        {
            reward += 5;
        }
        GameEvents.EnemyKilled?.Invoke(this);
        GameEvents.PointsUpdate?.Invoke(reward);
    }

    private void UseSword()
    {
        int reward = 10;
        if (Weakness == Weakness.sword)
        {
            reward += 5;
        }
        GameEvents.EnemyKilled?.Invoke(this);
        GameEvents.PointsUpdate?.Invoke(reward);

    }

    #region OnClicks

    public void Combat_OnClick()
    {
        ActiveCombatWithEnemy();
    }

    public void Bow_OnClick()
    {
        UseBow();
    }

    public void Sword_OnClick()
    {
        UseSword();
    }

    #endregion
}


public interface IEnemy
{
    SpawnPoint GetEnemyPosition();
    GameObject GetEnemyObject();
}
