using SIGVoip.Domain.Exceptions;
using SIGVoip.Domain.ValueObjects;
namespace SIGVoip.Domain.Entities;

/// <summary>
/// Representa o cliente central do sistema SIG VoIP, consolidando informações e integrações.
/// </summary>
public class ClienteSIG
{
    private readonly List<ContatoCliente> _contatosPessoais;
    // private readonly List<ServicoContratado> _servicosContratados; // Será adicionado quando ServicoContratado for definido

    /// <summary>
    /// O identificador único do ClienteSIG.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Documento fiscal (CPF ou CNPJ) do cliente.
    /// </summary>
    public DocumentoFiscal DocumentoFiscal { get; private set; }

    /// <summary>
    /// Nome da Pessoa Física (opcional, obrigatório se for PF).
    /// </summary>
    public NomePF? NomePessoaFisica { get; private set; }

    /// <summary>
    /// Razão Social da Pessoa Jurídica (opcional, obrigatório se for PJ).
    /// </summary>
    public NomePJ? NomePessoaJuridica { get; private set; }

    /// <summary>
    /// Nome de exibição do cliente, computado a partir do NomePF ou NomePJ.
    /// </summary>
    public string NomeExibicao => NomePessoaFisica?.ToString() ?? NomePessoaJuridica?.ToString() ?? string.Empty;

    /// <summary>
    /// Endereço principal do cliente.
    /// </summary>
    public Endereco Endereco { get; private set; }

    /// <summary>
    /// Email de contato principal/genérico do cliente.
    /// </summary>
    public Email? Email { get; private set; }

    /// <summary>
    /// Telefone de contato principal/genérico do cliente.
    /// </summary>
    public ContatoTelefonico? ContatoTelefonico { get; private set; }

    /// <summary>
    /// Coleção de contatos internos do cliente (para PJ) ou contatos adicionais (para PF).
    /// </summary>
    public IReadOnlyCollection<ContatoCliente> ContatosPessoais => _contatosPessoais.AsReadOnly();

    /// <summary>
    /// O usuário do sistema SIG associado a este cliente.
    /// </summary>
    public UsuarioSistema UsuarioSistema { get; private set; }

    /// <summary>
    /// ID de referência do cliente no sistema DocSales.
    /// </summary>
    public long? IdDocSales { get; private set; }

    /// <summary>
    /// ID de referência do cliente no sistema Omie.
    /// </summary>
    public long? IdOmie { get; private set; }

    /// <summary>
    /// ID de referência do cliente no sistema Next (ID geral, se aplicável).
    /// </summary>
    public string? IdNext { get; private set; }

    /// <summary>
    /// IP do servidor Next onde o cliente principal está alocado (se aplicável).
    /// </summary>
    public string? IpServidorNext { get; private set; }

    /// <summary>
    /// Data e hora da criação do cliente (UTC).
    /// </summary>
    public DateTime DataCriacao { get; private set; }

    /// <summary>
    /// Data e hora da última atualização do cliente (UTC).
    /// </summary>
    public DateTime DataAtualizacao { get; private set; }

    /// <summary>
    /// Construtor privado para uso do ORM/serialização.
    /// Não deve ser usado diretamente para criar novas instâncias.
    /// </summary>
    private ClienteSIG()
    {
        _contatosPessoais = new List<ContatoCliente>();
        // _servicosContratados = new List<ServicoContratado>();
    }

