using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelFromBeyondEnvController : MonoBehaviour
{
    [SerializeField] GameObject Door; // Reference to the door
    [SerializeField] GameObject[] Enemies; // Array of enemies in the level
    [SerializeField] Transform WizardSpawnPoint; // Spawn point of the wizard
    [SerializeField] Transform GhostSpawnPoint; // Spawn point of the ghost
    private bool doorUnlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
