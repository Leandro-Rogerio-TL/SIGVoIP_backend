// src/SIGVoip.Domain/Enums/TecnologiaTelefone.cs
namespace SIGVoip.Domain.Enums
{
    /// <summary>
    /// Define a tecnologia subjacente de um número de telefone.
    /// </summary>
    public enum TipoTelefone
    {
        NaoEspecificado = 0, // Valor padrão ou quando a tecnologia não é conhecida/aplicável
        Fixo = 1,
        Celular = 2,
    }
}