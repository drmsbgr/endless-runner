using UnityEngine;

namespace RatRush.Utilities
{
    [ExecuteAlways]
    public class MultiAligner : MonoBehaviour
    {
        [SerializeField] private float offset = 10f;
        public bool curve;
        [SerializeField] private float height = 2f;

        void Update()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                float y;

                if (curve)
                {
                    var t = (float)i / (transform.childCount - 1);
                    y = height * Mathf.Pow(Mathf.Sin(Mathf.PI * t), 2f);
                }
                else
                {
                    y = 0f;
                }

                var pos = y * transform.up + i * offset * transform.forward;

                transform.GetChild(i).localPosition = pos;
            }
        }
    }
}
