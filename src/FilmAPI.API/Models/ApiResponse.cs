namespace FilmAPI.API.Models
{
    /// <summary>
    /// API yanıtları için standart model
    /// </summary>
    /// <typeparam name="T">Yanıt veri tipi</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// İşlem başarılı mı?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// İşlem mesajı
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Yanıt verisi
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Başarılı yanıt oluşturur
        /// </summary>
        /// <param name="data">Yanıt verisi</param>
        /// <param name="message">İşlem mesajı</param>
        /// <returns>API yanıtı</returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "İşlem başarılı")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Hata yanıtı oluşturur
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <returns>API yanıtı</returns>
        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
} 