using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first Update
    //[Serializedfield] //help the desginers to change the speed but still be a private variable
    [SerializeField]
    private float _speed = 3.5f;
    private readonly float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserprefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;  // Not readonly because it changes
    [SerializeField]
    private int _lives = 3;  // Not readonly because it changes with damage
    private Spawnmanager _spawnManager;
    [SerializeField]
    private bool _IsTripleShotActive = false;  // Not readonly because it changes with power-up
    [SerializeField]
    private GameObject _TripleShotprefab;
    [SerializeField]
    private bool _IsSpeedPowerUpActive = false;  // Not readonly because it changes with power-up
    [SerializeField]
    private GameObject _shieldsprefab;
    [SerializeField]
    private bool _IsShieldPowerUpActive = false;
    [SerializeField]
    private GameObject _shieldvisualizer;
    [SerializeField]
    private int _score;
    private UI_Manager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;






    void Start()
    {
        //take the current position = new position ( 0,0,0 )
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = FindObjectOfType<Spawnmanager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawnmanager is NULL. Make sure the Spawnmanager script is in the scene.");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null)
        {
            Debug.LogError("the ui manager is NULL");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
             Debug.LogError("the AudioSource ON THE PLAYER is NULL");
        }
        else 
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //transform.Translate(Vector3.right);// moves it at the speed of sound i.e once per frame 

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);//moves it at one m/s if u want any custom value just multiple it by whatever time u want
        //transform.Translate(Vector3.up *verticalInput * _speed * Time.deltaTime);

        //new vector3(1,0,0)*input*3.5f*real time


        if (_IsSpeedPowerUpActive == false)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * _speedMultiplier * Time.deltaTime);
        }

        //creating player bounds
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        //transform.position=new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-3.8f,0),0); optimized upper code 

        if (transform.position.x >= 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {

        _canFire = Time.time + _fireRate;

        if (_IsTripleShotActive == true)
        {
            Debug.Log("FireLaser: Triple shot requested.");
            if (_TripleShotprefab == null)
            {
                Debug.LogError("Player.FireLaser: _TripleShotprefab is NOT assigned in the Inspector!");
            }
            else
            {
                Debug.Log("Player.FireLaser: Instantiating triple shot prefab at position: " + transform.position);
                Instantiate(_TripleShotprefab, transform.position + new Vector3(0, 1.18f, 0), Quaternion.identity);
            }
        }
        else
        {
            if (_laserprefab == null)
            {
                Debug.LogError("Player.FireLaser: _laserprefab is NOT assigned in the Inspector!");
            }
            else
            {
                Debug.Log("Player.FireLaser: Instantiating laser prefab at position: " + transform.position);
                Instantiate(_laserprefab, transform.position + new Vector3(0, 1.18f, 0), Quaternion.identity);
            }
        }
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Player.FireLaser: _audioSource is null, cannot play sound.");
        }

    }

    public void Damage()
    {
        if (_IsShieldPowerUpActive == true)
        {
            _IsShieldPowerUpActive = false;
            if (_shieldvisualizer != null)
            {
                _shieldvisualizer.SetActive(false);
            }
            return;
        }

        _lives -= 1;
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            if (_spawnManager != null)
            {
                Debug.Log("Player died. Calling OnPlayerDeath on Spawnmanager.");
                _spawnManager.OnPlayerDeath();
            }
            else
            {
                Debug.LogError("_spawnManager is null in Damage().");
            }
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _IsTripleShotActive = true;
        Debug.Log("TripleShotActive called. Triple shot enabled.");
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _IsTripleShotActive = false;
    }
    public void SpeedPowerUpActive()
    {
        _IsSpeedPowerUpActive = true;
        Debug.Log("Player.SpeedPowerUpActive: Speed power-up enabled.");
        StartCoroutine(SpeedPowerDownRoutine());
    }
    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _IsSpeedPowerUpActive = false;
        Debug.Log("Player.SpeedPowerDownRoutine: Speed power-up expired.");
    }

    public void ShieldPowerUpActive()
    {
        _IsShieldPowerUpActive = true;
        if (_shieldvisualizer != null)
        {
            _shieldvisualizer.SetActive(true);
            Debug.Log("Player.ShieldPowerUpActive: Activated existing shield visualizer.");
            // If it has an Animator or ParticleSystem, try to restart them
            var anim = _shieldvisualizer.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play(0);
            }
            var ps = _shieldvisualizer.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }
        else if (_shieldsprefab != null)
        {
            // Instantiate the shield prefab as a child of the player so its visuals are visible
            _shieldvisualizer = Instantiate(_shieldsprefab, transform.position, Quaternion.identity, transform);
            _shieldvisualizer.transform.localPosition = Vector3.zero;
            Debug.Log("Player.ShieldPowerUpActive: Instantiated shield prefab and activated it.");
        }
        else
        {
            Debug.LogWarning("Player.ShieldPowerUpActive: No shield visualizer or prefab assigned. Assign _shieldvisualizer or _shieldsprefab in the Inspector.");
        }

    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
   
}   

 