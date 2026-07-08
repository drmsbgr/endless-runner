using RatRush.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RatRush.UI
{
    public class UICollectible : MonoBehaviour
    {
        [HideInInspector] public CollectibleSO data;
        [SerializeField] private Image collectibleImage;
        [SerializeField] private TextMeshProUGUI countLabel;

        public void Set(CollectibleSO data)
        {
            this.data = data;
            collectibleImage.sprite = data.collectibleIcon;
        }

        public void SetCount(int v)
        {
            countLabel.text = v.ToString();
            if (v == 0)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
    }
}
