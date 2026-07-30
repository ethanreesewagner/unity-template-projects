using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INFORMATION ==================================================
// This script handles player movement and input in a 2D top-down
// environment. A Rigidbody2D component is required, and gravity
// should be disabled. Control the speed of the player via the
// public speed variable.
// ==============================================================
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMovement : MonoBehaviour
{
    /// <summary>
    /// Store reference to the rigidbody on this game object
    /// </summary>
    protected Rigidbody2D _rb2d;
    public float speed;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        EnsurePlayerComponents();
    }

    void Start()
    {
        EnsurePlayerComponents();
    }

    private void EnsurePlayerComponents()
    {
        if (_rb2d == null)
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        if (_rb2d == null)
        {
            _rb2d = gameObject.AddComponent<Rigidbody2D>();
        }

        _rb2d.gravityScale = 0f;
        _rb2d.freezeRotation = true;

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (!gameObject.CompareTag("Player"))
        {
            TrySetTag("Player");
        }
    }

    private void TrySetTag(string tagName)
    {
        if (string.IsNullOrEmpty(tagName))
        {
            return;
        }

        try
        {
            gameObject.tag = tagName;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Could not assign tag '{tagName}' to '{gameObject.name}': {ex.Message}");
        }
    }

    //Update is called once per frame
    //Each frame, check for inputs corresponding to WASD and apply motion to the player
    void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rb2d.velocity = movement * speed;

        if (_spriteRenderer != null)
        {
            if (movement.x < 0f)
            {
                _spriteRenderer.flipX = true;
            }
            else if (movement.x > 0f)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
    
}
