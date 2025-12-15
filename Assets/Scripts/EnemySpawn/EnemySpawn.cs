using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _spawnpoint;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true) {
            yield return new WaitForSeconds(4);
            var enemy = Instantiate(_enemyPrefab, _spawnpoint);
            enemy.GetComponent<EnemyMovement>().SetFields();
        }
    }
}
