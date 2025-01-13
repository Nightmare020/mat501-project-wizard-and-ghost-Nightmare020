using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class WizardAgent : Agent
{
    private WizardMovement _movement;
    private WizardValues _values;
    [SerializeField] GameObject MyKey; // Key the wizard picks up
    [SerializeField] private Transform Ghost;
    [SerializeField] Transform Door;
    [SerializeField] bool IHaveAKey; // Wizard has key or not
    private Rigidbody2D _rigidBody;
    private Transform _currentTarget;
    private HepFromBeyondEnvController _gameController;


    // Initialize is called before the first frame update
    public override void Initialize()
    {
        _movement = GetComponent<WizardMovement>();
        _values = GetComponent<WizardValues>();
        _rigidBody = _values.rigidBody;
        _gameController = FindObjectOfType<HepFromBeyondEnvController>();
        MyKey.SetActive(false);
        IHaveAKey = false;
    }

    public override void OnEpisodeBegin()
    {
        MyKey.SetActive(false);
        IHaveAKey = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position); // Wizard's position
        sensor.AddObservation(MyKey ? MyKey.transform.position : Vector3.zero); // Key's position
        sensor.AddObservation(Door.position); // Door's position
        sensor.AddObservation(Ghost.position); // Ghost's companion position
        sensor.AddObservation(IHaveAKey ? 1f : 0f); // Key possesion
        sensor.AddObservation(DetectEnemies()); // Nearby enemies
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Determine the current target
        if (!IHaveAKey)
        {
            MoveTowards(MyKey.transform.position);
        }
        else
        {
            MoveTowards(Door.position);
        }

        HandleEnemyInteractions();
        HandleObstacles();
    }

    private void MoveTowards(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        _movement.AIMove(direction);

        if (ShouldJump(target))
        {
            _movement.AIJump();
        }
    }

    private bool ShouldJump(Vector3 target)
    {
        return target.y > transform.position.y + 1f && _values.IsGrounded();
    }

    private float DetectEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemies"));
        return enemies.Length > 0 ? 1f : 0f;
    }

    private void HandleEnemyInteractions()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemies"));

        foreach (Collider2D enemy in enemies)
        {
            Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float dotProduct = Vector2.Dot(directionToEnemy, transform.right);

            GhostEnemy ghostEnemy = enemy.GetComponent<GhostEnemy>();

            if (ghostEnemy != null)
            {
                if (dotProduct > 0f)
                {
                    ghostEnemy.Stop();
                }
                else
                {
                    ghostEnemy.Resume();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            GhostEnemy ghostEnemy = other.transform.GetComponent<GhostEnemy>();

            if (ghostEnemy != null)
            {
                ghostEnemy.Die();
                AddReward(1.0f); // Reward for killing an enemy
            }
        }

        if (other.transform.CompareTag("Lock") && IHaveAKey)
        {
            MyKey.SetActive(false);
            IHaveAKey = false;
            _gameController.UnlockDoor();
            AddReward(5f);
        }
        else if (other.transform.CompareTag("Lava"))
        {
            // Penalize for collision with spikes
            AddReward(-1);
            _gameController.WizardDied();
        }
    }

    private void HandleObstacles()
    {
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, Vector2.right * _values.facingDirection, 1f, LayerMask.GetMask("Walls"));

        if (wallCheck.collider != null)
        {
            AddReward(-0.2f); // penalize for hitting a wall
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            MyKey.SetActive(true);
            IHaveAKey = true;
            collision.gameObject.SetActive(false);
            AddReward(1f); // reward for picking up the key
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        var discreteActions = actionsOut.DiscreteActions;

        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        continuousActions[0] = direction.x; // Horizontal movement

        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0; // Jump
        discreteActions[1] = Input.GetKey(KeyCode.LeftShift) ? 1 : 0; // Dash
    }
}
