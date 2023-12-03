using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemies.UI;
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
        [SerializeField] private EnemyHealthView _enemyHealthViewPrefab;
        [SerializeField] private Transform _enemiesRoot;
        [SerializeField] private Transform _healthViewRoot;
        [SerializeField] private float _delayBetweenSpawn;

        public readonly List<Enemy> AliveEnemies = new List<Enemy>();

        private List<EnemyHealthView> _enemyHealthViews = new List<EnemyHealthView>();
        
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
                if (destroyCancellationToken.IsCancellationRequested)
                {
                    return;
                }
                var nextPosition = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
                var enemy = Instantiate(_enemyPrefab, nextPosition, Quaternion.identity, _enemiesRoot);
                enemy.Died += OnEnemyDied;

                var enemyHealthView = Instantiate(_enemyHealthViewPrefab, nextPosition, Quaternion.identity, _healthViewRoot);
                _enemyHealthViews.Add(enemyHealthView);
                
                enemyHealthView.Init(enemy);
                
                AliveEnemies.Add(enemy);
                
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_delayBetweenSpawn), destroyCancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }
        }

        private void OnEnemyDied(Enemy deadEnemy)
        {
            deadEnemy.Died -= OnEnemyDied;
            AliveEnemies.Remove(deadEnemy);

            for (var i = _enemyHealthViews.Count-1; i >=0; i--)
            {
                var view = _enemyHealthViews[i];
                if (view.Enemy == deadEnemy)
                {
                    Destroy(view.gameObject);
                    _enemyHealthViews.RemoveAt(i);
                    break;
                }
            }
        }
    }
}