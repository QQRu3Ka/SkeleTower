using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private bool _isDoDamage;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("XROrigin");
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < 1f && !_isDoDamage)
        {
            _player.GetComponent<PlayerHealth>().TakeDamage(2);
            _isDoDamage = true;
        }
    }
}
