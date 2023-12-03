using System;
using System.Collections.Generic;
using DefaultNamespace;
using Player.UI;
using UnityEngine;

namespace Player
{
    public class PlayerHealthView : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private HealthPoint _pointTemplate;
        [SerializeField] private Vector2 _offset;

        private List<HealthPoint> _points = new List<HealthPoint>();
        
        private void Start()
        {
            _player.OnHealthChanged += OnHealthChanged;
            OnHealthChanged(_player);
        }

        private void OnDestroy()
        {
            _player.OnHealthChanged -= OnHealthChanged;
        }

        private void Update()
        {
            var screenPlayerPosition = (Vector2)Camera.main.WorldToScreenPoint(_player.transform.position);
            transform.position = screenPlayerPosition + _offset;
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