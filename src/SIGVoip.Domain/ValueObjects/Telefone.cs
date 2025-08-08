using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa um número de telefone validado, composto por DDD, Número principal e Ramal opcional.
/// Value Object imutável. Garante o formato básico do número.
/// As informações sobre uso (Comercial/Pessoal), tipo (Fixo/Celular) e WhatsApp são modeladas
/// no Value Object ContatoTelefonico que agrega este VO.
/// </summary>
public sealed record Telefone
{
    /// <summary>
    /// Código DDD do telefone.
    /// </summary>
    public string DDD { get; init; }

    /// <summary>
    /// Número principal do telefone (8 ou 9 dígitos).
    /// </summary>
    public string Numero { get; init; }

    /// <summary>
    /// Ramal do telefone (opcional).
    /// </summary>
    public string? Ramal { get; init; }

    /// <summary>
    /// Construtor que valida e cria o Value Object Telefone.
    /// Lança <see cref="InvalidTelefoneException"/> se DDD, Número ou Ramal forem inválidos.
    /// </summary>
    /// <param name="ddd">O código DDD.</param>
    /// <param name="numero">O número principal (8 ou 9 dígitos).</param>
    /// <param name="ramal">O ramal (opcional).</param>
    public Telefone(string ddd, string numero, string? ramal = null)
    {
        string cleanDdd = CleanDigits(ddd ?? string.Empty);
        string cleanNumero = CleanDigits(numero ?? string.Empty);
        string? cleanRamal = ramal != null ? CleanDigits(ramal) : null;

        try
        {
            ValidateNotNullOrWhiteSpace(cleanDdd, nameof(ddd));
            ValidateExactLength(cleanDdd, 2, nameof(ddd));
            ValidateIsNumeric(cleanDdd, nameof(ddd));

            ValidateNotNullOrWhiteSpace(cleanNumero, nameof(numero));
            ValidateMinLength(cleanNumero, 8, nameof(numero));
            ValidateMaxLength(cleanNumero, 9, nameof(numero));
            ValidateIsNumeric(cleanNumero, nameof(numero));
            ValidateDifferentCharacters(cleanNumero, nameof(numero));

            if (!string.IsNullOrWhiteSpace(cleanRamal))
            {
                ValidateMinLength(cleanRamal, 1, nameof(ramal));
                ValidateMaxLength(cleanRamal, 5, nameof(ramal));
                ValidateIsNumeric(cleanRamal, nameof(ramal));
            }

            DDD = cleanDdd;
            Numero = cleanNumero;
            Ramal = cleanRamal;
        }
        catch (ArgumentException ex)
        {
            // Exceções de helpers de validação são traduzidas para InvalidTelefoneException.
            throw new InvalidTelefoneException("Telefone", $"{ddd ?? "null"} {numero ?? "null"} {ramal ?? "null"}", ex.Message, ex);
        }
    }

    /// <summary>
    /// Retorna a representação formatada do telefone (ex: (DD) NNNN-NNNN, (DD) NNNNN-NNNN, (DD) NNNN-NNNN rXXX).
    /// </summary>
    /// <returns>O telefone formatado como string.</returns>
    public override string ToString()
    {
        string formattedNumero = Numero.Length == 8 ? Numero.Insert(4,"-") : Numero.Insert(5,"-");

        string result = $"({DDD}) {formattedNumero}";

        return string.IsNullOrWhiteSpace(Ramal) ? result : $"{result} - R: {Ramal}";
    }
}