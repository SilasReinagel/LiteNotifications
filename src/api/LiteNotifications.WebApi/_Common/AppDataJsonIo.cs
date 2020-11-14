using System;
using System.IO;
using System.Text.Json;

namespace LiteNotifications.WebApi._Common
{
    public sealed class AppDataJsonIo : Io
    {
        private readonly string _appStorageFolder;

        public AppDataJsonIo(string appFolderName)
        {
            _appStorageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appFolderName);
        }

        public void Put<T>(string name, T data)
        {
            if (!Directory.Exists(_appStorageFolder))
                Directory.CreateDirectory(_appStorageFolder);
            File.WriteAllText(GetFilePath(name), JsonSerializer.Serialize(data));
        }

        public T Get<T>(string name) => JsonSerializer.Deserialize<T>(File.ReadAllText(GetFilePath(name)));
        public bool Contains(string name) => File.Exists(GetFilePath(name));
        public void Delete(string name) => File.Delete(GetFilePath(name));
        private string GetFilePath(string name) => Path.Combine(_appStorageFolder, name + ".json");
    }
}
