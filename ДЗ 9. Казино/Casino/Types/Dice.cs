using System;

namespace CasinoGame.Types
{
    public struct Dice
    {
        private readonly int _min;
        private readonly int _max;

        public int Number { get; }

        public Dice(int min, int max)
        {
            if (min < 1 || max > int.MaxValue || min > max)
                throw new WrongDiceNumberException(min, 1, int.MaxValue);

            _min = min;
            _max = max;
            Random rng = new Random();
            Number = rng.Next(min, max + 1);
        }

    }
}
