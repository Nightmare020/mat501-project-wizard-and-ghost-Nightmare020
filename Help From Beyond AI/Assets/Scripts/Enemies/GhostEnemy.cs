
using System;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;

    //Camera camera;
    private bool dead = false;

    //[SerializeField] private Vector2 direction = new Vector2(1, 0);
    [SerializeField] private float speed = 1, normalSpeed = 1, maxSpeed = 7;
    //[SerializeField] private float minDistToWizard = 10, minMinDistance = 1;
    [SerializeField] private Color angerColor;

    //wizard
    private WizardValues _wizardValues;
    private GhostValues _ghostValues;
    private bool isStopped = false;
    private bool isDead = false;

    void Start()
    {
        //camera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D.gravityScale = 0;
        UpdateReferences();
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, minDistToWizard);
    //}

    private void FixedUpdate()
    {
        if (isDead) return;

        // Update references every few frames to avoid overhead
        if (Time.frameCount % 10 == 0)
        {
            UpdateReferences();
        }

        if (_wizardValues && _ghostValues)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (isStopped)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = Vector2.zero;

        // Chase ghost if it's still alive
        if (!_ghostValues._playerManager.isDead)
        {
            _spriteRenderer.color = Color.white;
            direction = (_ghostValues.transform.position - transform.position).normalized;
        }
        // Otherwise, chase wizard
        else if (!_wizardValues._playerManager.isDead)
        { 
            _spriteRenderer.color = angerColor;
            direction = (_wizardValues.transform.position - transform.position).normalized;
        }

        // Adjust movement speed
        _rigidbody2D.AddForce(direction * speed - _rigidbody2D.velocity);
    }

    private void UpdateReferences()
    {
        _wizardValues = GetWizard();
        _ghostValues = GetGhost();
    }

    public void Stop()
    {
        isStopped = true;
    }

    public void Resume()
    {
        isStopped = false;
    }

    public void IncreaseDifficulty()
    {
        normalSpeed = Mathf.Min(maxSpeed, speed + 1f);
        //minDistToWizard = Mathf.Max(minMinDistance, minDistToWizard - 1f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            // Ghost enemy kills the ghost
            _ghostValues._playerManager.Die();
            GhostAgent ghostAgent = other.gameObject.GetComponent<GhostAgent>();

            if (ghostAgent != null)
            {
                ghostAgent.AddReward(-1f);
            }
        }
        else if (other.gameObject.CompareTag("Wizard") && !_ghostValues._playerManager.isDead)
        {
            // Wizard kills the ghost enemy
            Die();

            WizardAgent wizardAgent = other.gameObject.GetComponent<WizardAgent>();

            if (wizardAgent != null)
            {
                wizardAgent.AddReward(1f);
            }
        }
        else if (other.gameObject.CompareTag("Wizard") && _ghostValues._playerManager.isDead)
        {
            // Ghost enemy kills the wizard
            WizardAgent wizardAgent = other.gameObject.GetComponent<WizardAgent>();

            if (wizardAgent != null)
            {
                wizardAgent.AddReward(-1f);
            }

            _wizardValues._playerManager.Die();
        }
    }

    GhostValues GetGhost()
    {
        GameObject ghostObj = GameObject.FindWithTag("Ghost");
        if (ghostObj)
        {
            return ghostObj.GetComponentInChildren<GhostValues>();
        }

        return null;
    }

    WizardValues GetWizard()
    {

        GameObject wizardObj = GameObject.FindWithTag("Wizard");
        if (wizardObj)
        {
            return wizardObj.GetComponentInChildren<WizardValues>();
        }

        return null;
    }

    public void Die()
    {
        dead = true;
        _spriteRenderer.color = Color.clear;
        _rigidbody2D.simulated = false;
        _collider2D.enabled = false;
    }


    private void Activate()
    {
        dead = false;
        _spriteRenderer.color = Color.white;
        _rigidbody2D.simulated = true;
        _collider2D.enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (dead)
        {
            Activate();
        }
    }
}