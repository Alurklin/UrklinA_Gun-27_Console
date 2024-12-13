using CasinoGame.Profiles;
using CasinoGame.Games;
using System;

namespace CasinoGame.Mechanics
{
    public class Casino : IGame
    {
        private PlayerProfile _playerProfile;
        private ISaveLoadService<string> _saveLoadService;

        public Casino(ISaveLoadService<string> saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _playerProfile = null; // Изначально профиль не загружен
        }

        public void StartGame()
        {
            Console.WriteLine("Welcome to the Casino!");

            // Загрузка профиля игрока
            string playerData = _saveLoadService.LoadData("playerProfile");

            if (string.IsNullOrEmpty(playerData))
            {
                // Если данных нет, создаем новый профиль
                Console.WriteLine("No profile found. Please enter your name:");
                string playerName = Console.ReadLine();

                // Профиль создается с 1000 деньгами
                _playerProfile = new PlayerProfile(playerName, 1000);
                Console.WriteLine($"Welcome, {_playerProfile.Name}! You have been given $1000.");
            }
            else
            {
                // Если профиль найден, загружаем его
                _playerProfile = new PlayerProfile(playerData, 1000); // Профиль с 1000 деньгами, для примера
                Console.WriteLine($"Welcome back, {_playerProfile.Name}! You have ${_playerProfile.Money}.");
            }

            // Ввод ставки
            decimal betAmount = 0;
            bool validBet = false;

            while (!validBet)
            {
                Console.WriteLine($"You have ${_playerProfile.Money}. Enter your bet:");

                // Проверка ввода ставки
                string input = Console.ReadLine();
                validBet = decimal.TryParse(input, out betAmount) && betAmount > 0 && betAmount <= _playerProfile.Money;

                if (!validBet)
                {
                    Console.WriteLine($"Invalid bet. You have ${_playerProfile.Money}, please enter a valid bet.");
                }
            }

            // Игровой процесс
            int choice = 0;
            bool validChoice = false;

            while (!validChoice)
            {
                Console.WriteLine("Choose a game: 1 - Blackjack, 2 - Dice");
                string input = Console.ReadLine();
                validChoice = int.TryParse(input, out choice) && (choice == 1 || choice == 2);

                if (!validChoice)
                {
                    Console.WriteLine("Invalid choice. Please enter 1 for Blackjack or 2 for Dice.");
                }
            }

            // Игровой процесс
            if (choice == 1)
            {
                var game = new BlackjackGame(_playerProfile, 52); // Пример с 52 картами
                game.OnWin += () =>
                {
                    Console.WriteLine("You won!");
                    _playerProfile.Money += (int)betAmount; // Добавляем ставку к деньгам игрока
                };
                game.OnLoose += () =>
                {
                    Console.WriteLine("You lost!");
                    _playerProfile.Money -= (int)betAmount; // Вычитаем ставку из денег игрока
                };
                game.OnDraw += () =>
                {
                    Console.WriteLine("It's a draw!");
                    // Деньги не изменяются в случае ничьей
                };

                game.PlayGame();
            }
            else if (choice == 2)
            {
                var game = new DiceGame(_playerProfile, 5, 1, 6); // Пример с 5 костями
                game.OnWin += () =>
                {
                    Console.WriteLine("You won!");
                    _playerProfile.Money += (int)betAmount; // Добавляем ставку к деньгам игрока
                };
                game.OnLoose += () =>
                {
                    Console.WriteLine("You lost!");
                    _playerProfile.Money -= (int)betAmount; // Вычитаем ставку из денег игрока
                };
                game.OnDraw += () =>
                {
                    Console.WriteLine("It's a draw!");
                    // Деньги не изменяются в случае ничьей
                };

                game.PlayGame();
            }

            // Если у игрока нет денег, выводим сообщение
            if (_playerProfile.Money <= 0)
            {
                Console.WriteLine("No money? Kicked!");
            }

            // Если у игрока больше денег, чем максимально возможное значение, уменьшение суммы
            if (_playerProfile.Money > 10000)
            {
                _playerProfile.Money /= 2;
                Console.WriteLine("You wasted half of your bank money in casino’s bar!");
            }

            // Сохранение профиля с актуальным количеством денег
            _saveLoadService.SaveData(_playerProfile.Name, "playerProfile");
            Console.WriteLine("Your profile has been saved.");

            // Добавляем паузу перед выходом из программы, чтобы пользователь успел увидеть результат
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); // Ожидаем нажатие клавиши
        }
    }
}
