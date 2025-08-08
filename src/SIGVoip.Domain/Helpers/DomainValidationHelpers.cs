using System.Text.RegularExpressions;

namespace SIGVoip.Domain.Helpers;
// Classe utilitária interna para validações genéricas no Domínio.
// Deve ser usada SOMENTE por outras classes dentro do projeto SIGVoip.Domain.
internal static class DomainValidationHelpers
{
    /// <summary>
    /// Remove todos os caracteres não numéricos de uma string.
    /// </summary>
    internal static string CleanDigits(string value)
    {
        return Regex.Replace(value ?? string.Empty, @"[^\d]", "");
    }

    /// <summary>
    /// Valida se o valor não é nulo, vazio ou contém apenas espaços em branco.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada se o valor for nulo, vazio ou whitespace.</exception>
    internal static void ValidateNotNullOrWhiteSpace(string value, string valueName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{valueName} não pode ser nulo ou vazio.");
        }
    }

    /// <summary>
    /// Valida se o valor contém apenas dígitos numéricos.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada se o valor não for numérico.</exception>
    internal static void ValidateIsNumeric(string value, string valueName)
    {
        if (!long.TryParse(value, out _))
        {
            throw new ArgumentException($"{valueName} contém caracteres não numéricos.");
        }
    }

    /// <summary>
    /// Valida se o valor tem um tamanho exato.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada se o tamanho for diferente do esperado.</exception>
    internal static void ValidateExactLength(string value, int exactLength, string valueName)
    {
        if (value.Length != exactLength)
        {
            throw new ArgumentException($"{valueName} deve ter exatamente {exactLength} caracteres.");
        }
    }

    /// <summary>
    /// Valida se o valor tem no mínimo um determinado tamanho.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada se o tamanho for menor que o mínimo.</exception>
    internal static void ValidateMinLength(string value, int minLength, string valueName)
    {
        if (value.Length < minLength)
        {
            throw new ArgumentException($"{valueName} deve ter no mínimo {minLength} caracteres.");
        }
    }

    /// <summary>
    /// Valida se o valor tem no máximo um determinado tamanho.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada se o tamanho for maior que o máximo.</exception>
    internal static void ValidateMaxLength(string value, int maxLength, string valueName)
    {
        if (value.Length > maxLength)
        {
            throw new ArgumentException($"{valueName} deve ter no máximo {maxLength} caracteres.");
        }
    }

    /// <summary>
    /// Valida se o valor não possui todos os caracteres iguais.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada se todos os caracteres forem iguais.</exception>
    internal static void ValidateDifferentCharacters(string value, string valueName)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Distinct().Count() == 1)
        {
            throw new ArgumentException($"{valueName} não pode ter todos os dígitos iguais.");
        }
    }

    /// <summary>
    /// Valida se o valor corresponde a uma expressão regular.
    /// </summary>
     /// <exception cref="ArgumentException">Lançada se o valor não corresponder à regex.</exception>
    internal static void ValidateByRegex(string value, string regexPattern, string valueName)
    {
        if (!Regex.IsMatch(value ?? string.Empty, regexPattern))
        {
            throw new ArgumentException($"{valueName} inválido. Não corresponde ao formato esperado.");
             // Ou uma mensagem mais específica: $"{valueName} contém caracteres inválidos ou formato incorreto."
        }
    }
}