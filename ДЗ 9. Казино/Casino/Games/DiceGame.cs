using CasinoGame.Profiles;
using CasinoGame.Types;
using System;
using System.Collections.Generic;

namespace CasinoGame.Games
{
    public class DiceGame : CasinoGameBase
    {
        private readonly int _numDice;
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly PlayerProfile _playerProfile;

        private List<Dice> _playerDice;
        private List<Dice> _computerDice;

        public DiceGame(PlayerProfile playerProfile, int numDice, int minValue, int maxValue)
        {
            if (numDice <= 0)
                throw new ArgumentException("Number of dice should be greater than zero.");
            if (minValue < 1 || maxValue > int.MaxValue || minValue > maxValue)
                throw new ArgumentException("Invalid range for dice values.");

            _playerProfile = playerProfile;
            _numDice = numDice;
            _minValue = minValue;
            _maxValue = maxValue;

            FactoryMethod(); // создаём кости
        }

        protected override void FactoryMethod()
        {
            _playerDice = new List<Dice>();
            _computerDice = new List<Dice>();

            // Создание костей для игрока
            for (int i = 0; i < _numDice; i++)
            {
                _playerDice.Add(new Dice(_minValue, _maxValue));
            }

            // Создание костей для компьютера
            for (int i = 0; i < _numDice; i++)
            {
                _computerDice.Add(new Dice(_minValue, _maxValue));
            }
        }

        // Метод подсчёта суммы значений костей
        private int GetDiceSum(List<Dice> diceList)
        {
            int sum = 0;
            foreach (var dice in diceList)
            {
                sum += dice.Number;
            }
            return sum;
        }

        // Метод для вывода результатов бросков костей
        private void PrintDiceResults()
        {
            Console.WriteLine("Your dice roll: ");
            foreach (var dice in _playerDice)
            {
                Console.WriteLine($"[{dice.Number}]");
            }
            Console.WriteLine($"Total: {GetDiceSum(_playerDice)}");

            Console.WriteLine("\nComputer's dice roll: ");
            foreach (var dice in _computerDice)
            {
                Console.WriteLine($"[{dice.Number}]");
            }
            Console.WriteLine($"Total: {GetDiceSum(_computerDice)}");
        }

        public override void PlayGame()
        {
            // Выводим результаты бросков костей
            PrintDiceResults();

            // Подсчитываем сумму бросков игрока и компьютера
            int playerTotal = GetDiceSum(_playerDice);
            int computerTotal = GetDiceSum(_computerDice);

            // Определяем победителя
            if (playerTotal > computerTotal)
            {
                PrintResult("You win!");
                OnWinInvoke();
            }
            else if (playerTotal < computerTotal)
            {
                PrintResult("You lose!");
                OnLooseInvoke();
            }
            else
            {
                PrintResult("It's a draw!");
                OnDrawInvoke();
            }

            // Если у игрока деньги 0, завершить игру
            if (_playerProfile.Money <= 0)
            {
                Console.WriteLine("No money? Kicked!");
            }
        }
    }
}
