namespace FilmAPI.Core.Helpers
{
    public static class CacheKeys
    {
        public const string AllMovies = "AllMovies";
        public static string MovieById(int id) => $"Movie_{id}";
    }
} 