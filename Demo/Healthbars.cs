using UnityEngine;

// Simple healthbar demo: shows player and enemy healthbars using OnGUI.
// Player and enemy can attack via keyboard: Space = player attacks, E = enemy attacks.
// Enemy is stronger than player (higher max health and higher damage).
public class Healthbars : MonoBehaviour
{
    // Player stats
    public float playerMaxHealth = 100f;
    public float playerHealth = 100f;
    public float playerDamage = 12f;

    // Enemy stats (more powerful)
    public float enemyMaxHealth = 160f;
    public float enemyHealth = 160f;
    public float enemyDamage = 20f;

    // GUI settings
    public Vector2 barSize = new Vector2(300, 24);
    public Vector2 playerBarPos = new Vector2(20, 20);
    public Vector2 enemyBarPos = new Vector2(20, 60);

    void Start()
    {
        // Initialize current health to max if not set
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);
        enemyHealth = Mathf.Clamp(enemyHealth, 0, enemyMaxHealth);
    }

    void Update()
    {
        // Controls for demo purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DamageEnemy(playerDamage);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            DamagePlayer(enemyDamage);
        }
    }

    void DamagePlayer(float dmg)
    {
        playerHealth = Mathf.Max(0, playerHealth - dmg);
    }

    void DamageEnemy(float dmg)
    {
        enemyHealth = Mathf.Max(0, enemyHealth - dmg);
    }

    void OnGUI()
    {
        DrawBar(playerBarPos, barSize, playerHealth / playerMaxHealth, Color.green, "Player");
        DrawBar(enemyBarPos, barSize, enemyHealth / enemyMaxHealth, Color.red, "Enemy");

        // Instructions
        GUI.Label(new Rect(20, 100, 400, 20), "Press Space to attack enemy (player damage: " + playerDamage + ")");
        GUI.Label(new Rect(20, 120, 400, 20), "Press E to have enemy attack player (enemy damage: " + enemyDamage + ")");
    }

    void DrawBar(Vector2 pos, Vector2 size, float fraction, Color color, string label)
    {
        var bgRect = new Rect(pos.x, pos.y, size.x, size.y);
        GUI.Box(bgRect, "");

        var fillRect = new Rect(pos.x + 2, pos.y + 2, (size.x - 4) * Mathf.Clamp01(fraction), size.y - 4);
        var prevColor = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(fillRect, Texture2D.whiteTexture);
        GUI.color = prevColor;

        GUI.Label(new Rect(pos.x + 6, pos.y + 2, size.x - 12, size.y), string.Format("{0}: {1}/{2}", label, Mathf.CeilToInt(fraction * (label=="Player"?playerMaxHealth:enemyMaxHealth)), (label=="Player"?Mathf.CeilToInt(playerMaxHealth):Mathf.CeilToInt(enemyMaxHealth))));
    }
}
