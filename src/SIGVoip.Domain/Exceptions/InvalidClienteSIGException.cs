namespace SIGVoip.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando a entidade ClienteSIG ou seus componentes são inválidos.
/// </summary>
public class InvalidClienteSIGException : Exception
{
    /// <summary>
    /// Nome da propriedade ou campo relacionado à falha de validação.
    /// </summary>
    public string? PropertyName { get; }

    /// <summary>
    /// O valor que foi tentado e causou a falha de validação.
    /// </summary>
    public string? AttemptedValue { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidClienteSIGException"/>.
    /// </summary>
    public InvalidClienteSIGException() : base("O ClienteSIG fornecido é inválido.") { }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidClienteSIGException"/> com uma mensagem.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public InvalidClienteSIGException(string message) : base(message) { }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidClienteSIGException"/> com uma mensagem e exceção interna.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public InvalidClienteSIGException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidClienteSIGException"/> com detalhes da falha.
    /// </summary>
    /// <param name="propertyName">O nome da propriedade ou campo relacionado.</param>
    /// <param name="attemptedValue">O valor que foi tentado.</param>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public InvalidClienteSIGException(string propertyName, string attemptedValue, string message)
        : base($"ClienteSIG inválido. Campo '{propertyName}' com valor '{attemptedValue}': {message}")
    {
        PropertyName = propertyName;
        AttemptedValue = attemptedValue;
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidClienteSIGException"/> com detalhes da falha e exceção interna.
    /// </summary>
    /// <param name="propertyName">O nome da propriedade ou campo relacionado.</param>
    /// <param name="attemptedValue">O valor que foi tentado.</param>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public InvalidClienteSIGException(string propertyName, string attemptedValue, string message, Exception innerException)
        : base($"{propertyName ?? "ClienteSIG"} [{attemptedValue ?? "null"}] inválido. Detalhe: {innerException?.Message ?? message}", innerException)
    {
        PropertyName = propertyName;
        AttemptedValue = attemptedValue;
    }
}