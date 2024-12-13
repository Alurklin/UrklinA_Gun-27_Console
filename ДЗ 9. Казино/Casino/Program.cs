using CasinoGame.Mechanics;
using CasinoGame.Services;
using System;

class Program
{
    static void Main()
    {
        var saveLoadService = new FileSystemSaveLoadService("path_to_save_data"); // Путь к директории для сохранения
        var casino = new Casino(saveLoadService);
        casino.StartGame();
    }
}
