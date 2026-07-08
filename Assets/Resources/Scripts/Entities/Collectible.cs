using RatRush.Managers;
using RatRush.ScriptableObjects;
using UnityEngine;

namespace RatRush.Entities
{
    public class Collectible : MonoBehaviour
    {
        public CollectibleSO data;
        [SerializeField] private float rotateSpeed;

        void Update()
        {
            transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
        }
        public void Collect()
        {
            GameManager.instance.score += data.scoreBonus;
            if (GameManager.instance.collectiblesData.ContainsKey(data.name))
                GameManager.instance.collectiblesData[data.name]++;
            else
                GameManager.instance.collectiblesData[data.name] = 1;

            gameObject.SetActive(false);
        }
    }
}
