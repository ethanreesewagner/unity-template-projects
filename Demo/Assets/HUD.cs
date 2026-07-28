using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyLogic enemyLogic;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text enemyHealthText;
    [SerializeField] private Text instructionsText;

    private string _playerHealthLabel = "Player HP: N/A";
    private string _enemyHealthLabel = "Enemy HP: N/A";
    private readonly string _instructionLabel = "Press Q to attack";
    private bool _shouldShowHud;

    private void Awake()
    {
        _shouldShowHud = FindObjectOfType<WorldHealthBars>() == null;

        if (!_shouldShowHud)
        {
            if (playerHealthText != null)
            {
                playerHealthText.text = string.Empty;
            }

            if (enemyHealthText != null)
            {
                enemyHealthText.text = string.Empty;
            }

            if (instructionsText != null)
            {
                instructionsText.text = string.Empty;
            }

            return;
        }

        EnsurePlayerHealthAndAttack();

        if (enemyLogic == null)
        {
            enemyLogic = FindObjectOfType<EnemyLogic>();
        }

        if (playerHealthText == null || enemyHealthText == null || instructionsText == null)
        {
            var texts = GetComponentsInChildren<Text>();
            foreach (var text in texts)
            {
                if (playerHealthText == null && text.name.ToLower().Contains("player"))
                {
                    playerHealthText = text;
                }

                if (enemyHealthText == null && text.name.ToLower().Contains("enemy"))
                {
                    enemyHealthText = text;
                }

                if (instructionsText == null && text.name.ToLower().Contains("instruction"))
                {
                    instructionsText = text;
                }
            }
        }
    }

    private void Update()
    {
        if (!_shouldShowHud)
        {
            return;
        }

        if (playerHealth == null)
        {
            EnsurePlayerHealthAndAttack();
        }

        if (playerHealth != null)
        {
            _playerHealthLabel = $"Player HP: {playerHealth.CurrentHealth:0}/{playerHealth.MaxHealth:0}";
        }
        else
        {
            _playerHealthLabel = "Player HP: N/A";
        }

        if (enemyLogic != null)
        {
            _enemyHealthLabel = $"Enemy HP: {enemyLogic.CurrentHealth:0}/{enemyLogic.MaxHealth:0}";
        }
        else
        {
            _enemyHealthLabel = "Enemy HP: N/A";
        }

        if (playerHealthText != null)
        {
            playerHealthText.text = string.Empty;
        }

        if (enemyHealthText != null)
        {
            enemyHealthText.text = string.Empty;
        }

        if (instructionsText != null)
        {
            instructionsText.text = string.Empty;
        }
    }

    private void EnsurePlayerHealthAndAttack()
    {
        if (playerHealth != null)
        {
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            playerObject = FindObjectOfType<PlayerHealth>()?.gameObject;
        }

        if (playerObject == null)
        {
            playerObject = FindObjectOfType<TopDownMovement>()?.gameObject;
        }

        if (playerObject == null)
        {
            return;
        }

        playerHealth = playerObject.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = playerObject.AddComponent<PlayerHealth>();
        }

        var attacking = playerObject.GetComponent<Attacking>();
        if (attacking == null)
        {
            playerObject.AddComponent<Attacking>();
        }
    }

    private void OnGUI()
    {
    }
}
