namespace Stream_Linkfiy_Backend.Helpers
{
    public static class SpotifyConcurrency
    {
        public static readonly SemaphoreSlim GlobalSemaphore = new SemaphoreSlim(5,5);
    }
}
