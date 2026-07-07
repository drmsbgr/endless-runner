using UnityEngine;
using UnityEngine.UIElements;

namespace RatRush
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;

        void Update()
        {
            transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }
    }
}
