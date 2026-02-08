namespace NeorisBackend.DTOs.Responses
{
    /// <summary>
    /// DTO genérico de respuesta para operaciones exitosas
    /// </summary>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// Datos devueltos por la operación
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Mensaje adicional sobre la operación
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Verdadero si la operación fue exitosa, falso en caso contrario
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// Constructor por defecto que inicializa una respuesta exitosa
        /// </summary>
        public ApiResponseDto()
        {
            Success = true;
        }

        /// <summary>
        /// Constructor que inicializa una respuesta exitosa con datos y un mensaje opcional
        /// </summary>
        /// <param name="data">Datos devueltos por la operación</param>
        /// <param name="message">Mensaje adicional sobre la operación</param>
        public ApiResponseDto(T data, string message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }
    }
}
