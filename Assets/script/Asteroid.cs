using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Spawnmanager _Spawnmanager;
    // Start is called before the first frame update
    private void Start()
    {
        _Spawnmanager = GameObject.Find("spawn_manager").GetComponent<Spawnmanager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            if (_explosionPrefab == null)
            {
                Debug.LogError("Asteroid.OnTriggerEnter2D: _explosionPrefab is not assigned on the Asteroid prefab/instance.");
            }
            else
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject);
            if (_Spawnmanager != null)
            {
                _Spawnmanager.StartSpawning();
            }
            else
            {
                Debug.LogWarning("Asteroid.OnTriggerEnter2D: _Spawnmanager is null.");
            }

            Destroy(this.gameObject, 0.25f);
        }
    }
}
