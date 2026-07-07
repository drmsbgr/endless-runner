using System;
using RatRush.Contracts;

namespace RatRush
{
    public static class GameEvents
    {
        public static Action OnPlayerDead;
        public static Action<ICollectible> OnPlayerCollect;
    }
}