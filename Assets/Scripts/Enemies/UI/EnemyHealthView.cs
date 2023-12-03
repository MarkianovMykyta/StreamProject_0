using System.Collections.Generic;
using DefaultNamespace;
using Player.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.UI
{
    public class EnemyHealthView : MonoBehaviour
    {
        [SerializeField] private HealthPoint _pointTemplate;
        [SerializeField] private Vector2 _offset;

        private List<HealthPoint> _points = new List<HealthPoint>();
        
        public Enemy Enemy { get; private set; }
        private void OnDestroy()
        {
            Enemy.OnHealthChanged -= OnHealthChanged;
        }

        private void Update()
        {
            var screenPlayerPosition = (Vector2)Camera.main.WorldToScreenPoint(Enemy.transform.position);
            transform.position = screenPlayerPosition + _offset;
        }

        public void Init(Enemy enemy)
        {
            Enemy = enemy;
            InitHealthView(Enemy.MaxHealth);
            
            Enemy.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(IDestructable destructable)
        {
            if (!destructable.IsAlive)
            {
                gameObject.SetActive(false);
            }

            if (destructable.MaxHealth != _points.Count)
            {
                InitHealthView(destructable.MaxHealth);
            }

            for (var i = 0; i < _points.Count; i++)
            {
                if (i < destructable.Health)
                {
                    _points[i].Show();
                }
                else
                {
                    _points[i].Hide();
                }
            }
        }

        private void InitHealthView(int maxHealth)
        {
            for (var i = 0; i < _points.Count; i++)
            {
                Destroy(_points[i].gameObject);
            }
            
            _points.Clear();

            for (var i = 0; i < maxHealth; i++)
            {
                var point = Instantiate(_pointTemplate, _pointTemplate.transform.parent);
                point.gameObject.SetActive(true);
                _points.Add(point);
            }
        }
    }
}