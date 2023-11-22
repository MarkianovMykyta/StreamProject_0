using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        public event Action PlayClicked; 
        
        [SerializeField] private Button _playButton;
        

        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            gameObject.SetActive(false);
            PlayClicked?.Invoke();
        }
    }
}