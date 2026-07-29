using UnityEngine;

public class moneysystem : MonoBehaviour
{
    public static moneysystem Instance { get; private set; }

    [SerializeField] private int startCoins = 0;
    private int _coins;

    public int Coins => _coins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _coins = Mathf.Max(0, startCoins);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _coins += amount;
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0)
        {
            return true;
        }

        if (_coins < amount)
        {
            return false;
        }

        _coins -= amount;
        return true;
    }

    public bool CanAfford(int amount)
    {
        return amount > 0 && _coins >= amount;
    }

    public void SetCoins(int amount)
    {
        _coins = Mathf.Max(0, amount);
    }
}
