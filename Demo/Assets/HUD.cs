using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyLogic enemyLogic;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text enemyHealthText;
    [SerializeField] private Text instructionsText;
    [SerializeField] private Text outcomeText;

    private string _playerHealthLabel = "Player HP: N/A";
    private string _enemyHealthLabel = "Enemy HP: N/A";
    private string _outcomeLabel = string.Empty;
    private readonly string _instructionLabel = "Press Q to attack";

    private void Awake()
    {
        EnsurePlayerHealthAndAttack();

        if (enemyLogic == null)
        {
            enemyLogic = FindObjectOfType<EnemyLogic>();
        }

        if (playerHealthText == null || enemyHealthText == null || instructionsText == null || outcomeText == null)
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

                if (outcomeText == null && (text.name.ToLower().Contains("result") || text.name.ToLower().Contains("outcome") || text.name.ToLower().Contains("status")))
                {
                    outcomeText = text;
                }
            }
        }
    }

    private void Update()
    {
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

        if (playerHealth != null && playerHealth.CurrentHealth <= 0f)
        {
            _outcomeLabel = "YOU LOSE";
        }
        else if (enemyLogic != null && enemyLogic.CurrentHealth <= 0f)
        {
            _outcomeLabel = "YOU WIN";
        }
        else
        {
            _outcomeLabel = string.Empty;
        }

        if (playerHealthText != null)
        {
            playerHealthText.text = _playerHealthLabel;
        }

        if (enemyHealthText != null)
        {
            enemyHealthText.text = _enemyHealthLabel;
        }

        if (instructionsText != null)
        {
            instructionsText.text = _outcomeLabel.Length > 0 ? string.Empty : _instructionLabel;
        }

        if (outcomeText != null)
        {
            outcomeText.text = _outcomeLabel;
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
        if (outcomeText != null)
        {
            return;
        }

        if (_outcomeLabel.Length == 0)
        {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 32,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(Screen.width * 0.5f - 150f, Screen.height * 0.25f, 300f, 60f), _outcomeLabel, style);
    }
}
