using UnityEngine;

public class Medkits : MonoBehaviour
{
    [SerializeField] private PointSystem _pointSystem;
    [SerializeField] private PlayerHealth _healthSystem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("RightHand") && _pointSystem.Points >= 10)
        {
            _pointSystem.RevokePoints(10);
            _healthSystem.GiveHealth(10);
        }
    }
}
