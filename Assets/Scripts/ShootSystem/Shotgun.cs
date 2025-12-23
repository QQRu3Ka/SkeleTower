using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private InputActionProperty _action;

    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _curAmmo;
    [SerializeField] private int _reloadTime;

    [SerializeField] private Transform _pistolEnd;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GameObject _reloadCanvas;
    [SerializeField] private PointSystem _pointSystem;

    private bool _isReloading;

    private void Start()
    {
        _action.action.Enable();

        _action.action.performed += ctx => Shoot();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        _reloadCanvas.SetActive(true);
        yield return new WaitForSeconds(_reloadTime);
        _reloadCanvas.SetActive(false);
        _curAmmo = _maxAmmo;
        _isReloading = false;
    }

    private void Shoot()
    {
        if (_isReloading) return;
        if (_curAmmo == 0)
        {
            StartCoroutine(Reload());
        }
        else
        {
            _curAmmo--;
            if (Physics.Raycast(_pistolEnd.position, GetPelletDirection(_pistolEnd.forward), out RaycastHit hit))
            {
                var obj = hit.transform.gameObject;
                if (obj.CompareTag("Enemy"))
                {
                    _pointSystem.AddOnePoint();
                    obj.GetComponent<EnemyMovement>().SetDeath();
                }
                var effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 1f);
            }
            if (Physics.Raycast(_pistolEnd.position, GetPelletDirection(_pistolEnd.forward), out hit))
            {
                var obj = hit.transform.gameObject;
                if (obj.CompareTag("Enemy"))
                {
                    _pointSystem.AddOnePoint();
                    obj.GetComponent<EnemyMovement>().SetDeath();
                }
                var effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 1f);
            }
            if (Physics.Raycast(_pistolEnd.position, GetPelletDirection(_pistolEnd.forward), out hit))
            {
                var obj = hit.transform.gameObject;
                if (obj.CompareTag("Enemy"))
                {
                    _pointSystem.AddOnePoint();
                    obj.GetComponent<EnemyMovement>().SetDeath();
                }
                var effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 1f);
            }
            if (Physics.Raycast(_pistolEnd.position, GetPelletDirection(_pistolEnd.forward), out hit))
            {
                var obj = hit.transform.gameObject;
                if (obj.CompareTag("Enemy"))
                {
                    _pointSystem.AddOnePoint();
                    obj.GetComponent<EnemyMovement>().SetDeath();
                }
                var effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 1f);
            }
            if (Physics.Raycast(_pistolEnd.position, GetPelletDirection(_pistolEnd.forward), out hit))
            {
                var obj = hit.transform.gameObject;
                if (obj.CompareTag("Enemy"))
                {
                    _pointSystem.AddOnePoint();
                    obj.GetComponent<EnemyMovement>().SetDeath();
                }
                var effect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 1f);
            }
        }
    }

    private Vector3 GetPelletDirection(Vector3 baseDirection)
    {
        var horizontalAngle = Random.Range(-5, 5); 
        var verticalAngle = Random.Range(-5, 5);

        var spreadRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        return spreadRotation * baseDirection;
    }
}
