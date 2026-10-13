using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    
    
    void Start()
    {
        var playerGO = GameObject.Find("Player");
        if (playerGO != null)
        {
            _player = playerGO.GetComponent<Player>();
            if (_player == null)
            {
                Debug.LogError("Enemy.Start: Player component not found on GameObject 'Player'.");
            }
        }
        else
        {
            Debug.LogError("Enemy.Start: Could not find GameObject named 'Player' in the scene.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Enemy.Start: AudioSource component missing on Enemy. Audio won't play.");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("the Animator is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8.37f)
        {
            float RandomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(RandomX, 10.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Enemy.OnTriggerEnter2D: collided with '{other.name}' tag='{other.tag}'");

        if (other.CompareTag("Player")) 

        {
            if (other.transform.TryGetComponent<Player>(out Player player))
            {
                player.Damage();
            }
            if (_anim != null)
            {
                _anim.SetTrigger("OnEnemyDeath");
            }
            else
            {
                Debug.LogWarning("Enemy.OnTriggerEnter2D: Animator is null.");
            }

            _speed = 0;
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Enemy.OnTriggerEnter2D: AudioSource is null, cannot play death sound.");
            }

            Destroy(this.gameObject, 2.8f);
            
        }

    if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            _player?.AddScore(10);
            if (_anim != null)
            {
                _anim.SetTrigger("OnEnemyDeath");
            }
            _speed = 0;
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Enemy.OnTriggerEnter2D: AudioSource is null, cannot play death sound.");
            }
            Destroy(this.gameObject, 2.8f);
        }
    }   

}
