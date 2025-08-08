// src/SIGVoip.Domain/ValueObjects/ContatoTelefonico.cs
using SIGVoip.Domain.Exceptions;
using SIGVoip.Domain.Enums;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa um ponto de contato telefônico específico, incluindo o número (Telefone VO)
/// e metadados sobre seu uso, tecnologia e disponibilidade de WhatsApp. Value Object imutável.
/// </summary>
public sealed record ContatoTelefonico
{
    /// <summary>
    /// O Value Object Telefone que representa o número.
    /// </summary>
    public Telefone NumeroTelefone { get; init; }

    /// <summary>
    /// O uso do telefone (Comercial, Pessoal, etc.).
    /// </summary>
    public UsoTelefone Uso { get; init; }

    /// <summary>
    /// A tecnologia do telefone (Fixo, Celular, etc.).
    /// </summary>
    public TipoTelefone Tipo { get; init; }

    /// <summary>
    /// Indica se o número possui WhatsApp associado.
    /// </summary>
    public bool EhWhatsapp { get; init; }

    /// <summary>
    /// Descrição livre opcional para o contato telefônico.
    /// </summary>
    public string? Descricao { get; init; }

    /// <summary>
    /// Construtor que cria um Value Object ContatoTelefonico, validando a coerência dos dados.
    /// </summary>
    /// <param name="numeroTelefone">O Value Object Telefone.</param>
    /// <param name="uso">O uso do telefone (Comercial, Pessoal, etc.).</param>
    /// <param name="tipo">A tecnologia do telefone (Fixo, Celular, etc.).</param>
    /// <param name="ehWhatsapp">Indica se possui WhatsApp.</param>
    /// <param name="descricao">Descrição livre (opcional).</param>
    /// <exception cref="ArgumentNullException">Lançada se o Telefone fornecido for nulo.</exception>
    /// <exception cref="InvalidValueObjectException">Lançada se a combinação de tipologias for inconsistente.</exception>
    public ContatoTelefonico(Telefone numeroTelefone, UsoTelefone uso, TipoTelefone tipo, bool ehWhatsapp, string? descricao = null)
    {
        if (numeroTelefone is null)
        {
            throw new ArgumentNullException(nameof(numeroTelefone), "O Value Object Telefone não pode ser nulo.");
        }
        
        NumeroTelefone = numeroTelefone;
        Uso = uso;
        Tipo = tipo;
        EhWhatsapp = ehWhatsapp;
        
        /// <summary>
        /// Aplica as regras de validação de coerência para as propriedades do ContatoTelefonico.
        /// </summary>
        if (Uso == UsoTelefone.NaoEspecificado && Tipo == TipoTelefone.NaoEspecificado)
        {
            throw new InvalidContatoTelefonicoException("O contato telefônico deve ter um 'Uso' ou 'Tipo' especificado para ser válido.");
        }
        
        /// <param name="descricao">A string a ser normalizada.</param>
        /// <returns>A string trimada ou null se vazia/whitespace.</returns>
        Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim();
    }

    /// <summary>
    /// Retorna uma representação formatada do contato telefônico.
    /// </summary>
    /// <returns>A string formatada do contato telefônico.</returns>
    public override string ToString()
    {
        var parts = new List<string>
        {
            NumeroTelefone.ToString()
        };

        if (Uso != UsoTelefone.NaoEspecificado)
        {
            parts.Add($"Uso: {Uso}");
        }

        if (Tipo != TipoTelefone.NaoEspecificado)
        {
            parts.Add($"Tipo: {Tipo}");
        }

        if (EhWhatsapp)
        {
            parts.Add("WhatsApp");
        }

        if (!string.IsNullOrWhiteSpace(Descricao))
        {
            parts.Add($"({Descricao})");
        }

        return string.Join(" | ", parts);
    }
}