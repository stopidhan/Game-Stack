using UnityEngine;

public class GameManager072 : MonoBehaviour
{
    public static GameManager072 Instance { get; private set; }
    public static float GlobalSpeedMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ResetSpeed()
    {
        GlobalSpeedMultiplier = 1f;
    }
}