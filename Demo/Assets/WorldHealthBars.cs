using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBars : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyLogic enemyLogic;
    [SerializeField] private Vector3 playerOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private Vector3 enemyOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private float barWidth = 1.2f;
    [SerializeField] private float barHeight = 0.18f;
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.55f);
    [SerializeField] private Color fillColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color enemyFillColor = new Color(0.8f, 0.3f, 0.3f, 1f);

    private RectTransform _playerBar;
    private RectTransform _enemyBar;
    private Image _playerFillImage;
    private Image _enemyFillImage;

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

        CreateBars();
    }

    private void CreateBars()
    {
        if (playerHealth != null)
        {
            (_playerBar, _playerFillImage) = CreateHealthBar("PlayerHPBar", fillColor);
        }

        if (enemyLogic != null)
        {
            (_enemyBar, _enemyFillImage) = CreateHealthBar("EnemyHPBar", enemyFillColor);
        }
    }

    private (RectTransform bar, Image fill) CreateHealthBar(string name, Color fillTint)
    {
        var barObject = new GameObject(name);
        barObject.transform.SetParent(transform, false);

        var canvas = barObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 500;

        var canvasRect = barObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(barWidth, barHeight);

        var background = new GameObject("Background");
        background.transform.SetParent(barObject.transform, false);
        var bgImage = background.AddComponent<Image>();
        bgImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        bgImage.color = backgroundColor;
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0f);
        bgRect.anchorMax = new Vector2(1f, 1f);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        var fillObject = new GameObject("Fill");
        fillObject.transform.SetParent(barObject.transform, false);
        var fillImage = fillObject.AddComponent<Image>();
        fillImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        fillImage.color = fillTint;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        fillImage.fillAmount = 1f;

        var fillRect = fillObject.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.offsetMin = new Vector2(0.04f * barWidth, 0.08f * barHeight);
        fillRect.offsetMax = new Vector2(-0.04f * barWidth, -0.08f * barHeight);

        return (canvasRect, fillImage);
    }

    private void Update()
    {
        UpdateBar(playerHealth, playerOffset, _playerBar, _playerFillImage);
        UpdateBar(enemyLogic, enemyOffset, _enemyBar, _enemyFillImage);
    }

    private void UpdateBar(MonoBehaviour targetHealth, Vector3 offset, RectTransform bar, Image fillImage)
    {
        if (bar == null || fillImage == null || targetHealth == null)
        {
            return;
        }

        Transform targetTransform = targetHealth.transform;
        var screenPosition = targetTransform.position + offset;
        bar.position = screenPosition;
        bar.rotation = Quaternion.identity;

        float fillAmount = 0f;
        if (targetHealth is PlayerHealth player)
        {
            if (player.MaxHealth > 0)
            {
                fillAmount = Mathf.Clamp01(player.CurrentHealth / player.MaxHealth);
            }
        }
        else if (targetHealth is EnemyLogic enemy)
        {
            if (enemy.MaxHealth > 0)
            {
                fillAmount = Mathf.Clamp01(enemy.CurrentHealth / enemy.MaxHealth);
            }
        }

        fillImage.fillAmount = fillAmount;
    }
}
