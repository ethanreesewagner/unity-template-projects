using UnityEngine;

public class harvestingmechanic : MonoBehaviour
{
    [SerializeField] private int coinReward = 10;
    [SerializeField] private float cooldownSeconds = 65f;
    [SerializeField] private string cooldownMessage = "This resource is still refreshing.";

    private float _lastHarvestTime = -Mathf.Infinity;

    private bool IsOnCooldown => Time.time < _lastHarvestTime + cooldownSeconds;
    private float RemainingCooldown => Mathf.Max(0f, (_lastHarvestTime + cooldownSeconds) - Time.time);

    private void OnMouseDown()
    {
        if (IsOnCooldown)
        {
            Debug.Log(cooldownMessage + $" ({RemainingCooldown:0.0}s remaining)");
            return;
        }

        if (moneysystem.Instance == null)
        {
            Debug.LogWarning("No moneysystem instance found in the scene.");
            return;
        }

        moneysystem.Instance.AddCoins(coinReward);
        _lastHarvestTime = Time.time;
        Debug.Log($"Harvested {coinReward} coins. Next harvest available in {cooldownSeconds:0} seconds.");
    }
}
