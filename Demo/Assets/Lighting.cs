using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lighting : MonoBehaviour
{
    [Header("Night settings")]
    [SerializeField] private float globalLightIntensity = 0.22f;
    [SerializeField] private Color globalLightColor = new Color(0.12f, 0.18f, 0.35f, 1f);
    [SerializeField] private Color cameraBackgroundColor = new Color(0.04f, 0.06f, 0.12f, 1f);
    [SerializeField] private Color playerLightColor = new Color(0.95f, 0.82f, 0.42f, 1f);
    [SerializeField] private float playerLightRadius = 3.5f;
    [SerializeField] private float playerLightInnerRadius = 0.8f;
    [SerializeField] private float playerLightIntensity = 1.2f;

    private Light2D _playerLight;
    private Light2D _globalLight;

    private void Start()
    {
        ConfigureNightScene();
        CreatePlayerLight();
    }

    private void LateUpdate()
    {
        if (_playerLight != null)
        {
            _playerLight.transform.position = transform.position;
        }
    }

    private void ConfigureNightScene()
    {
        if (Camera.main != null)
        {
            Camera.main.backgroundColor = cameraBackgroundColor;
        }

        foreach (var light in FindObjectsOfType<Light2D>())
        {
            if (light.lightType == Light2D.LightType.Global)
            {
                _globalLight = light;
                break;
            }
        }

        if (_globalLight != null)
        {
            _globalLight.intensity = globalLightIntensity;
            _globalLight.color = globalLightColor;
        }
    }

    private void CreatePlayerLight()
    {
        if (_playerLight != null)
        {
            return;
        }

        var lightObject = new GameObject("Player Light");
        lightObject.transform.SetParent(transform, false);
        lightObject.transform.localPosition = Vector3.zero;

        _playerLight = lightObject.AddComponent<Light2D>();
        _playerLight.lightType = Light2D.LightType.Point;
        _playerLight.color = playerLightColor;
        _playerLight.intensity = playerLightIntensity;
        _playerLight.pointLightOuterRadius = playerLightRadius;
        _playerLight.pointLightInnerRadius = playerLightInnerRadius;
    }
}
