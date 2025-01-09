using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HepFromBeyondEnvController : MonoBehaviour
{
    [SerializeField] GameObject Door; // Reference to the door
    [SerializeField] GameObject[] Enemies; // Array of enemies in the level
    [SerializeField] Transform WizardSpawnPoint; // Spawn point of the wizard
    [SerializeField] Transform GhostSpawnPoint; // Spawn point of the ghost
    private bool doorUnlocked = false;

    public void UnlockDoor()
    {
        if (!doorUnlocked)
        {
            doorUnlocked = true;
            Debug.Log("Door Unlocked");
            Door.SetActive(false);
        }
    }

    public void WizardDied()
    {
        Debug.Log("Wizard died. Resetting...");
    }

    public void GhostDied()
    {
        Debug.Log("Wizard died. Resetting...");
    }

    public void ResetLevel()
    {
        // reset players to spawn points
        GameObject wizard = GameObject.FindWithTag("Wizard");
        GameObject ghost = GameObject.FindWithTag("Ghost");

        wizard.transform.position = WizardSpawnPoint.position;
        ghost.transform.position = GhostSpawnPoint.position;

        // Reset door
        doorUnlocked = false;
        Door.SetActive(true);

        // Reset enemies
        foreach (GameObject enemy in Enemies)
        {
            enemy.SetActive(true);
        }

        Debug.Log("Level reset complete");
    }
}
