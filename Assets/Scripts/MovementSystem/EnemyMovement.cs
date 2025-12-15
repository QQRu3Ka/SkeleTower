using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _enemy;
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _upperTowerPoint;

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
    }

    public void SetFields()
    {
        _player = GameObject.FindGameObjectWithTag("XROrigin").transform;
        _endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        _upperTowerPoint = GameObject.FindGameObjectWithTag("UpperTowerPoint").transform;
    }
}
