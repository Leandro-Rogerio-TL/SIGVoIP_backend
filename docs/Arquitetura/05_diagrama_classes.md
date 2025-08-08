# 🧠 5. Diagrama de Classes (UML) - Modelagem do Domínio SIGVoIP

## Objetivo
Este documento define a estrutura e as relações dos principais objetos, entidades do domínio e Value Objects (VOs) do sistema SIGVoIP, fornecendo a base conceitual para a implementação do código. O foco está na modelagem do domínio central, independente de detalhes de integração com sistemas externos.

---

## 1. Modelagem do Domínio: Entidades e Value Objects do SIGVoIP

### 1.1. A Entidade Central: `ClienteSIG` (Aggregate Root)

A entidade `ClienteSIG` é o **coração** do sistema SIGVoIP, atuando como a **Aggregate Root** para todas as informações de cliente. Seu papel principal é representar de forma centralizada e unificada um cliente (Pessoa Física ou Jurídica) e seus dados de integração com as diversas plataformas.

#### Importância:
* **Fonte Única de Verdade:** Garante a consistência dos dados do cliente em todo o sistema.
* **Centralização de IDs:** Armazena os IDs de referência do cliente em cada sistema externo (ID Omie, ID Next, ID DocSales), facilitando a identificação e sincronização.
* **Gerenciamento Unificado:** Permite a gestão do ciclo de vida do cliente e seu acesso ao SistemaSIG a partir de um único ponto.
* **Encapsulamento de Peculiaridades:** Adapta-se às diferentes formas de representação de um mesmo cliente nos sistemas externos.

#### Propriedades Essenciais:
* `Id`: GUID (PK)
* `DocumentoFiscal`: Value Object (`Documento`)
* `NomePessoaFisica`: Value Object (`NomePF?`) - Opcional, obrigatório se `DocumentoFiscal.EhCPF`.
* `NomePessoaJuridica`: Value Object (`NomePJ?`) - Opcional, obrigatório se `DocumentoFiscal.EhCNPJ`.
* `NomeExibicao`: string (Propriedade computada, derivada de `NomePF.ToString()` ou `NomePJ.ToString()`). Usada para identificação amigável em interfaces, não para unicidade.
* `Endereco`: Value Object (`Endereco`)
* `Email`: Value Object (`Email?`)
* `ContatoTelefonico`: Value Object (`ContatoTelefonico?`)
* `DataCriacao`: DateTime
* `DataAtualizacao`: DateTime
* `CustomerDocSalesID`: BIGINT (ID do cliente no DocSales)
* `ClienteOmieID`: BIGINT (ID do cliente no Omie)

#### Relacionamentos:
* `ClienteSIG 1 -- 1 UsuarioSIG`: Um cliente pode ter um usuário associado para acesso ao sistema.
* `ClienteSIG 1 -- * Contato`: Um cliente (especialmente PJ) pode ter múltiplos contatos.
* `ClienteSIG 1 -- * AssinanteNext`: Um cliente pode ter múltiplos assinantes/serviços no ecossistema Next.
* `ClienteSIG 1 -- * ProcessamentoProposta`: Um cliente pode ter múltiplas propostas DocSales processadas.

#### Comportamentos e Validações:
* **Construtores:**
    * `CriarClientePF(Documento cpf, NomePF nomePF, ...)`: Validações específicas para PF.
    * `CriarClientePJ(Documento cnpj, NomePJ nomePJ, List<Contato> contatos, ...)`: Validações específicas para PJ, incluindo a obrigatoriedade de ao menos um `Contato`.
* **Métodos de Gerenciamento:**
    * `AtualizarDadosPrincipais(NomePF/NomePJ, Endereco, Email principal, ContatoTelefonico principal)`
    * `AlterarStatus(bool ativo)`
    * `AdicionarContato(Contato novoContato)`
    * `RemoverContato(Guid idContato)`
    * `AtualizarDetalhesContato(Guid idContato, NomePF novoNome, string novaFuncao)`
    * `GerenciarEmailsContato(Guid idContato, IEnumerable<Email> emails)`
    * `GerenciarContatosTelefonicosContato(Guid idContato, IEnumerable<ContatoTelefonico> telefones)`
    * `AtualizarLoginUsuario(string novoLogin)`
    * `AtualizarEmailRecuperacaoUsuario(Email novoEmail)`
    * `AlterarStatusUsuario(bool ativo)`
    * `AdicionarAssinanteNext(AssinanteNext novoAssinante)`
    * `RemoverAssinanteNext(Guid idAssinante)`
    * `AtualizarAssinanteNext(Guid idAssinante, ...)`
