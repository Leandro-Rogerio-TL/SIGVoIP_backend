using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;
using System.Net.Mail;

namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa um endereço de email validado.
/// Value Object imutável. Garante o formato básico e a validade sintática do email.
/// </summary>
public sealed record Email
{
    /// <summary>
    /// O endereço de email.
    /// </summary>
    public string Valor { get; init; }

    /// <summary>
    /// Construtor que valida e cria o Value Object Email.
    /// Lança <see cref="InvalidEmailException"/> se o email for inválido.
    /// Utiliza <see cref="System.Net.Mail.MailAddress"/> para validação sintática robusta.
    /// Captura exceções esperadas como <see cref="ArgumentException"/> (de helpers) e <see cref="FormatException"/>
    /// (de <see cref="System.Net.Mail.MailAddress"/>) e as traduz para <see cref="InvalidEmailException"/>,
    /// encapsulando a exceção original.
    /// </summary>
    /// <param name="valor">O endereço de email.</param>
    /// <exception cref="InvalidEmailException">Lançada se o formato do email for inválido.</exception>
    public Email(string valor)
    {
        string cleanValor = (valor ?? string.Empty).Trim();

        try
        {
            ValidateNotNullOrWhiteSpace(cleanValor, nameof(valor));

            new MailAddress(cleanValor);

            Valor = cleanValor;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidEmailException(nameof(valor), valor ?? "null", ex.Message, ex);
        }
        catch (FormatException ex)
        {
             throw new InvalidEmailException(nameof(valor), valor ?? "null", ex.Message, ex);
        }
    }

    /// <summary>
    /// Retorna o endereço de email como string.
    /// </summary>
    /// <returns>O endereço de email.</returns>
    public override string ToString()
    {
        return Valor;
    }
}