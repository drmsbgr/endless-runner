using UnityEngine;

namespace RatRush.Entities
{
    public class Trashcan : MonoBehaviour
    {
        [SerializeField] private GameObject normal;
        [SerializeField] private GameObject fell;

        public void Refresh()
        {
            var state = Random.Range(0, 3);

            if (state == 0)
            {
                normal.SetActive(false);
                fell.SetActive(false);
            }
            else if (state == 1)
            {
                normal.SetActive(true);
                fell.SetActive(false);
            }
            else
            {
                normal.SetActive(false);
                fell.SetActive(true);
            }
        }
    }
}
