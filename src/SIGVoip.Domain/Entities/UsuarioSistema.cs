using SIGVoip.Domain.Exceptions;
using SIGVoip.Domain.ValueObjects;
using static SIGVoip.Domain.Helpers.DomainValidationHelpers;

namespace SIGVoip.Domain.Entities;

/// <summary>
/// Representa um usuário do sistema SIG para autenticação e autorização.
/// </summary>
public class UsuarioSistema
{
    /// <summary>
    /// O identificador único do usuário.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// O login (username) do usuário.
    /// </summary>
    public string Login { get; private set; }

    /// <summary>
    /// Data de criação do usuário (UTC).
    /// </summary>
    public DateTime DataCriacao { get; private set; }

    /// <summary>
    /// Status de atividade do usuário.
    /// </summary>
    public bool Ativo { get; private set; }

    /// <summary>
    /// Email para recuperação de senha ou notificações.
    /// </summary>
    public Email EmailRecuperacao { get; private set; }

    /// <summary>
    /// ID do ClienteSIG ao qual este usuário está associado.
    /// </summary>
    public Guid ClienteSIGId { get; private set; }

    private UsuarioSistema() { }

    /// <summary>
    /// Construtor para criar uma nova instância de UsuarioSistema.
    /// </summary>
    /// <param name="clienteSIGId">ID do ClienteSIG associado.</param>
    /// <param name="login">Login do usuário.</param>
    /// <param name="emailRecuperacao">Email para recuperação de senha.</param>
    /// <param name="ativo">Indica se o usuário está ativo (padrão: true).</param>
    /// <exception cref="InvalidUsuarioSistemaException">Lançada se as validações forem violadas.</exception>
    public UsuarioSistema(Guid clienteSIGId, string login, Email emailRecuperacao, bool ativo = true)
    {
        try
        {
            if (clienteSIGId == Guid.Empty)
            {
                throw new ArgumentException("O ID do ClienteSIG não pode ser vazio.", nameof(clienteSIGId));
            }
            if (emailRecuperacao is null)
            {
                throw new ArgumentNullException(nameof(emailRecuperacao), "O email de recuperação não pode ser nulo.");
            }

            ValidateNotNullOrWhiteSpace(login, nameof(login));
            ValidateMinLength(login, 5, nameof(login));
            ValidateMaxLength(login, 50, nameof(login));
            ValidateByRegex(login, @"^[a-zA-Z0-9._\-]+$", nameof(login));

            Id = Guid.NewGuid();
            ClienteSIGId = clienteSIGId;
            Login = login.Trim();
            EmailRecuperacao = emailRecuperacao;
            Ativo = ativo;
            DataCriacao = DateTime.UtcNow;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidUsuarioSistemaException(ex.ParamName ?? "UsuarioSistema", login, ex.Message, ex);
        }
    }

    /// <summary>
    /// Altera o status de atividade do usuário.
    /// </summary>
    /// <param name="ativo">True para ativar, false para desativar.</param>
    public void AlterarStatus(bool ativo)
    {
        Ativo = ativo;
    }

    /// <summary>
    /// Atualiza o email de recuperação do usuário.
    /// </summary>
    /// <param name="novoEmailRecuperacao">O novo email de recuperação.</param>
    /// <exception cref="InvalidUsuarioSistemaException">Lançada se o email for nulo.</exception>
    public void AtualizarEmailRecuperacao(Email novoEmailRecuperacao)
    {
        if (novoEmailRecuperacao is null)
        {
            throw new InvalidUsuarioSistemaException(nameof(novoEmailRecuperacao), "null", "Email de recuperação não pode ser nulo ao atualizar.");
        }
        EmailRecuperacao = novoEmailRecuperacao;
    }

    /// <summary>
    /// Atualiza o login do usuário.
    /// </summary>
    /// <param name="novoLogin">O novo login.</param>
    /// <exception cref="InvalidUsuarioSistemaException">Lançada se o login for nulo ou inválido.</exception>
    public void AtualizarLogin(string novoLogin)
    {
        try
        {
            ValidateNotNullOrWhiteSpace(novoLogin, nameof(novoLogin));
            ValidateMinLength(novoLogin, 5, nameof(novoLogin));
            ValidateMaxLength(novoLogin, 50, nameof(novoLogin));
            ValidateByRegex(novoLogin, @"^[a-zA-Z0-9._\-]+$", nameof(novoLogin));
            Login = novoLogin.Trim();
        }
        catch (ArgumentException ex)
        {
            throw new InvalidUsuarioSistemaException(ex.ParamName ?? "Login", novoLogin, ex.Message, ex);
        }
    }
}