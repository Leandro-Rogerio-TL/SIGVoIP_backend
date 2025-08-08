namespace SIGVoip.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um Value Object ContatoTelefonico é inválido,
/// seja por dados inconsistentes ou violação de regras de coerência.
/// Segue o padrão das exceções de domínio do projeto para tratamento claro de erros.
/// </summary>
public class InvalidContatoTelefonicoException : Exception
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
    /// Inicializa uma nova instância da classe <see cref="InvalidContatoTelefonicoException"/> com uma mensagem padrão.
    /// </summary>
    public InvalidContatoTelefonicoException() : base("O Contato Telefônico fornecido é inválido.")
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidContatoTelefonicoException"/> com uma mensagem especificada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public InvalidContatoTelefonicoException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidContatoTelefonicoException"/> com uma mensagem especificada e uma InnerException.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public InvalidContatoTelefonicoException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidContatoTelefonicoException"/> com detalhes sobre a propriedade, valor tentado e uma mensagem.
    /// </summary>
    /// <param name="propertyName">O nome da propriedade ou campo relacionado à falha.</param>
    /// <param name="attemptedValue">O valor que foi tentado.</param>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public InvalidContatoTelefonicoException(string propertyName, string attemptedValue, string message)
        : base($"Contato Telefônico inválido. Campo '{propertyName}' com valor '{attemptedValue}': {message}")
    {
        PropertyName = propertyName;
        AttemptedValue = attemptedValue;
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InvalidContatoTelefonicoException"/> com detalhes sobre a propriedade, valor tentado, mensagem e uma exceção interna.
    /// Aplica o padrão de mensagem base decidido.
    /// </summary>
    /// <param name="propertyName">O nome da propriedade ou campo relacionado à falha.</param>
    /// <param name="attemptedValue">O valor que foi tentado.</param>
    /// <param name="message">A mensagem que descreve o erro (usada como fallback se a exceção interna não tiver mensagem).</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public InvalidContatoTelefonicoException(string propertyName, string attemptedValue, string message, Exception innerException)
        : base($"{propertyName ?? "Value Object"} [{attemptedValue ?? "null"}] inválido. Detalhe: {innerException?.Message ?? message}", innerException)
    {
        PropertyName = propertyName;
        AttemptedValue = attemptedValue;
    }
}