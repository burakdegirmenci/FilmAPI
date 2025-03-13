using Dapper;
using FilmAPI.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FilmAPI.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Veritabanı ve tablo oluşturma
            await CreateDatabaseIfNotExists(connectionString);
            
            // Örnek veri ekleme
            await SeedMovies(connectionString);
        }
        
        private static async Task CreateDatabaseIfNotExists(string connectionString)
        {
            // Master veritabanına bağlanma
            var builder = new SqlConnectionStringBuilder(connectionString);
            var database = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                await connection.OpenAsync();
                
                // Veritabanı var mı kontrol et
                var checkDbQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{database}'";
                var dbExists = await connection.ExecuteScalarAsync<int>(checkDbQuery) > 0;
                
                if (!dbExists)
                {
                    // Veritabanı oluştur
                    var createDbQuery = $"CREATE DATABASE [{database}]";
                    await connection.ExecuteAsync(createDbQuery);
                }
            }
            
            // Yeni veritabanına bağlan ve tabloyu oluştur
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                
                // Movies tablosunu oluştur
                var createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movies')
                BEGIN
                    CREATE TABLE Movies (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Title NVARCHAR(255) NOT NULL,
                        Director NVARCHAR(255) NOT NULL,
                        ReleaseYear INT NOT NULL,
                        Genre NVARCHAR(100) NOT NULL,
                        Rating DECIMAL(2,1) NOT NULL
                    )
                END";
                
                await connection.ExecuteAsync(createTableQuery);
            }
        }
        
        private static async Task SeedMovies(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                
                // Veritabanında film sayısını kontrol et
                var movieCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Movies");
                
                // Eğer film yoksa, örnek filmler ekle
                if (movieCount == 0)
                {
                    var movies = GenerateMovies();
                    var query = @"
                        INSERT INTO Movies (Title, Director, ReleaseYear, Genre, Rating)
                        VALUES (@Title, @Director, @ReleaseYear, @Genre, @Rating)";
                    
                    await connection.ExecuteAsync(query, movies);
                }
            }
        }
        
        private static Movie[] GenerateMovies()
        {
            return new Movie[]
            {
                new Movie { Title = "Esaretin Bedeli", Director = "Frank Darabont", ReleaseYear = 1994, Genre = "Dram", Rating = 9.3m },
                new Movie { Title = "Baba", Director = "Francis Ford Coppola", ReleaseYear = 1972, Genre = "Suç/Dram", Rating = 9.2m },
                new Movie { Title = "Kara Şövalye", Director = "Christopher Nolan", ReleaseYear = 2008, Genre = "Aksiyon", Rating = 9.0m },
                new Movie { Title = "Yüzüklerin Efendisi: Kralın Dönüşü", Director = "Peter Jackson", ReleaseYear = 2003, Genre = "Fantastik", Rating = 8.9m },
                new Movie { Title = "Pulp Fiction", Director = "Quentin Tarantino", ReleaseYear = 1994, Genre = "Suç/Dram", Rating = 8.9m },
                new Movie { Title = "Schindler'in Listesi", Director = "Steven Spielberg", ReleaseYear = 1993, Genre = "Biyografi/Dram", Rating = 8.9m },
                new Movie { Title = "Dövüş Kulübü", Director = "David Fincher", ReleaseYear = 1999, Genre = "Dram", Rating = 8.8m },
                new Movie { Title = "Forrest Gump", Director = "Robert Zemeckis", ReleaseYear = 1994, Genre = "Dram/Komedi", Rating = 8.8m },
                new Movie { Title = "Başlangıç", Director = "Christopher Nolan", ReleaseYear = 2010, Genre = "Bilim Kurgu", Rating = 8.7m },
                new Movie { Title = "Matrix", Director = "Lana ve Lilly Wachowski", ReleaseYear = 1999, Genre = "Bilim Kurgu", Rating = 8.7m }
            };
        }
    }
} 