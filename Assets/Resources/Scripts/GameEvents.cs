using System;
using RatRush.Contracts;

namespace RatRush
{
    public static class GameEvents
    {
        public static Action OnGameStart;
        public static Action OnGameOver;
        public static Action<bool> OnPlayerImpact;
        public static Action<ICollectible> OnPlayerCollect;
    }
}