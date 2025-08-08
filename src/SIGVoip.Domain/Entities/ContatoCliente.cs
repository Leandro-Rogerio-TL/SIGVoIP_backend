using SIGVoip.Domain.Exceptions;
using SIGVoip.Domain.ValueObjects;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

namespace SIGVoip.Domain.Entities;

/// <summary>
/// Representa um contato específico associado a um cliente (tipicamente Pessoa Jurídica).
/// Inclui nome, função, emails e telefones, e um status de atividade.
/// </summary>
public class ContatoCliente
{
    private readonly List<Email> _emails;
    private readonly List<ContatoTelefonico> _contatosTelefonicos;

    /// <summary>
    /// O identificador único do contato.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// O ID do cliente ao qual este contato está associado.
    /// </summary>
    public Guid ClienteSIGId { get; private set; }

    /// <summary>
    /// O nome completo da pessoa de contato.
    /// </summary>
    public NomePF NomeCompleto { get; private set; }

    /// <summary>
    /// A função ou papel da pessoa de contato na empresa (ex: "Gerente Financeiro", "Técnico").
    /// </summary>
    public string Funcao { get; private set; }

    /// <summary>
    /// Coleção de emails da pessoa de contato.
    /// </summary>
    public IReadOnlyCollection<Email> Emails => _emails.AsReadOnly();

    /// <summary>
    /// Coleção de contatos telefônicos da pessoa de contato.
    /// </summary>
    public IReadOnlyCollection<ContatoTelefonico> ContatosTelefonicos => _contatosTelefonicos.AsReadOnly();

    /// <summary>
    /// Indica se o contato está ativo.
    /// </summary>
    public bool Ativo { get; private set; }

    /// <summary>
    /// Construtor privado para uso do ORM/serialização.
    /// Não deve ser usado diretamente para criar novas instâncias.
    /// </summary>
    private ContatoCliente()
    {
        _emails = new List<Email>();
        _contatosTelefonicos = new List<ContatoTelefonico>();
    }

    /// <summary>
    /// Construtor para criar uma nova instância de ContatoCliente.
    /// Valida as informações essenciais para a criação do contato.
    /// </summary>
    /// <param name="clienteSIGId">O ID do cliente ao qual este contato pertence.</param>
    /// <param name="nomeCompleto">O nome da pessoa de contato.</param>
    /// <param name="funcao">A função ou papel da pessoa de contato.</param>
    /// <param name="emails">Uma lista inicial de emails para o contato.</param>
    /// <param name="contatosTelefonicos">Uma lista inicial de contatos telefônicos para o contato.</param>
    /// <param name="ativo">Indica se o contato está ativo (padrão: true).</param>
    /// <exception cref="InvalidContatoClienteException">Lançada se as regras de negócio para criação do contato forem violadas.</exception>
    public ContatoCliente(
        Guid clienteSIGId,
        NomePF nomeCompleto,
        string funcao,
        IEnumerable<Email> emails,
        IEnumerable<ContatoTelefonico> contatosTelefonicos,
        bool ativo = true)
    {
        _emails = new List<Email>();
        _contatosTelefonicos = new List<ContatoTelefonico>();

        try
        {
            if (clienteSIGId == Guid.Empty) throw new ArgumentException("O ID do ClienteSIG não pode ser vazio.", nameof(clienteSIGId));
            if (nomeCompleto is null) throw new ArgumentNullException(nameof(nomeCompleto), "O nome completo do contato não pode ser nulo.");

            ValidateNotNullOrWhiteSpace(funcao, nameof(funcao));
            ValidateMinLength(funcao, 2, nameof(funcao));
            ValidateMaxLength(funcao, 50, nameof(funcao));
            // Regex para caracteres comuns em nomes de função/cargo
            ValidateByRegex(funcao, @"^[A-Za-zÀ-ÖØ-öø-ÿ0-9\s.,&\-/\(\)'""]+$", nameof(funcao));
            ValidateDifferentCharacters(funcao, nameof(funcao));


            if (emails is null || !emails.Any())
            {
                throw new ArgumentException("Pelo menos um email deve ser fornecido para o contato.", nameof(emails));
            }
            if (contatosTelefonicos is null || !contatosTelefonicos.Any())
            {
                throw new ArgumentException("Pelo menos um contato telefônico deve ser fornecido para o contato.", nameof(contatosTelefonicos));
            }

            Id = Guid.NewGuid();
            ClienteSIGId = clienteSIGId;
            NomeCompleto = nomeCompleto;
            Funcao = funcao.Trim(); // Garante trim do valor após validação
            Ativo = ativo;

            foreach (var email in emails)
            {
                AdicionarEmail(email); // Usa o método para garantir validação de duplicidade
            }
            foreach (var telefone in contatosTelefonicos)
            {
                AdicionarContatoTelefonico(telefone); // Usa o método para garantir validação de duplicidade
            }
        }
        catch (ArgumentException ex)
        {
            throw new InvalidContatoClienteException(ex.ParamName ?? "ContatoCliente", "", ex.Message, ex);
        }
    }

