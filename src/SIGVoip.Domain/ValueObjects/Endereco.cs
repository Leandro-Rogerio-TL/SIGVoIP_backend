using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

/// <summary>
/// Representa um endereço validado, agregando o Value Object CEP. Value Object imutável.
/// </summary>
namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa um endereço validado, agregando o Value Object CEP. Value Object imutável.
/// </summary>
public sealed record Endereco
{
    /// <summary>
    /// Logradouro do endereço. Não pode ser nulo, vazio ou apenas whitespace.
    /// </summary>
    public string Logradouro { get; init; }

    /// <summary>
    /// Número do endereço (opcional). Pode ser nulo ou string.Empty. Se for fornecido (não null, empty, ou whitespace), pode ter validações de formato.
    /// </summary>
    public string? Numero { get; init; }

    /// <summary>
    /// Complemento do endereço (opcional). Pode ser nulo ou string.Empty. Se for fornecido (não null, empty, ou whitespace), pode ter validações de formato.
    /// </summary>
    public string? Complemento { get; init; }

    /// <summary>
    /// Bairro do endereço. Não pode ser nulo, vazio ou apenas whitespace.
    /// </summary>
    public string Bairro { get; init; }

    /// <summary>
    /// Cidade do endereço. Não pode ser nulo, vazio ou apenas whitespace.
    /// </summary>
    public string Cidade { get; init; }

    /// <summary>
    /// Estado (UF ou nome completo) do endereço. Não pode ser nulo, vazio ou apenas whitespace.
    /// </summary>
    public string Estado { get; init; }

    /// <summary>
    /// Value Object CEP do endereço. Sua criação é validada intrinsecamente.
    /// </summary>
    public CEP Cep { get; init; }

    /// <summary>
    /// Padrão Regex básico para caracteres permitidos em campos de endereço (letras com acentos, números, espaços, hífen e alguns símbolos comuns).
    /// Inclui: a-z, A-Z, caracteres acentuados, 0-9, espaços, -, ., ,, #, /.
    /// Este padrão é genérico e pode precisar ser ajustado para formatos de endereço específicos
    /// dos sistemas integrados (DocSales, Omie, Next) ou regras de negócio mais estritas.
    /// </summary>
    private const string EnderecoCaracteresRegex = @"^[a-zA-ZáàâãéèêíïóôõöúçñÁÀÂÃÉÈÊÍÏÓÔÕÖÚÇÑ\s\d.,#/-]+$";

    /// <summary>
    /// Construtor que valida e cria o Value Object Endereco.
    /// Valida campos obrigatórios, campos opcionais (se fornecidos) e o CEP agregado.
    /// Aplica Trim() nos campos string antes de validar.
    /// </summary>
    /// <param name="logradouro">O logradouro do endereço. Obrigatório.</param>
    /// <param name="bairro">O bairro do endereço. Obrigatório.</param>
    /// <param name="cidade">A cidade do endereço. Obrigatório.</param>
    /// <param name="estado">O estado do endereço. Obrigatório.</param>
    /// <param name="cepValor">O valor do CEP. Obrigatório.</param>
    /// <param name="numero">O número do endereço (opcional). Se fornecido, validado por Regex básica.</param>
    /// <param name="complemento">O complemento do endereço (opcional). Se fornecido, validado por Regex básica.</param>
    /// <exception cref="InvalidAddressException">Lançada se algum campo ou o CEP forem inválidos, encapsulando a exceção original.</exception>
    public Endereco(string logradouro, string bairro, string cidade, string estado, string cepValor, string? numero = null, string? complemento = null)
    {
        string cleanLogradouro = logradouro?.Trim() ?? string.Empty;
        string cleanBairro = bairro?.Trim() ?? string.Empty;
        string cleanCidade = cidade?.Trim() ?? string.Empty;
        string cleanEstado = estado?.Trim() ?? string.Empty;
        string? cleanNumero = numero?.Trim(); // Trim no opcional, mantém null se input for null
        string? cleanComplemento = complemento?.Trim(); // Trim no opcional, mantém null se input for null


        try
        {
            ValidateNotNullOrWhiteSpace(cleanLogradouro, nameof(logradouro));
            ValidateMinLength(cleanLogradouro, 2, nameof(logradouro));
            ValidateMaxLength(cleanLogradouro, 150, nameof(logradouro));
            ValidateByRegex(cleanLogradouro, EnderecoCaracteresRegex, nameof(logradouro));
            Logradouro = cleanLogradouro;

            ValidateNotNullOrWhiteSpace(cleanBairro, nameof(bairro));
            ValidateMinLength(cleanBairro, 2, nameof(bairro));
            ValidateMaxLength(cleanBairro, 40, nameof(bairro));
            ValidateByRegex(cleanBairro, EnderecoCaracteresRegex, nameof(bairro));
            Bairro = cleanBairro;

            ValidateNotNullOrWhiteSpace(cleanCidade, nameof(cidade));
            ValidateMinLength(cleanCidade, 2, nameof(cidade));
            ValidateMaxLength(cleanCidade, 25, nameof(cidade));
            ValidateByRegex(cleanCidade, EnderecoCaracteresRegex, nameof(cidade));
            Cidade = cleanCidade;

            ValidateNotNullOrWhiteSpace(cleanEstado, nameof(estado));
            ValidateMinLength(cleanEstado, 2, nameof(estado));
            ValidateMaxLength(cleanEstado, 25, nameof(estado)); // Considera nome completo ou UF
            ValidateByRegex(cleanEstado, EnderecoCaracteresRegex, nameof(estado));
            Estado = cleanEstado;

            if (!string.IsNullOrWhiteSpace(cleanNumero))
            {
                 ValidateByRegex(cleanNumero, EnderecoCaracteresRegex, nameof(numero));
            }
            Numero = cleanNumero; // Atribui o valor limpo (pode ser null)

             if (!string.IsNullOrWhiteSpace(cleanComplemento))
             {
                 ValidateByRegex(cleanComplemento, EnderecoCaracteresRegex, nameof(complemento));
             }
            Complemento = cleanComplemento; // Atribui o valor limpo (pode ser null)

            Cep = new CEP(cepValor);

        }
        catch (ArgumentException ex)
        {
             throw new InvalidAddressException("Endereco", "Múltiplos campos", ex.Message, ex);
        }
        catch (InvalidCepException ex)
        {
             throw new InvalidAddressException(nameof(Cep), cepValor ?? "null", $"CEP inválido na criação do endereço.", ex);
        }
    }

    /// <summary>
    /// Retorna uma representação formatada do endereço completo em uma única linha.
    /// Campos nulos, vazios ou apenas whitespace são omitidos.
    /// Nota: Este formato é uma representação básica. Para exibição em UI,
    /// pode ser necessário formatar de maneira diferente (ex: múltiplas linhas).
    /// </summary>
    /// <returns>O endereço formatado como string.</returns>
    public override string ToString()
    {
        return string.Join(", ", new[] { Logradouro, Numero, Complemento, Bairro, Cidade, Estado, Cep?.ToString() }
                                   .Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}