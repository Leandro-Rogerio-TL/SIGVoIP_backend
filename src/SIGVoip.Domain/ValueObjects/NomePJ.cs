// src/SIGVoip.Domain/ValueObjects/NomePJ.cs
using System;
using System.Text.RegularExpressions;
using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa o nome de uma pessoa jurídica, composto por Razão Social (opcional) e Nome Fantasia (opcional).
/// Pelo menos um dos dois campos deve ser fornecido.
/// Value Object imutável. Garante a validade e o formato básico do nome.
/// </summary>
public sealed record NomePJ
{
    /// <summary>
    /// A Razão Social da empresa (opcional).
    /// </summary>
    public string? RazaoSocial { get; init; }

    /// <summary>
    /// O Nome Fantasia da empresa (opcional).
    /// </summary>
    public string? NomeFantasia { get; init; }

    /// <summary>
    /// Construtor que valida e cria o Value Object NomePJ.
    /// Lança <see cref="InvalidNomePessoaJuridicaException"/> se as partes do nome forem inválidas.
    /// </summary>
    /// <param name="razaoSocial">A Razão Social da empresa (opcional).</param>
    /// <param name="nomeFantasia">O Nome Fantasia da empresa (opcional).</param>
    public NomePJ(string? razaoSocial, string? nomeFantasia)
    {
        string? cleanRazaoSocial = (razaoSocial ?? string.Empty).Trim();
        string? cleanNomeFantasia = (nomeFantasia ?? string.Empty).Trim();

        try
        {
            // Regex para letras (básicas e acentuadas), números, espaços e símbolos comuns em nomes empresariais
            const string nomeEmpresarialRegexPattern = @"^[A-Za-zÀ-ÖØ-öø-ÿ0-9\s.,&\-/\(\)'""°ºª]+$";

            bool isRazaoSocialProvided = !string.IsNullOrWhiteSpace(cleanRazaoSocial);
            bool isNomeFantasiaProvided = !string.IsNullOrWhiteSpace(cleanNomeFantasia);

            if (!isRazaoSocialProvided && !isNomeFantasiaProvided)
            {
                throw new ArgumentException("Pelo menos a Razão Social ou o Nome Fantasia deve ser fornecido.");
            }

            if (isRazaoSocialProvided)
            {
                ValidateMinLength(cleanRazaoSocial!, 3, nameof(razaoSocial));
                ValidateByRegex(cleanRazaoSocial!, nomeEmpresarialRegexPattern, nameof(razaoSocial));
                ValidateDifferentCharacters(cleanRazaoSocial!, nameof(razaoSocial));
            }
            else
            {
                cleanRazaoSocial = null;
            }

            if (isNomeFantasiaProvided)
            {
                ValidateMinLength(cleanNomeFantasia!, 2, nameof(nomeFantasia));
                ValidateByRegex(cleanNomeFantasia!, nomeEmpresarialRegexPattern, nameof(nomeFantasia));
                ValidateDifferentCharacters(cleanNomeFantasia!, nameof(nomeFantasia));
            }
            else
            {
                cleanNomeFantasia = null;
            }

            RazaoSocial = cleanRazaoSocial;
            NomeFantasia = cleanNomeFantasia;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidNomePessoaJuridicaException(
                $"{nameof(NomePJ)}",
                $"Razão Social: '{razaoSocial ?? "null"}', Nome Fantasia: '{nomeFantasia ?? "null"}'",
                ex.Message,
                ex);
        }
    }

    /// <summary>
    /// Retorna o nome principal da pessoa jurídica (Razão Social se disponível, caso contrário Nome Fantasia).
    /// </summary>
    /// <returns>O nome principal formatado como string.</returns>
    public override string ToString()
    {
        return RazaoSocial ?? NomeFantasia ?? string.Empty;
    }
}