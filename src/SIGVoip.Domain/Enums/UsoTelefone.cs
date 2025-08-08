namespace SIGVoip.Domain.Enums;

/// <summary>
/// Define o tipo de uso de um número de telefone.
/// </summary>
public enum UsoTelefone
{
    NaoEspecificado = 0, // Valor padrão ou quando o uso não é conhecido/aplicável
    Comercial = 1,
    Pessoal = 2,
}