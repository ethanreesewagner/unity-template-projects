using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyLogic enemyLogic;
    [SerializeField] private moneysystem moneySystem;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text enemyHealthText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text instructionsText;
    [SerializeField] private Text outcomeText;
    [SerializeField] private Vector3 coinTextOffset = new Vector3(0f, 1.25f, 0f);

    private string _playerHealthLabel = "Player HP: N/A";
    private string _enemyHealthLabel = "Enemy HP: N/A";
    private string _coinLabel = "Coins: N/A";
    private string _outcomeLabel = string.Empty;
    private readonly string _instructionLabel = "Press Q to attack";

    private void Awake()
    {
        EnsurePlayerHealthAndAttack();

        if (enemyLogic == null)
        {
            enemyLogic = FindObjectOfType<EnemyLogic>();
        }

        if (playerHealthText == null || enemyHealthText == null || coinsText == null || instructionsText == null || outcomeText == null)
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

                if (coinsText == null && text.name.ToLower().Contains("coin"))
                {
                    coinsText = text;
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

        if (moneySystem == null)
        {
            moneySystem = FindObjectOfType<moneysystem>();
            if (moneySystem == null)
            {
                moneySystem = gameObject.AddComponent<moneysystem>();
            }
        }

        EnsureCoinsText();
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

        if (moneySystem != null)
        {
            _coinLabel = $"Coins: {moneySystem.Coins}";
        }
        else
        {
            _coinLabel = "Coins: N/A";
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

        EnsureOutcomeText();

        if (playerHealthText != null)
        {
            playerHealthText.text = _playerHealthLabel;
        }

        if (enemyHealthText != null)
        {
            enemyHealthText.text = _enemyHealthLabel;
        }

        if (coinsText != null)
        {
            coinsText.text = _coinLabel;
            UpdateCoinsTextPosition();
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

    private void EnsureOutcomeText()
    {
        if (outcomeText != null)
        {
            return;
        }

        var texts = GetComponentsInChildren<Text>(true);
        foreach (var text in texts)
        {
            if (text.name.ToLower().Contains("result") || text.name.ToLower().Contains("outcome") || text.name.ToLower().Contains("status"))
            {
                outcomeText = text;
                return;
            }
        }

        GameObject outcomeObject = new GameObject("OutcomeText");
        outcomeObject.transform.SetParent(transform, false);

        outcomeText = outcomeObject.AddComponent<Text>();
        outcomeText.text = _outcomeLabel;
        outcomeText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        outcomeText.fontSize = 32;
        outcomeText.alignment = TextAnchor.MiddleCenter;
        outcomeText.color = Color.white;

        RectTransform outcomeRect = outcomeObject.GetComponent<RectTransform>();
        outcomeRect.sizeDelta = new Vector2(400f, 80f);
        outcomeRect.anchorMin = new Vector2(0.5f, 0.5f);
        outcomeRect.anchorMax = new Vector2(0.5f, 0.5f);
        outcomeRect.anchoredPosition = new Vector2(0f, 80f);
    }

    private void EnsureCoinsText()
    {
        if (coinsText != null)
        {
            return;
        }

        var texts = GetComponentsInChildren<Text>(true);
        foreach (var text in texts)
        {
            if (text.name.ToLower().Contains("coin") || text.name.ToLower().Contains("balance") || text.name.ToLower().Contains("money"))
            {
                coinsText = text;
                return;
            }
        }

        GameObject playerObject = playerHealth != null ? playerHealth.gameObject : FindObjectOfType<PlayerHealth>()?.gameObject;
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (playerObject == null)
        {
            playerObject = gameObject;
        }

        GameObject canvasObject = new GameObject("CoinBalanceCanvas");
        canvasObject.transform.SetParent(playerObject.transform, false);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 100;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(250f, 60f);
        canvasRect.localScale = Vector3.one * 0.01f;
        canvasRect.localPosition = coinTextOffset;

        GameObject textObject = new GameObject("CoinBalanceText");
        textObject.transform.SetParent(canvasObject.transform, false);

        coinsText = textObject.AddComponent<Text>();
        coinsText.text = _coinLabel;
        coinsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        coinsText.fontSize = 24;
        coinsText.alignment = TextAnchor.MiddleCenter;
        coinsText.color = Color.white;

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(250f, 60f);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.anchoredPosition = Vector2.zero;
    }

    private void UpdateCoinsTextPosition()
    {
        if (coinsText == null)
        {
            return;
        }

        Transform targetTransform = playerHealth != null ? playerHealth.transform : null;
        if (targetTransform == null)
        {
            return;
        }

        Transform canvasTransform = coinsText.transform.parent;
        if (canvasTransform != null)
        {
            canvasTransform.SetParent(targetTransform, false);
            canvasTransform.localPosition = coinTextOffset;
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
