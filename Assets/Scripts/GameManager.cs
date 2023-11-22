using System;
using Enemies;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private Player _player;
    [SerializeField] private Menu.Menu _menu;

    private void Awake()
    {
        _enemiesSpawner.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);

        _menu.PlayClicked += OnPlayClicked;
    }

    private void OnPlayClicked()
    {
        StartGame();
    }

    private void StartGame()
    {
        _enemiesSpawner.gameObject.SetActive(true);
        _player.gameObject.SetActive(true);
    }
}