using UnityEngine;

namespace Enemies
{
    public class Grave : MonoBehaviour
    {
        [SerializeField] private float _destroyTimer;

        private void Start()
        {
            Destroy(gameObject, _destroyTimer);
        }
    }
}