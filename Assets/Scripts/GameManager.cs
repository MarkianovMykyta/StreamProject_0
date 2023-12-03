using System;
using Enemies;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private GameObject _playerRoot;
    [SerializeField] private Menu.Menu _menu;

    private void Awake()
    {
        _enemiesSpawner.gameObject.SetActive(false);
        _playerRoot.SetActive(false);

        _menu.PlayClicked += OnPlayClicked;
    }

    private void OnPlayClicked()
    {
        StartGame();
    }

    private void StartGame()
    {
        _enemiesSpawner.gameObject.SetActive(true);
        _playerRoot.SetActive(true);
    }
}