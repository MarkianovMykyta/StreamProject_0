using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerHealthView : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameObject _pointTemplate;
        [SerializeField] private Vector2 _offset;

        private List<GameObject> _points = new List<GameObject>();
        
        private void OnEnable()
        {
            _player.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
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
            
            if (destructable.Health > _points.Count)
            {
                var diff = destructable.Health - _points.Count;

                for (var i = 0; i < diff; i++)
                {
                    var point = Instantiate(_pointTemplate, _pointTemplate.transform.parent);
                    point.gameObject.SetActive(true);
                    _points.Add(point);
                }
            }
            else
            {
                var diff = _points.Count - destructable.Health;
                for (var i = 0; i < diff; i++)
                {
                    var point = _points[^1];
                    Destroy(point);
                    _points.RemoveAt(_points.Count-1);
                }
            }
        }
    }
}