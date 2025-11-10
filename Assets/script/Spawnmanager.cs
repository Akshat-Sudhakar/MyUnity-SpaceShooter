using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnmanager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerups;
    

    // Start is called before the first frame update
    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }
    

    

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (!_stopSpawning)
        {
            var positionToSpawn = new Vector3(Random.Range(-9f, 9f), 10.5f, 0);
            if (_enemyPrefab == null)
            {
                Debug.LogError("Spawnmanager.SpawnRoutine: _enemyPrefab is not assigned in the Inspector.");
            }
            else
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
                if (_enemyContainer != null)
                {
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
            }
            yield return new WaitForSeconds(5.0f);

        }

    }

    IEnumerator SpawnPowerupRoutine()
    {   
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            var positionToSpawn = new Vector3(Random.Range(-9f, 9f), 10.5f, 0);
            if (_powerups == null || _powerups.Length == 0)
            {
                Debug.LogWarning("Spawnmanager.SpawnPowerupRoutine: _powerups array is empty or not assigned.");
            }
            else
            {
                int randomPowerUp = Random.Range(0, _powerups.Length);
                var prefab = _powerups[randomPowerUp];
                if (prefab == null)
                {
                    Debug.LogWarning($"Spawnmanager.SpawnPowerupRoutine: _powerups[{randomPowerUp}] is null.");
                }
                else
                {
                    Instantiate(prefab, positionToSpawn, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        StopAllCoroutines();
        Debug.Log("Spawning stopped: Player died.");
        // Destroy all existing enemies
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            Destroy(e.gameObject);
        }
    }

    

}

