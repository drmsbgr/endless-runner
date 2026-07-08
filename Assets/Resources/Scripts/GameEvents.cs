using System;
using RatRush.Entities;

namespace RatRush
{
    public static class GameEvents
    {
        public static Action OnGameStart;
        public static Action OnGameOver;
        public static Action<bool> OnPlayerImpact;
        public static Action<Collectible> OnPlayerCollect;
    }
}