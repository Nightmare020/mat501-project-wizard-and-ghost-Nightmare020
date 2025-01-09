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
    [SerializeField] bool IHaveAKey; // Wizard has key or not
    private Rigidbody2D _rigidBody;
    

    // Initialize is called before the first frame update
    public override void Initialize()
    {
        _movement = GetComponent<WizardMovement>();
        _values = GetComponent<WizardValues>();
        _rigidBody = _values.rigidBody;

        MyKey.SetActive(false);
        IHaveAKey = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    private void ShootSpell()
    {

    }
}
