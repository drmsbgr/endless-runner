using UnityEngine;

namespace RatRush.Entities
{
    public class Bench : MonoBehaviour
    {
        private static readonly int TalkHash = Animator.StringToHash("Talk");
        [SerializeField] private Animator kid;
        [SerializeField] private Animator girl;
        public bool hasUsed;

        public void Refresh()
        {
            var state = Random.Range(0, 4);

            kid.SetFloat(TalkHash, 0f);

            if (state == 0)
            {
                kid.gameObject.SetActive(true);
                girl.gameObject.SetActive(false);
            }
            else if (state == 1)
            {
                kid.gameObject.SetActive(false);
                girl.gameObject.SetActive(true);
            }
            else if (state == 2)
            {
                kid.gameObject.SetActive(true);
                kid.SetFloat(TalkHash, 1f);
                girl.gameObject.SetActive(true);
            }
            else
            {
                kid.gameObject.SetActive(false);
                girl.gameObject.SetActive(false);
            }
        }

        public bool HasNoHuman() => !kid.gameObject.activeInHierarchy && !girl.gameObject.activeInHierarchy;
    }
}