* **Regras e Validações Gerais:**
    * **Unicidade do `ClienteSIG`:** Único por `DocumentoFiscal` (CPF/CNPJ).
    * **Lançamento de Exceções:** Violações de regras de negócio lançam `InvalidClienteSIGException`.

### 1.2. Outras Entidades de Domínio

* **`ProcessamentoProposta` (Aggregate Root)**
    * **Descrição:** Representa um documento/proposta aprovada vinda do DocSales e o status de seu processamento no SIGVoIP.
    * **Atributos:** `Id`, `ProposalDocSalesID`, `ProposalDocSalesCode`, `ClienteSigID` (FK), `DataRegistro`, `StatusGeral`, `StatusClienteSIG`, `MensagemErroClienteSIG`, `StatusAssinanteNext`, `AssinanteNextID` (FK, Nullable), `MensagemErroAssinanteNext`, `StatusClienteOmie`, `MensagemErroClienteOmie`, `StatusContaReceberOmie`, `MensagemErroContaReceberOmie`, `DataUltimaTentativa`, `NumeroTentativas`, `DadosOriginaisPropostaDocSalesJson`.
    * **Relacionamentos:** `ProcessamentoProposta 1 -- 1 ClienteSIG`.

* **`UsuarioSIG` (Entidade de Domínio)**
    * **Descrição:** Representa um usuário do sistema SIGVoIP sob a perspectiva do domínio de negócio, com credenciais de acesso e perfis para o painel administrativo.
    * **Atributos:** `Id`, `UserName`, `PasswordHash`, `Email` (VO), `Ativo`, `Perfis` (ENUM `PerfilUsuario`), `ClienteSigID` (FK, Nullable), `FuncionarioID` (FK, Nullable), `DataCriacao`.
    * **Relacionamentos:** `UsuarioSIG 1 -- 1 ClienteSIG` (se for um usuário de cliente), `UsuarioSIG 1 -- 1 Funcionario` (se for um usuário interno).
    * **Observação:** Esta entidade de domínio é mapeada para a entidade de infraestrutura e persistência `UsuarioSigBd` (detalhada em `06_modelo_dados.md`).

* **`ServidorNext` (Aggregate Root)**
    * **Descrição:** Gerencia as informações de cada servidor Next (NextBilling/NextRouter) ao qual o SIGVoIP se conecta, incluindo capacidade e status.
    * **Atributos:** `Id`, `Nome` (Unique), `TipoServidor` (Enum), `UrlApi`, `ChaveApi`, `IpPrincipal`, `IpsSecundarios` (JSON), `CapacidadeTotalClientes`, `CapacidadeTotalDIDs`, `ClientesAtuais`, `DIDsAtuais`, `Ativo`, `DataUltimaAtualizacaoDados`.
    * **Relacionamentos:** `ServidorNext 1 -- * AssinanteNext`.

* **`AssinanteNext` (Entidade de Domínio)**
    * **Descrição:** Gerencia as informações de um assinante em um servidor Next. Substitui o conceito de `ServicoContratado` para as integrações Next, refletindo a peculiaridade de múltiplos cadastros por documento no Next, diferenciados por sufixo e IP de servidor.
    * **Atributos:** `Id`, `AssinanteID` (ID no Next), `ServidorIPs` (Lista de IPs), `SufixoAssinante`, `NomeClienteNoNext`, `IpServidorNext` (para clareza), *Outras propriedades que detalham o serviço (tipo, plano, ramais, URAs, DIDs, etc.)*.
    * **Relacionamentos:** `AssinanteNext * -- 1 ClienteSIG`, `AssinanteNext * -- 1 ServidorNext`.

* **`Contato` (Entidade de Domínio)**
    * **Descrição:** Gerencia as informações de contatos de clientes e funcionários.
    * **Atributos:** `Id`, `NomePF` (VO), `Departamento`, `Cargo`, `Observacao`.
    * **Relacionamentos:** `Contato * -- 1 ClienteSIG`, `Contato * -- 1 Funcionario`, `Contato 1 -- * ContatoTelefonico` (VO), `Contato 1 -- * Email` (VO).

