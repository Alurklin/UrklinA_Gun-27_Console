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
            _playerProfile = new PlayerProfile("Player", 1000);
        }

        public void StartGame()
        {
            Console.WriteLine("Welcome to the Casino!");

            // Загрузка профиля игрока
            string playerData = _saveLoadService.LoadData("playerProfile");

            if (!string.IsNullOrEmpty(playerData))
                _playerProfile = new PlayerProfile(playerData, 1000);

            // Выбор игры
            Console.WriteLine("Choose a game: 1 - Blackjack, 2 - Dice");
            int choice = int.Parse(Console.ReadLine());

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
        }
    }
}