    /// <summary>
    /// Construtor para criar uma nova instância de ClienteSIG (Pessoa Física).
    /// Valida as informações essenciais para a criação do cliente PF.
    /// </summary>
    /// <param name="documentoFiscal">CPF do cliente.</param>
    /// <param name="nomePessoaFisica">Nome da pessoa física.</param>
    /// <param name="endereco">Endereço principal.</param>
    /// <param name="usuarioSistema">Usuário do sistema SIG associado.</param>
    /// <param name="emailPrincipal">Email de contato principal (opcional).</param>
    /// <param name="contatoTelefonicoPrincipal">Telefone de contato principal (opcional).</param>
    /// <param name="contatosPessoais">Lista opcional de contatos adicionais para PF.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se as validações forem violadas.</exception>
    public ClienteSIG(
        DocumentoFiscal documentoFiscal,
        NomePF nomePessoaFisica,
        Endereco endereco,
        UsuarioSistema usuarioSistema,
        Email? emailPrincipal = null,
        ContatoTelefonico? contatoTelefonicoPrincipal = null,
        IEnumerable<ContatoCliente>? contatosPessoais = null) : this() // Chama o construtor privado para inicializar listas
    {
        try
        {
            if (documentoFiscal is null || !documentoFiscal.EhCpf)
            {
                throw new ArgumentException("Documento fiscal inválido ou não é um CPF.", nameof(documentoFiscal));
            }
            if (nomePessoaFisica is null)
            {
                throw new ArgumentNullException(nameof(nomePessoaFisica), "O nome da pessoa física não pode ser nulo.");
            }
            if (endereco is null)
            {
                throw new ArgumentNullException(nameof(endereco), "O endereço não pode ser nulo.");
            }
            if (usuarioSistema is null)
            {
                throw new ArgumentNullException(nameof(usuarioSistema), "O usuário do sistema não pode ser nulo.");
            }

            Id = Guid.NewGuid();
            DocumentoFiscal = documentoFiscal;
            NomePessoaFisica = nomePessoaFisica;
            Endereco = endereco;
            Email = emailPrincipal;
            ContatoTelefonico = contatoTelefonicoPrincipal;
            UsuarioSistema = usuarioSistema;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;

            if (contatosPessoais != null)
            {
                foreach (var contato in contatosPessoais)
                {
                    AdicionarContatoPessoal(contato); // Usa o método para validação de duplicidade, se implementado
                }
            }
        }
        catch (ArgumentException ex)
        {
            throw new InvalidClienteSIGException(ex.ParamName ?? "ClienteSIG", documentoFiscal?.ToString() ?? "null", ex.Message, ex);
        }
    }

    /// <summary>
    /// Construtor para criar uma nova instância de ClienteSIG (Pessoa Jurídica).
    /// Valida as informações essenciais para a criação do cliente PJ.
    /// </summary>
    /// <param name="documentoFiscal">CNPJ do cliente.</param>
    /// <param name="nomePessoaJuridica">Razão Social da pessoa jurídica.</param>
    /// <param name="endereco">Endereço principal.</param>
    /// <param name="usuarioSistema">Usuário do sistema SIG associado.</param>
    /// <param name="contatosPessoais">Lista obrigatória de contatos internos da empresa (mínimo 1).</param>
    /// <param name="emailPrincipal">Email de contato principal (opcional).</param>
    /// <param name="contatoTelefonicoPrincipal">Telefone de contato principal (opcional).</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se as validações forem violadas.</exception>
    public ClienteSIG(
        DocumentoFiscal documentoFiscal,
        NomePJ nomePessoaJuridica,
        Endereco endereco,
        UsuarioSistema usuarioSistema,
        IEnumerable<ContatoCliente> contatosPessoais,
        Email? emailPrincipal = null,
        ContatoTelefonico? contatoTelefonicoPrincipal = null) : this() // Chama o construtor privado para inicializar listas
    {
        try
        {
            if (documentoFiscal is null || !documentoFiscal.EhCnpj)
            {
                throw new ArgumentException("Documento fiscal inválido ou não é um CNPJ.", nameof(documentoFiscal));
            }
            if (nomePessoaJuridica is null)
            {
                throw new ArgumentNullException(nameof(nomePessoaJuridica), "O nome da pessoa jurídica não pode ser nulo.");
            }
            if (endereco is null)
            {
                throw new ArgumentNullException(nameof(endereco), "O endereço não pode ser nulo.");
            }
            if (usuarioSistema is null)
            {
                throw new ArgumentNullException(nameof(usuarioSistema), "O usuário do sistema não pode ser nulo.");
            }
            if (contatosPessoais is null || !contatosPessoais.Any())
            {
                throw new ArgumentException("Pessoa Jurídica deve ter pelo menos um contato pessoal.", nameof(contatosPessoais));
            }

            Id = Guid.NewGuid();
            DocumentoFiscal = documentoFiscal;
            NomePessoaJuridica = nomePessoaJuridica;
            Endereco = endereco;
            Email = emailPrincipal;
            ContatoTelefonico = contatoTelefonicoPrincipal;
            UsuarioSistema = usuarioSistema;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;

            foreach (var contato in contatosPessoais)
            {
                AdicionarContatoPessoal(contato); // Usa o método para validação de duplicidade, se implementado
            }
        }
        catch (ArgumentException ex)
        {
            throw new InvalidClienteSIGException(ex.ParamName ?? "ClienteSIG", documentoFiscal?.ToString() ?? "null", ex.Message, ex);
        }
    }

