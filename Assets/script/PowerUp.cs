using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;
    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8.37f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"PowerUp.OnTriggerEnter2D: collided with '{other.name}' tag='{other.tag}' id={_powerupID}");

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (_clip != null)
            {
                AudioSource.PlayClipAtPoint(_clip, transform.position);
            }
            else
            {
                Debug.LogWarning("PowerUp.OnTriggerEnter2D: _clip is null. No sound will be played.");
            }

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        Debug.Log("PowerUp: Applying Triple Shot to player");
                        player.TripleShotActive();
                        break;

                    case 1:
                        Debug.Log("PowerUp: Applying Speed PowerUp to player");
                        player.SpeedPowerUpActive();
                        break;

                    case 2:
                        Debug.Log("PowerUp: Applying Shield PowerUp to player");
                        player.ShieldPowerUpActive();
                        break;

                    default:
                        Debug.Log("PowerUp.OnTriggerEnter2D: Unknown powerupID");
                        break;
                }
            }
            else
            {
                Debug.LogWarning("PowerUp.OnTriggerEnter2D: Player component missing on collided GameObject.");
            }

            Destroy(this.gameObject);
        }
    }
}