* **`Funcionario` (Entidade de Domínio)**
    * **Descrição:** Gerencia as informações dos funcionários internos do SIGVoIP.
    * **Atributos:** `Id`, `NomePF` (VO), `Documento` (VO), `Endereco` (VO), `Email` (VO), `Telefone` (VO - `ContatoTelefonico` para consistência), `DataCriacao`, `DataAtualizacao`, `Cargo`, `Ativo`.
    * **Relacionamentos:** `Funcionario 1 -- 1 UsuarioSIG` (opcional), `Funcionario * -- 1 Departamento`, `Funcionario 1 -- * Contato`.

* **`Departamento` (Entidade de Domínio)**
    * **Descrição:** Gerencia as informações dos departamentos da empresa.
    * **Atributos:** `Id`, `NomeDepartamento`.
    * **Relacionamentos:** `Departamento 1 -- * Funcionario`.

### 1.3. Value Objects (VOs)

* **`NomePF`**
    * **Descrição:** Encapsula NomePrincipal, NomeDoMeio e SobreNome para Pessoa Física.
    * **Atributos:** `NomePrincipal`, `NomeDoMeio` (opcional), `SobreNome`.

* **`NomePJ`**
    * **Descrição:** Encapsula Razão Social e Nome Fantasia para Pessoa Jurídica.
    * **Atributos:** `RazaoSocial`, `NomeFantasia` (opcional).

* **`Documento`**
    * **Descrição:** CPF ou CNPJ. Objeto imutável com lógica de validação interna.
    * **Atributos:** `Valor` (apenas dígitos).

* **`ContatoTelefonico`**
    * **Descrição:** Representa um ponto de contato telefônico específico. Agrega um Value Object `Telefone` e metadados descritivos.
    * **Atributos:** `Tipo` (ENUM `UsoTelefone`), `Uso` (ENUM `TipoTelefone`), `EhWhatsapp`, `Telefone` (VO `Telefone`).

* **`Telefone`**
    * **Descrição:** Representa um número de telefone validado (DDD + Número + Ramal opcional). Objeto Imutável.
    * **Atributos:** `DDD`, `Numero`, `Ramal` (opcional).

* **`Email`**
    * **Descrição:** Representa um endereço de email validado. Objeto Imutável.
    * **Atributos:** `Valor`.

* **`Endereco`**
    * **Descrição:** Informações de endereço. Objeto imutável. Agrega VO `CEP`.
    * **Atributos:** `Logradouro`, `Numero` (opcional), `Complemento` (opcional), `Bairro`, `Cidade`, `Estado` (UF), `Cep` (VO `CEP`).

* **`CEP`**
    * **Descrição:** Representa um CEP validado. Objeto Imutável. Usado como parte do Value Object `Endereco`.
    * **Atributos:** `Valor` (apenas dígitos).

### 1.4. Enums de Domínio

* **`UsoTelefone`**
    * **Descrição:** Enum para classificar o uso de um telefone.
    * **Valores:** `NaoEspecificado`, `Comercial`, `Pessoal`, `Outro`.

* **`TipoTelefone`**
    * **Descrição:** Enum para classificar a tecnologia ou tipo de linha de um telefone.
    * **Valores:** `NaoEspecificado`, `Fixo`, `Celular`, `Outro`.

* **`PerfilUsuario`**
    * **Descrição:** Enum para classificar os perfis de acesso dos usuários no sistema.
    * **Valores:** `Administrador`, `Financeiro`, `Operacional`, `Cliente`.

---

## 2. Diagrama Visual de Classes

O diagrama de classes correspondente a esta modelagem de domínio será gerado usando PlantUML e armazenado como imagem PNG na pasta `/docs/arquitetura/visual/`.

* `05_diagrama_classes.plantuml` (código-fonte)
* `05_diagrama_classes.png` (visual gerado)

---

### **3. Considerações de Integração (Mapeamento com Modelos Externos)**

A complexidade de múltiplos "cadastros" para o mesmo CPF/CNPJ em sistemas externos (Next) é tratada pela entidade **`AssinanteNext`**. Esta entidade representa cada serviço/contrato específico do cliente no ecossistema Next, diferenciado por sufixo e IP de servidor. As definições detalhadas dos modelos de integração (IntegMod) para DocSales, Omie e Next, que são utilizados para comunicação com as APIs externas, podem ser encontradas no documento **`14_wiki_tecnica.md`**.