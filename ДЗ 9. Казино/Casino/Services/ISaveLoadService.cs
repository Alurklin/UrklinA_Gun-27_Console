    public interface ISaveLoadService<T>
    {
        void SaveData(T data, string identifier);  // Сохранение данных
        T LoadData(string identifier);  // Загрузка данных
    }
