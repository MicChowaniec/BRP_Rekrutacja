using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private PlayerActions playerActions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playerActions = new PlayerActions();
            playerActions.Newactionmap.Enable(); // w��cz map� akcji
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Zwraca wektor ruchu z Left Stick + D-Pad
    /// </summary>
    public Vector2 GetMove()
    {
        if (playerActions != null)
            return playerActions.Newactionmap.Move.ReadValue<Vector2>();

        return Vector2.zero;
    }

    /// <summary>
    /// Sprawdza wci�ni�cie Back (East Button)
    /// </summary>
    public bool EastButtonPressed()
    {
        if (playerActions != null)
            return playerActions.Newactionmap.Back.WasPressedThisFrame();

        return false;
    }
}