    /// <summary>
    /// Atualiza as informações básicas do contato.
    /// </summary>
    /// <param name="novoNomeCompleto">O novo nome completo do contato.</param>
    /// <param name="novaFuncao">A nova função ou papel do contato.</param>
    /// <exception cref="InvalidContatoClienteException">Lançada se os novos valores forem inválidos.</exception>
    public void AtualizarContato(NomePF novoNomeCompleto, string novaFuncao)
    {
        try
        {
            if (novoNomeCompleto is null) throw new ArgumentNullException(nameof(novoNomeCompleto), "O nome completo do contato não pode ser nulo.");

            ValidateNotNullOrWhiteSpace(novaFuncao, nameof(novaFuncao));
            ValidateMinLength(novaFuncao, 2, nameof(novaFuncao));
            ValidateMaxLength(novaFuncao, 50, nameof(novaFuncao));
            ValidateByRegex(novaFuncao, @"^[A-Za-zÀ-ÖØ-öø-ÿ0-9\s.,&\-/\(\)'""]+$", nameof(novaFuncao));
            ValidateDifferentCharacters(novaFuncao, nameof(novaFuncao));

            NomeCompleto = novoNomeCompleto;
            Funcao = novaFuncao.Trim();
        }
        catch (ArgumentException ex)
        {
            throw new InvalidContatoClienteException(ex.ParamName ?? "ContatoCliente", "", ex.Message, ex);
        }
    }

    /// <summary>
    /// Adiciona um novo email à coleção de emails do contato.
    /// </summary>
    /// <param name="novoEmail">O email a ser adicionado.</param>
    /// <exception cref="InvalidContatoClienteException">Lançada se o email for nulo ou já existir.</exception>
    public void AdicionarEmail(Email novoEmail)
    {
        if (novoEmail is null)
        {
            throw new InvalidContatoClienteException(nameof(novoEmail), "null", "Email não pode ser nulo ao adicionar.");
        }
        if (_emails.Contains(novoEmail)) // Record equality handles content comparison
        {
            throw new InvalidContatoClienteException(nameof(novoEmail), novoEmail.ToString(), "Este email já existe para este contato.");
        }
        _emails.Add(novoEmail);
    }

    /// <summary>
    /// Remove um email da coleção de emails do contato.
    /// </summary>
    /// <param name="emailARemover">O email a ser removido.</param>
    /// <exception cref="InvalidContatoClienteException">Lançada se o email não for encontrado ou for nulo.</exception>
    public void RemoverEmail(Email emailARemover)
    {
        if (emailARemover is null)
        {
            throw new InvalidContatoClienteException(nameof(emailARemover), "null", "Email não pode ser nulo ao remover.");
        }
        if (!_emails.Remove(emailARemover))
        {
            throw new InvalidContatoClienteException(nameof(emailARemover), emailARemover.ToString(), "Email não encontrado para remoção.");
        }
    }

    /// <summary>
    /// Adiciona um novo contato telefônico à coleção de telefones do contato.
    /// </summary>
    /// <param name="novoContato">O contato telefônico a ser adicionado.</param>
    /// <exception cref="InvalidContatoClienteException">Lançada se o contato telefônico for nulo ou já existir.</exception>
    public void AdicionarContatoTelefonico(ContatoTelefonico novoContato)
    {
        if (novoContato is null)
        {
            throw new InvalidContatoClienteException(nameof(novoContato), "null", "Contato Telefônico não pode ser nulo ao adicionar.");
        }
        if (_contatosTelefonicos.Contains(novoContato)) // Record equality handles content comparison
        {
            throw new InvalidContatoClienteException(nameof(novoContato), novoContato.ToString(), "Este contato telefônico já existe para este contato.");
        }
        _contatosTelefonicos.Add(novoContato);
    }

    /// <summary>
    /// Remove um contato telefônico da coleção de telefones do contato.
    /// </summary>
    /// <param name="contatoARemover">O contato telefônico a ser removido.</param>
    /// <exception cref="InvalidContatoClienteException">Lançada se o contato telefônico não for encontrado ou for nulo.</exception>
    public void RemoverContatoTelefonico(ContatoTelefonico contatoARemover)
    {
        if (contatoARemover is null)
        {
            throw new InvalidContatoClienteException(nameof(contatoARemover), "null", "Contato Telefônico não pode ser nulo ao remover.");
        }
        if (!_contatosTelefonicos.Remove(contatoARemover))
        {
            throw new InvalidContatoClienteException(nameof(contatoARemover), contatoARemover.ToString(), "Contato Telefônico não encontrado para remoção.");
        }
    }

    /// <summary>
    /// Altera o status de atividade do contato.
    /// </summary>
    /// <param name="ativo">True para ativar, false para desativar.</param>
    public void AlterarStatus(bool ativo)
    {
        Ativo = ativo;
    }
}