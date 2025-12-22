using UnityEngine;

public class GetShotgun : MonoBehaviour
{
    [SerializeField] private GameObject _freeHand;
    [SerializeField] private GameObject _handWithPistol;
    [SerializeField] private GameObject _handWithShotgun;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RightHand"))
        {
            _freeHand.SetActive(false);
            _handWithShotgun.SetActive(true);
            _handWithPistol.SetActive(false);
        }
    }
}
