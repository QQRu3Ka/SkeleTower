using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _enemy;
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _upperTowerPoint;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _agent.SetDestination(_endPoint.position);
    }

    private void Update()
    {
        if (Vector3.Distance(_endPoint.position, _enemy.position) < 1)
        {
            _agent.enabled = false;
            _enemy.position = _upperTowerPoint.position;
            _agent.enabled = true;
            _agent.SetDestination(_player.position);
        }
        if(Vector3.Distance(_player.position, _enemy.position) < 2)
        {
            _animator.SetInteger("skeletState", 1);
        }
    }

    public void SetDeath()
    {
        _agent.enabled = false;
        _animator.SetInteger("skeletState", 2);
        Destroy(_enemy.gameObject, 1f);
    }

    public void SetFields()
    {
        _player = GameObject.FindGameObjectWithTag("XROrigin").transform;
        _endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        _upperTowerPoint = GameObject.FindGameObjectWithTag("UpperTowerPoint").transform;
    }
}
