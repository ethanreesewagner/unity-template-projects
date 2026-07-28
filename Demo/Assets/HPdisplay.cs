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
        EnsurePlayerHealth();

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
        if (playerHealth == null)
        {
            EnsurePlayerHealth();
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
            playerHealthText.text = _playerHealthLabel;
        }

        if (enemyHealthText != null)
        {
            enemyHealthText.text = _enemyHealthLabel;
        }
    }

    private void EnsurePlayerHealth()
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