    /// <summary>
    /// Atualiza o endereço principal do cliente.
    /// </summary>
    /// <param name="novoEndereco">O novo endereço.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se o novo endereço for nulo.</exception>
    public void AtualizarEndereco(Endereco novoEndereco)
    {
        if (novoEndereco is null)
        {
            throw new InvalidClienteSIGException(nameof(novoEndereco), "null", "Endereço não pode ser nulo ao atualizar.");
        }
        Endereco = novoEndereco;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza os contatos principais (email e telefone) do cliente.
    /// </summary>
    /// <param name="novoEmailPrincipal">O novo email principal (pode ser nulo).</param>
    /// <param name="novoContatoTelefonicoPrincipal">O novo telefone principal (pode ser nulo).</param>
    public void AtualizarContatosGerais(Email? novoEmailPrincipal, ContatoTelefonico? novoContatoTelefonicoPrincipal)
    {
        Email = novoEmailPrincipal;
        ContatoTelefonico = novoContatoTelefonicoPrincipal;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Adiciona um novo contato pessoal à coleção de contatos do cliente.
    /// </summary>
    /// <param name="novoContato">O contato a ser adicionado.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se o contato for nulo ou já existir.</exception>
    public void AdicionarContatoPessoal(ContatoCliente novoContato)
    {
        if (novoContato is null)
        {
            throw new InvalidClienteSIGException(nameof(novoContato), "null", "Contato a ser adicionado não pode ser nulo.");
        }
        if (_contatosPessoais.Any(c => c.Id == novoContato.Id)) // Validação de duplicidade por ID
        {
            throw new InvalidClienteSIGException(nameof(novoContato), novoContato.Id.ToString(), "Contato já existe na lista.");
        }
        _contatosPessoais.Add(novoContato);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove um contato pessoal da coleção pelo seu ID.
    /// </summary>
    /// <param name="contatoId">O ID do contato a ser removido.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se o contato não for encontrado.</exception>
    public void RemoverContatoPessoal(Guid contatoId)
    {
        var contato = _contatosPessoais.FirstOrDefault(c => c.Id == contatoId);
        if (contato is null)
        {
            throw new InvalidClienteSIGException(nameof(contatoId), contatoId.ToString(), "Contato não encontrado para remoção.");
        }
        _contatosPessoais.Remove(contato);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza um contato pessoal existente (nome e função).
    /// </summary>
    /// <param name="contatoId">O ID do contato a ser atualizado.</param>
    /// <param name="novoNome">O novo nome completo da pessoa.</param>
    /// <param name="novaFuncao">A nova função da pessoa no contato.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se o contato não for encontrado ou os dados forem inválidos.</exception>
    public void AtualizarDetalhesContatoPessoal(Guid contatoId, NomePF novoNome, string novaFuncao)
    {
        var contato = _contatosPessoais.FirstOrDefault(c => c.Id == contatoId);
        if (contato is null)
        {
            throw new InvalidClienteSIGException(nameof(contatoId), contatoId.ToString(), "Contato não encontrado para atualização de detalhes.");
        }
        // Delega a validação e atualização para o próprio ContatoCliente
        contato.AtualizarContato(novoNome, novaFuncao);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Gerencia a lista de emails de um contato pessoal específico.
    /// </summary>
    /// <param name="contatoId">O ID do contato.</param>
    /// <param name="novosEmails">A nova coleção de emails. A lista interna do contato será substituída ou atualizada para refletir esta coleção.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se o contato não for encontrado.</exception>
    public void GerenciarEmailsContatoPessoal(Guid contatoId, IEnumerable<Email> novosEmails)
    {
        var contato = _contatosPessoais.FirstOrDefault(c => c.Id == contatoId);
        if (contato is null)
        {
            throw new InvalidClienteSIGException(nameof(contatoId), contatoId.ToString(), "Contato não encontrado para gerenciar emails.");
        }
        // Delega a lógica de gerenciamento de emails para o ContatoCliente
        // É importante que o ContatoCliente tenha um método para substituir/gerenciar a lista.
        // O exemplo de ContatoCliente possui Adicionar/Remover, assumindo que Gerenciar significa um processo de substituição.
        // Se a lógica for de adicionar/remover individualmente, os métodos seriam diferentes.
        // Por simplicidade, assumindo que o ContatoCliente pode ter um método como 'AtualizarEmails(IEnumerable<Email>)'
        // contato.AtualizarEmails(novosEmails); // Este método precisaria existir em ContatoCliente
        // Usando Adicionar/Remover como no exemplo de ContatoCliente:
        var emailsAtuais = contato.Emails.ToList(); // Crie uma cópia para modificação
        foreach (var emailExistente in emailsAtuais.Except(novosEmails))
        {
            contato.RemoverEmail(emailExistente);
        }
        foreach (var novoEmail in novosEmails.Except(emailsAtuais))
        {
            contato.AdicionarEmail(novoEmail);
        }
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Gerencia a lista de contatos telefônicos de um contato pessoal específico.
    /// </summary>
    /// <param name="contatoId">O ID do contato.</param>
    /// <param name="novosTelefones">A nova coleção de telefones. A lista interna do contato será substituída ou atualizada para refletir esta coleção.</param>
    /// <exception cref="InvalidClienteSIGException">Lançada se o contato não for encontrado.</exception>
    public void GerenciarContatosTelefonicosContatoPessoal(Guid contatoId, IEnumerable<ContatoTelefonico> novosTelefones)
    {
        var contato = _contatosPessoais.FirstOrDefault(c => c.Id == contatoId);
        if (contato is null)
        {
            throw new InvalidClienteSIGException(nameof(contatoId), contatoId.ToString(), "Contato não encontrado para gerenciar telefones.");
        }
        // Delega a lógica de gerenciamento de telefones para o ContatoCliente
        // contato.AtualizarContatosTelefonicos(novosTelefones); // Este método precisaria existir em ContatoCliente
        // Usando Adicionar/Remover como no exemplo de ContatoCliente:
        var telefonesAtuais = contato.ContatosTelefonicos.ToList(); // Crie uma cópia para modificação
        foreach (var telefoneExistente in telefonesAtuais.Except(novosTelefones))
        {
            contato.RemoverContatoTelefonico(telefoneExistente);
        }
        foreach (var novoTelefone in novosTelefones.Except(telefonesAtuais))
        {
            contato.AdicionarContatoTelefonico(novoTelefone);
        }
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o login do usuário do sistema associado.
    /// </summary>
    /// <param name="novoLogin">O novo login.</param>
    public void AtualizarLoginUsuario(string novoLogin)
    {
        // Delega a responsabilidade para a entidade UsuarioSistema
        UsuarioSistema.AtualizarLogin(novoLogin);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o email de recuperação do usuário do sistema associado.
    /// </summary>
    /// <param name="novoEmail">O novo email de recuperação.</param>
    public void AtualizarEmailRecuperacaoUsuario(Email novoEmail)
    {
        // Delega a responsabilidade para a entidade UsuarioSistema
        UsuarioSistema.AtualizarEmailRecuperacao(novoEmail);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Altera o status (ativo/inativo) do usuário do sistema associado.
    /// </summary>
    /// <param name="ativo">True para ativo, false para inativo.</param>
    public void AlterarStatusUsuario(bool ativo)
    {
        // Delega a responsabilidade para a entidade UsuarioSistema
        UsuarioSistema.AlterarStatus(ativo);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o ID de referência do DocSales.
    /// </summary>
    /// <param name="idDocSales">O novo ID do DocSales.</param>
    public void AtualizarIdDocSales(long? idDocSales)
    {
        IdDocSales = idDocSales;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o ID de referência do Omie.
    /// </summary>
    /// <param name="idOmie">O novo ID do Omie.</param>
    public void AtualizarIdOmie(long? idOmie)
    {
        IdOmie = idOmie;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o ID de referência do Next.
    /// </summary>
    /// <param name="idNext">O novo ID do Next.</param>
    public void AtualizarIdNext(string? idNext)
    {
        IdNext = idNext?.Trim();
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o IP do servidor Next onde o cliente está alocado.
    /// </summary>
    /// <param name="ipServidorNext">O novo IP do servidor Next.</param>
    public void AtualizarIpServidorNext(string? ipServidorNext)
    {
        IpServidorNext = ipServidorNext?.Trim();
        DataAtualizacao = DateTime.UtcNow;
    }

    // Métodos para ServicoContratado (a serem adicionados quando a entidade ServicoContratado for definida)
    // public void AdicionarServicoContratado(ServicoContratado novoServico) { /* ... */ }
    // public void RemoverServicoContratado(Guid idServico) { /* ... */ }
}