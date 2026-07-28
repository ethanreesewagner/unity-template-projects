using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lighting : MonoBehaviour
{
    [Header("Night settings")]
    [SerializeField] private float globalLightIntensity = 0.22f;
    [SerializeField] private Color globalLightColor = new Color(0.06f, 0.10f, 0.25f, 0.75f);
    [SerializeField] private Color cameraBackgroundColor = new Color(0.03f, 0.05f, 0.18f, 0.75f);
    [SerializeField] private Color playerLightColor = new Color(0.95f, 0.82f, 0.42f, 1f);
    [SerializeField] private float playerLightRadius = 3.5f;
    [SerializeField] private float playerLightInnerRadius = 0.8f;
    [SerializeField] private float playerLightIntensity = 1.2f;

    private Light2D _playerLight;
    private Light2D _globalLight;
    private Transform _targetTransform;

    private void Start()
    {
        FindTargetTransform();
        ConfigureNightScene();
        CreatePlayerLight();
    }

    private void LateUpdate()
    {
        if (_playerLight != null)
        {
            if (_targetTransform != null)
            {
                _playerLight.transform.position = _targetTransform.position;
            }
            else
            {
                _playerLight.transform.position = transform.position;
            }
        }
    }

    private void FindTargetTransform()
    {
        _targetTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_targetTransform != null)
        {
            return;
        }

        var playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            _targetTransform = playerHealth.transform;
            return;
        }

        var playerMovement = FindObjectOfType<TopDownMovement>();
        if (playerMovement != null)
        {
            _targetTransform = playerMovement.transform;
            return;
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

        if (_globalLight == null)
        {
            var globalLightObject = new GameObject("Global Light");
            globalLightObject.transform.SetParent(transform, false);
            _globalLight = globalLightObject.AddComponent<Light2D>();
            _globalLight.lightType = Light2D.LightType.Global;
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
