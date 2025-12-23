using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private EnemySpawn _enemySpawn;
    [SerializeField] private GameObject _canvas;

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            StartWaves();
        }
    }
    public void StartWaves()
    {
        _canvas.SetActive(false);
        _enemySpawn.StartCoroutine(_enemySpawn.SpawnEnemies());
    }
}
