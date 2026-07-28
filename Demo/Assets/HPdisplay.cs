using UnityEngine;
using UnityEngine.UI;

public class HPdisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyLogic enemyLogic;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text enemyHealthText;

    private string _playerHealthLabel = "Player HP: N/A";
    private string _enemyHealthLabel = "Enemy HP: N/A";

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        if (enemyLogic == null)
        {
            enemyLogic = FindObjectOfType<EnemyLogic>();
        }

        if (playerHealthText == null || enemyHealthText == null)
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
            }
        }
    }

    private void Update()
    {
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
            playerHealthText.text = _playerHealthLabel;
        }

        if (enemyHealthText != null)
        {
            enemyHealthText.text = _enemyHealthLabel;
        }
    }

    private void OnGUI()
    {
        if (playerHealthText != null && enemyHealthText != null)
        {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            normal = { textColor = Color.white },
            alignment = TextAnchor.UpperLeft
        };

        float x = 10f;
        float y = 10f;
        float lineHeight = 26f;

        GUI.Label(new Rect(x, y, 300, lineHeight), _playerHealthLabel, style);
        GUI.Label(new Rect(x, y + lineHeight, 300, lineHeight), _enemyHealthLabel, style);
    }
}
