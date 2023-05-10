namespace DocStorageApi.Data.DbScripts
{
    public static class DbObjects
    {
        public static Dictionary<string, string> LoadScriptsFromFolder(string folderPath)
        {
            var scriptsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPath);
            var scriptFiles = Directory.GetFiles(scriptsFolder, "*.sql", SearchOption.TopDirectoryOnly);

            var scriptCache = new Dictionary<string, string>(scriptFiles.Length, StringComparer.OrdinalIgnoreCase);

            foreach (var scriptFile in scriptFiles)
            {
                var scriptName = Path.GetFileNameWithoutExtension(scriptFile);

                var scriptContent = File.ReadAllText(scriptFile);

                scriptCache.Add(scriptName, scriptContent);
            }

            return scriptCache;
        }
    }
}
