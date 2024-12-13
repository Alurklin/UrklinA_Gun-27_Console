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
                Console.WriteLine($"Welcome back, {_playerProfile.Name}!");
            }

            // Выбор игры
            Console.WriteLine("Choose a game: 1 - Blackjack, 2 - Dice");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
            {
                Console.WriteLine("Invalid choice. Please enter 1 for Blackjack or 2 for Dice.");
            }

            // Игровой процесс
            if (choice == 1)
            {
                var game = new BlackjackGame(_playerProfile, 52); // Пример с 52 картами
                game.OnWin += () => Console.WriteLine("You won!");
                game.OnLoose += () => Console.WriteLine("You lost!");
                game.OnDraw += () => Console.WriteLine("It's a draw!");

                game.PlayGame();
            }
            else if (choice == 2)
            {
                var game = new DiceGame(_playerProfile, 5, 1, 6); // Пример с 5 костями
                game.OnWin += () => Console.WriteLine("You won!");
                game.OnLoose += () => Console.WriteLine("You lost!");
                game.OnDraw += () => Console.WriteLine("It's a draw!");

                game.PlayGame();
            }

            // Сохранение профиля
            _saveLoadService.SaveData(_playerProfile.Name, "playerProfile");
            Console.WriteLine("Your profile has been saved.");

            // Добавляем паузу перед выходом из программы, чтобы пользователь успел увидеть результат
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(); // Ожидаем нажатие клавиши
        }
    }
}
