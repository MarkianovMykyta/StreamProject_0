using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [SerializeField] private float _enemyAmount;
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemiesRoot;
        [SerializeField] private float _delayBetweenSpawn;

        public readonly List<Enemy> AliveEnemies = new List<Enemy>();

        private void Update()
        {
            if (AliveEnemies.Count == 0)
            {
                SpawnNewWave();
            }
        }

        private async void SpawnNewWave()
        {
            for (var i = 0; i < _enemyAmount; i++)
            {
                var nextPosition = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
                var enemy = Instantiate(_enemyPrefab, nextPosition, Quaternion.identity, _enemiesRoot);

                enemy.Died += OnEnemyDied;
                
                AliveEnemies.Add(enemy);

                await Task.Delay(TimeSpan.FromSeconds(_delayBetweenSpawn));
            }
        }

        private void OnEnemyDied(Enemy deadEnemy)
        {
            deadEnemy.Died -= OnEnemyDied;
            AliveEnemies.Remove(deadEnemy);
        }
    }
}