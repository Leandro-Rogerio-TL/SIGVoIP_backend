using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

/// <summary>
/// Representa um Documento Fiscal validado (CPF ou CNPJ). Value Object imutável.
/// Garante formato, numericidade e dígitos verificadores válidos ao ser criado.
/// </summary>
namespace SIGVoip.Domain.ValueObjects;

public sealed record DocumentoFiscal
{
    /// <summary>
    /// O valor do documento fiscal (apenas dígitos).
    /// </summary>
    public string Valor { get; init; }

    /// <summary>
    /// Indica se o documento fiscal é um CPF.
    /// </summary>
    public bool EhCpf => Valor?.Length == 11;

    /// <summary>
    /// Indica se o documento fiscal é um CNPJ.
    /// </summary>
    public bool EhCnpj => Valor?.Length == 14;

    // Multiplicadores para cálculo dos dígitos verificadores de CPF
    private static readonly int[] _multiplicadoresCpf1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] _multiplicadoresCpf2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    // Multiplicadores para cálculo dos dígitos verificadores de CNPJ
    private static readonly int[] _multiplicadoresCnpj1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] _multiplicadoresCnpj2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    /// <summary>
    /// Construtor que valida o documento fiscal (CPF ou CNPJ).
    /// </summary>
    /// <param name="valor">O valor do documento fiscal (apenas dígitos ou formatado).</param>
    /// <exception cref="ArgumentException">Lançada por helpers se formato/numericidade básicos falharem.</exception>
    /// <exception cref="InvalidDocumentException">Lançada se o tamanho for inválido ou dígitos verificadores estiverem incorretos.</exception>
    public DocumentoFiscal(string valor)
    {
        string cleanValor = string.Empty;

        try
        {
            ValidateNotNullOrWhiteSpace(valor, nameof(valor));
            cleanValor = CleanDigits(valor);

            // Validações que usam helpers devem estar no try-catch de ArgumentException
            ValidateIsNumeric(cleanValor, nameof(valor));
            ValidateDifferentCharacters(cleanValor, nameof(valor));

            Valor = cleanValor; // Atribui o valor limpo APÓS validações básicas
        }
         catch (ArgumentException ex)
        {
            // Captura ArgumentException dos helpers e relança InvalidDocumentException.
            throw new InvalidDocumentException(nameof(valor), valor, $"Erro básico de formato/numericidade: {ex.Message}", ex);
        }

        // Validações específicas do domínio DocumentoFiscal (fora do try-catch de ArgumentException)
        ValidateLength(Valor); // Agora usa o Valor já limpo e atribuído

        if (!IsValidDigit())
        {
            // Esta exceção é lançada por lógica intrínseca do VO, não por um helper.
            throw new InvalidDocumentException("Dígitos verificadores do documento fiscal inválidos.");
        }
    }

    // Valida o tamanho do documento fiscal (11 ou 14) - Lógica específica do domínio
    private void ValidateLength(string valor)
    {
        if (valor.Length != 11 && valor.Length != 14)
        {
            throw new InvalidDocumentException("O documento fiscal deve ter 11 (CPF) ou 14 (CNPJ) dígitos.");
        }
    }

    // Validação de dígitos verificadores - Lógica específica do domínio
    private bool IsValidDigit()
    {
        if (EhCpf) return ValidateDigit(Valor, _multiplicadoresCpf1, _multiplicadoresCpf2);
        if (EhCnpj) return ValidateDigit(Valor, _multiplicadoresCnpj1, _multiplicadoresCnpj2);
        return false;
    }

    // Método interno para cálculo e validação genérica de DV
    private static bool ValidateDigit(string numero, int[] multiplicadores1, int[] multiplicadores2)
    {
        string tempNumero = numero.Substring(0, multiplicadores1.Length);
        int digito1 = CalculateSingleDigit(tempNumero, multiplicadores1);

        tempNumero += digito1;
        int digito2 = CalculateSingleDigit(tempNumero, multiplicadores2);

        return numero.EndsWith(digito1.ToString() + digito2.ToString());
    }

    // Método interno para cálculo de um único dígito verificador
    private static int CalculateSingleDigit(string numero, int[] multiplicadores)
    {
        int soma = 0;
        for (int i = 0; i < multiplicadores.Length; i++)
            soma += int.Parse(numero[i].ToString()) * multiplicadores[i];

        int resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    /// <summary>
    /// Retorna a representação formatada do documento fiscal (CPF ou CNPJ).
    /// </summary>
    /// <returns>O documento fiscal formatado.</returns>
    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Valor)) return string.Empty;

        if (EhCpf) return Convert.ToUInt64(Valor).ToString(@"000\.000\.000\-00");
        if (EhCnpj) return Convert.ToUInt64(Valor).ToString(@"00\.000\.000\/0000\-00");

        return Valor;
    }
}