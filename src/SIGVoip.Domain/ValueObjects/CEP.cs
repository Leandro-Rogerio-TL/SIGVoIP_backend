using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

/// <summary>
/// Representa um CEP validado (8 dígitos numéricos). Value Object imutável.
/// </summary>
namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa um CEP validado (8 dígitos numéricos). Value Object imutável.
/// </summary>
public sealed record CEP
{
    /// <summary>
    /// O valor do CEP (apenas dígitos).
    /// </summary>
    public string Valor { get; init; }

    /// <summary>
    /// Construtor que valida e cria o Value Object CEP.
    /// Lança <see cref="InvalidCepException"/> se o CEP for inválido.
    /// Utiliza helpers de validação (<see cref="ArgumentException"/>) que são capturados e traduzidos
    /// para <see cref="InvalidCepException"/>, preservando a exceção original.
    /// </summary>
    /// <param name="valor">O valor do CEP.</param>
    /// <exception cref="InvalidCepException">Lançada se o valor do CEP for inválido.</exception>
    public CEP(string valor)
    {
        // Limpa os dígitos primeiro (inclui remoção de espaços).
        string cleanedValor = CleanDigits(valor ?? string.Empty);

        try
        {
            // Valida se a string resultante não ficou vazia após a limpeza.
            ValidateNotNullOrWhiteSpace(cleanedValor, nameof(valor));

            // Valida o comprimento exato e se é numérico no valor limpo.
            ValidateExactLength(cleanedValor, 8, nameof(valor));
            ValidateIsNumeric(cleanedValor, nameof(valor)); // Manter por enquanto, conforme discussão

            // O valor final armazenado é o valor limpo (apenas dígitos).
            Valor = cleanedValor;
        }
        catch (ArgumentException ex)
        {
            // Captura ArgumentException dos helpers e relança InvalidCepException, preservando a exceção original.
            // Passa o nome do parâmetro, o valor original, a mensagem da inner e a inner, conforme o padrão de chamada.
            throw new InvalidCepException(nameof(valor), valor ?? "null", ex.Message, ex);
        }
        // Tratamento de exceções genéricas não é feito no domínio, conforme padrão do projeto.
    }

    /// <summary>
    /// Retorna a representação formatada do CEP (00000-000).
    /// Utiliza string.Insert para adicionar o hífen.
    /// </summary>
    /// <returns>O CEP formatado.</returns>
    public override string ToString()
    {
        return Valor.Insert(5, "-");
    }
}