using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GhostAgent : Agent
{
    private GhostMovement _movement;
    private GhostValues _values;
    [SerializeField] GameObject MyKey; // Key the wizard picks up
    [SerializeField] Transform Door;
    [SerializeField] Transform Wizard;
    [SerializeField] bool CompanionHasAKey; // Wizard has key or not
    private Rigidbody2D _rigidBody;
    private Transform _currentTarget;
    private HepFromBeyondEnvController _gameController;

    // Initialize is called before the first frame update
    public override void Initialize()
    {
        _movement = GetComponent<GhostMovement>();
        _values = GetComponent<GhostValues>();
        _rigidBody = _values.rigidBody;
        _gameController = FindObjectOfType<HepFromBeyondEnvController>();
        MyKey.SetActive(false);
        CompanionHasAKey = false;
    }

    public override void OnEpisodeBegin()
    {
        MyKey.SetActive(false);
        CompanionHasAKey = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ghost's position
        sensor.AddObservation(transform.position);

        // Wizard's position
        sensor.AddObservation(Wizard.position);

        // Door's position
        sensor.AddObservation(Door.position);

        // Whether the ghost has the key
        sensor.AddObservation(CompanionHasAKey ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AssistWizard();
    }

    private void AssistWizard()
    {
        float distanceToWizard = Vector3.Distance(transform.position, Wizard.position);

        if (distanceToWizard > 3f)
        {
            _movement.AIMove((Wizard.position - transform.position).normalized);
        }

        if (distanceToWizard < 2f && !Wizard.GetComponent<WizardValues>().doubleJumpPerformed)
        {
            Wizard.GetComponent<WizardMovement>().AIDoubleJump();
            AddReward(1f);
        }

        HandleBarriers();
    }

    private void HandleBarriers()
    {
        RaycastHit2D barrierCheck = Physics2D.Raycast(transform.position, Vector2.right, 5f, LayerMask.GetMask("Barriers"));

        if (barrierCheck.collider != null)
        {
            _movement.PlaceTrampoline(barrierCheck.point);
            AddReward(1f); // Reward for placing trampoline
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            MyKey.SetActive(true);
            CompanionHasAKey = true;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Lock") && CompanionHasAKey)
        {
            MyKey.SetActive(false);
            CompanionHasAKey = false;
            _gameController.UnlockDoor();
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            // Penalize for collision with spikes
            AddReward(-1.0f);
            _gameController.GhostDied();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;

        // Manual movement controls
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        // If no manual input, use AIMove logic
        if (direction == Vector2.zero && Wizard != null)
        {
            direction = (Wizard.position - transform.position).normalized;
        }

        // Apply movement
        continuousActions[0] = direction.x; // Horizontal
        continuousActions[1] = direction.y; // Vertical

        // Optional: Place trampoline with heuristic
        if (Input.GetKey(KeyCode.T))
        {
            RaycastHit2D barrierCheck = Physics2D.Raycast(transform.position, Vector2.right, 5f, LayerMask.GetMask("Barriers"));

            if (barrierCheck.collider != null)
            {
                _movement.PlaceTrampoline(barrierCheck.point); // Place trampoline
                AddReward(1f); // Reward for placing trampoline
                Debug.Log("Heuristic: Placed trampoline");
            }
        }

        // Optional: Assist the wizard in jumping (double jump)
        if (Input.GetKey(KeyCode.Space))
        {
            if (Vector3.Distance(transform.position, Wizard.position) < 2f && !Wizard.GetComponent<WizardValues>().doubleJumpPerformed)
            {
                Wizard.GetComponent<WizardMovement>().AIDoubleJump(); // Trigger double jump
                AddReward(1f); // Reward for assisting
                Debug.Log("Heuristic: Assisted wizard with double jump");
            }
        }
    }
}
