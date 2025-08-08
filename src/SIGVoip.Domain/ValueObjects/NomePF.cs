// src/SIGVoip.Domain/ValueObjects/NomePF.cs
using System;
using System.Text.RegularExpressions;
using SIGVoip.Domain.Exceptions;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

namespace SIGVoip.Domain.ValueObjects;

/// <summary>
/// Representa o nome de uma pessoa física, composto por Primeiro Nome, Nome(s) do Meio (opcional) e Sobrenome.
/// Value Object imutável. Garante a validade e o formato básico do nome.
/// </summary>
public sealed record NomePF
{
    /// <summary>
    /// O primeiro nome da pessoa.
    /// </summary>
    public string PrimeiroNome { get; init; }

    /// <summary>
    /// O(s) nome(s) do meio da pessoa (opcional).
    /// </summary>
    public string? NomeDoMeio { get; init; }

    /// <summary>
    /// O sobrenome da pessoa.
    /// </summary>
    public string Sobrenome { get; init; }

    /// <summary>
    /// Construtor que valida e cria o Value Object NomePF.
    /// Lança <see cref="InvalidNomePessoaFisicaException"/> se as partes do nome forem inválidas.
    /// </summary>
    /// <param name="primeiroNome">O primeiro nome da pessoa.</param>
    /// <param name="sobrenome">O sobrenome da pessoa.</param>
    /// <param name="nomeDoMeio">O(s) nome(s) do meio da pessoa (opcional).</param>
    public NomePF(string primeiroNome, string sobrenome, string? nomeDoMeio = null)
    {
        string cleanPrimeiroNome = (primeiroNome ?? string.Empty).Trim();
        string? cleanNomeDoMeio = (nomeDoMeio ?? string.Empty).Trim();
        string cleanSobrenome = (sobrenome ?? string.Empty).Trim();

        try
        {
            // Regex para letras (básicas e acentuadas), espaços, hífens e apóstrofos
            const string nomeRegexPattern = @"^[A-Za-zÀ-ÖØ-öø-ÿ\s'-]+$";

            ValidateNotNullOrWhiteSpace(cleanPrimeiroNome, nameof(primeiroNome));
            ValidateMinLength(cleanPrimeiroNome, 2, nameof(primeiroNome));
            ValidateByRegex(cleanPrimeiroNome, nomeRegexPattern, nameof(primeiroNome));
            ValidateDifferentCharacters(cleanPrimeiroNome, nameof(primeiroNome)); // Valida "aaaaa"

            ValidateNotNullOrWhiteSpace(cleanSobrenome, nameof(sobrenome));
            ValidateMinLength(cleanSobrenome, 2, nameof(sobrenome));
            ValidateByRegex(cleanSobrenome, nomeRegexPattern, nameof(sobrenome));
            ValidateDifferentCharacters(cleanSobrenome, nameof(sobrenome)); // Valida "aaaaa"

            if (!string.IsNullOrWhiteSpace(cleanNomeDoMeio))
            {
                ValidateByRegex(cleanNomeDoMeio, nomeRegexPattern, nameof(nomeDoMeio));
                ValidateDifferentCharacters(cleanNomeDoMeio, nameof(nomeDoMeio)); // Valida "aaaaa"
            }
            else
            {
                cleanNomeDoMeio = null; // Garante que seja null se for vazio/whitespace
            }

            PrimeiroNome = cleanPrimeiroNome;
            NomeDoMeio = cleanNomeDoMeio;
            Sobrenome = cleanSobrenome;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidNomePessoaFisicaException(
                $"{nameof(NomePF)}",
                $"{primeiroNome ?? "null"} {nomeDoMeio ?? "null"} {sobrenome ?? "null"}",
                ex.Message,
                ex);
        }
    }

    /// <summary>
    /// Retorna o nome completo formatado da pessoa (ex: "João da Silva", "Maria Alice da Costa").
    /// </summary>
    /// <returns>O nome completo formatado como string.</returns>
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(NomeDoMeio)
            ? $"{PrimeiroNome} {Sobrenome}"
            : $"{PrimeiroNome} {NomeDoMeio} {Sobrenome}";
    }
}