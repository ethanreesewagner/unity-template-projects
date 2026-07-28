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

        if (instructionsText != null)
        {
            instructionsText.text = _instructionLabel;
        }
    }

    private void OnGUI()
    {
        if (playerHealthText != null && enemyHealthText != null && instructionsText != null)
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
        GUI.Label(new Rect(x, y + lineHeight * 2, 300, lineHeight), _instructionLabel, style);
    }
}
