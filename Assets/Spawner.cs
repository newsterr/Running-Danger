using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefebs;
    [SerializeField] private Transform obstacleParent;

    public float obstacleSpawnTime = 3f;
    [Range(0,1)] public float ObstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 4f;
    [Range(0,1)] public float ObstacleSpeedFactor = 0.2f;

    private float _obstacleSpawnTime;
    private float _obstacleSpeed;

    private float timeAlive;
    private float timeUntilObstacleSpawn;

    private void Start() {
        GameManager.Instance.onGameOver.AddListener(ClearObstacles);
        GameManager.Instance.onPlay.AddListener(ResetFactors);
    }

     private void Update() {
        if (GameManager.Instance.isPlaying){
            timeAlive += Time.deltaTime;

            CalculateFactors();

            SpawnLoop();
        }
    }

    private void SpawnLoop() {
        timeUntilObstacleSpawn += Time.deltaTime;

        if(timeUntilObstacleSpawn >= _obstacleSpawnTime) {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles() {
        foreach(Transform child in obstacleParent){
            Destroy(child.gameObject);
        }
    }

    private void CalculateFactors() {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive,ObstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive,ObstacleSpeedFactor);
    }

    private void ResetFactors() {
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn() {
        GameObject obstacleToSpawn = obstaclePrefebs[Random.Range(0, obstaclePrefebs.Length)];

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent;


        Rigidbody2D obsacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obsacleRB.velocity = Vector2.left * _obstacleSpeed;
    }
}
