using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameObject GhostEnemy, spawnsGhostEnemies;

        private List<PlayerEnemy> _playerEnemies;
        private List<GhostEnemy> _ghostEnemies;

        [SerializeField] private int maxGhostEnemies = 2;
        private List<Transform> ghostEnemiesSpawnPoints;

        [SerializeField] private float minSpawnDistance;

        private void Start()
        {
            _playerEnemies = new List<PlayerEnemy>();
            _ghostEnemies = new List<GhostEnemy>();

            //wizardEnemiesSpawnPoints = new List<Transform>();
            //wizardEnemiesSpawnPoints.AddRange(spawnsWizardEnemies.GetComponentsInChildren<Transform>());
            //wizardEnemiesSpawnPoints.Remove(spawnsWizardEnemies.transform);

            ghostEnemiesSpawnPoints = new List<Transform>();
            ghostEnemiesSpawnPoints.AddRange(spawnsGhostEnemies.GetComponentsInChildren<Transform>());
            ghostEnemiesSpawnPoints.Remove(spawnsGhostEnemies.transform);


            SpawnEnemies();
        }

        public void SpawnEnemies()
        {
            //ghostenemies
            int enemiesSpawned = 0;
            int enemiesToSpawn = maxGhostEnemies - _ghostEnemies.Count;

            while (enemiesSpawned < enemiesToSpawn)
            {
                Transform spawnPoint = ghostEnemiesSpawnPoints[Random.Range(0, ghostEnemiesSpawnPoints.Count)];
                Transform closestGhostEnemy = GetClosestGhostEnemy(spawnPoint.position);
                if (closestGhostEnemy == null || (closestGhostEnemy != null &&
                                                  Vector2.Distance(spawnPoint.position,
                                                      closestGhostEnemy.transform.position) > minSpawnDistance))
                {
                    GameObject newGhost = Instantiate(GhostEnemy, transform);
                    newGhost.transform.position = spawnPoint.position;
                    _ghostEnemies.Add(newGhost.GetComponent<GhostEnemy>());
                    enemiesSpawned++;
                }
            }


            //wizzard enemies
            //enemiesSpawned = 0;
            //enemiesToSpawn = maxWizardEnemies - _playerEnemies.Count;
            //while (enemiesSpawned < enemiesToSpawn)
            //{
            //    Transform spawnPoint = wizardEnemiesSpawnPoints[Random.Range(0, wizardEnemiesSpawnPoints.Count)];
            //    Transform closestWizzardEnemy = GetClosestWizzardEnemy(spawnPoint.position);
            //    if (closestWizzardEnemy == null || (closestWizzardEnemy != null &&
            //                                        Vector2.Distance(spawnPoint.position,
            //                                            closestWizzardEnemy.transform.position) > minSpawnDistance))
            //    {
            //        GameObject newWizzard = Instantiate(wizardEnemy, transform);
            //        newWizzard.transform.position = spawnPoint.position;
            //        _playerEnemies.Add(newWizzard.GetComponent<PlayerEnemy>());
            //        enemiesSpawned++;
            //    }
            //}
        }

        public void IncreaseDifficulty()
        {
            //IncreaseDifficultyWizard();
            IncreaseDifficultyGhost();
        }

        //public void IncreaseDifficultyWizard()
        //{
        //    maxWizardEnemies = Mathf.Max(1, (int)(maxWizardEnemies * 2f));
        //    SpawnEnemies();
        //    foreach (var enemy in _playerEnemies)
        //    {
        //        enemy.IncreaseDifficulty();
        //    }
        //}

        public void IncreaseDifficultyGhost()
        {
            maxGhostEnemies = Mathf.Max(1, (int)(maxGhostEnemies * 2f));
            SpawnEnemies();
            foreach (GhostEnemy enemy in _ghostEnemies)
            {
                enemy.IncreaseDifficulty();
            }
        }

        public Transform GetClosestGhostEnemy(Vector2 position)
        {
            float min = Single.PositiveInfinity;

            Transform result = null;
            for (int i = 0; i < _ghostEnemies.Count; i++)
            {
                float dist = Vector2.Distance(position, _ghostEnemies[i].transform.position);
                if (dist < min)
                {
                    min = dist;
                    result = _ghostEnemies[i].transform;
                }
            }

            return result;
        }

        public Transform GetClosestWizzardEnemy(Vector2 position)
        {
            float min = Single.PositiveInfinity;

            Transform result = null;
            for (int i = 0; i < _playerEnemies.Count; i++)
            {
                float dist = Vector2.Distance(position, _playerEnemies[i].transform.position);
                RaycastHit2D hit = Physics2D.Raycast(position,
                    ((Vector2)_playerEnemies[i].transform.position - position).normalized);
                if (hit && hit.transform.CompareTag("Wizard Enemy"))
                {
                    if (dist < min)
                    {
                        min = dist;
                        result = _playerEnemies[i].transform;
                    }
                }
            }

            return result;
        }
    }
}