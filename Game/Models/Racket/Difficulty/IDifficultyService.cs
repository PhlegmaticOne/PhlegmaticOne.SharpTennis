using System;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Racket.Difficulty
{
    public interface IDifficultyService<out T> where T : IRacketDifficulty
    {
        T GetDifficulty(DifficultyType difficultyType);
    }

    public class PlayerDifficultyService : IDifficultyService<PlayerRacketDifficulty>
    {
        public PlayerRacketDifficulty GetDifficulty(DifficultyType difficultyType)
        {
            return new PlayerRacketDifficulty(difficultyType);
        }
    }

    public class EnemyDifficultyService : IDifficultyService<EnemyRacketDifficulty>
    {
        public EnemyRacketDifficulty GetDifficulty(DifficultyType difficultyType)
        {
            return new EnemyRacketDifficulty(difficultyType);
        }
    }

    public interface IRacketDifficulty { }

    public class PlayerRacketDifficulty : IRacketDifficulty
    {
        private readonly DifficultyType _difficulty;
        private readonly Random _random;

        public PlayerRacketDifficulty(DifficultyType difficulty)
        {
            _difficulty = difficulty;
            _random = new Random(69);
        }

        public float GetMaxLerp()
        {
            switch (_difficulty)
            {
                case DifficultyType.Easy: return 400;
                case DifficultyType.Medium: return 300;
                case DifficultyType.Hard: return 240;
                case DifficultyType.Impossible: return 320;
                default: return 300;
            }
        }
    }

    public class EnemyRacketDifficulty : IRacketDifficulty
    {
        private readonly DifficultyType _difficulty;
        private readonly Random _random;

        public EnemyRacketDifficulty(DifficultyType difficulty)
        {
            _difficulty = difficulty;
            _random = new Random(69);
        }

        public float GetFollowingLerp()
        {
            switch (_difficulty)
            {
                case DifficultyType.Easy: return 0.008f;
                case DifficultyType.Medium: return 0.01f;
                case DifficultyType.Hard: return 0.013f;
                case DifficultyType.Impossible: return 0.02f;
                default: return 0.015f;
            }
        }

        public float GetMaxPositionToKick()
        {
            switch (_difficulty)
            {
                case DifficultyType.Easy: return 25;
                case DifficultyType.Medium: return 28;
                case DifficultyType.Hard: return 31;
                case DifficultyType.Impossible: return 100;
                default: return 23;
            }
        }

        public float GenerateKnockAngle()
        {
            switch (_difficulty)
            {
                case DifficultyType.Easy: return _random.Next(35, 45);
                case DifficultyType.Medium: return _random.Next(25, 35);
                case DifficultyType.Hard: return _random.Next(15, 25);
                case DifficultyType.Impossible: return _random.Next(15, 45);
                default: return _random.Next(15, 45);
            }
        }

        public float GenerateKickAngle()
        {
            switch (_difficulty)
            {
                case DifficultyType.Easy: return _random.Next(20, 25);
                case DifficultyType.Medium: return _random.Next(15, 20);
                case DifficultyType.Hard: return _random.Next(10, 20);
                case DifficultyType.Impossible: return _random.Next(10, 25);
                default: return _random.Next(10, 25);
            }
        }

        public float GetMaxLerp()
        {
            switch (_difficulty)
            {
                case DifficultyType.Easy: return 360;
                case DifficultyType.Medium: return 300;
                case DifficultyType.Hard: return 240;
                case DifficultyType.Impossible: return 320;
                default: return 300;
            }
        }
    }
}
