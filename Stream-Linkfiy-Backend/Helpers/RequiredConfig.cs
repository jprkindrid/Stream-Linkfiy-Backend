namespace Stream_Linkfiy_Backend.Helpers
{
    public static class RequiredConfig
    {
        public static string Get(IConfiguration config, string key)
        {
            var value = config[key]?.Trim();
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Configuration key '{key}' is missing or empty.");
            }
            return value;
        }
    }
}
