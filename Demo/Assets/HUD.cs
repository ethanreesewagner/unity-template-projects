using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyLogic enemyLogic;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text enemyHealthText;
    [SerializeField] private Text instructionsText;

    private void Update()
    {
        if (playerHealth != null && playerHealthText != null)
        {
            playerHealthText.text = $"Player HP: {playerHealth.CurrentHealth:0}/{playerHealth.MaxHealth:0}";
        }

        if (enemyLogic != null && enemyHealthText != null)
        {
            enemyHealthText.text = $"Enemy HP: {enemyLogic.CurrentHealth:0}/{enemyLogic.MaxHealth:0}";
        }

        if (instructionsText != null)
        {
            instructionsText.text = "Press Q to attack";
        }
    }
}
