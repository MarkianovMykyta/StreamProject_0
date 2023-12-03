using UnityEngine;

namespace Player.UI
{
    public class HealthPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _pointImg;

        public void Hide()
        {
            _pointImg.SetActive(false);
        }

        public void Show()
        {
            _pointImg.SetActive(true);
        }
    }
}