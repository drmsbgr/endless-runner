using RatRush.Enums;
using UnityEngine;

namespace RatRush.Controllers
{
    public class PlayerAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource SFXSource;
        [SerializeField] private AudioClip impactMetal;
        [SerializeField] private AudioClip squish;
        [SerializeField] private AudioClip impact3;

        public void Damage()
        {
            SFXSource.PlayOneShot(impact3);
        }

        public void Crush(ObstacleType obstacleType)
        {
            if (obstacleType == ObstacleType.Human)
                SFXSource.PlayOneShot(squish, Random.value + .5f);
            else
                SFXSource.PlayOneShot(impactMetal, Random.value + .5f);
        }
    }
}