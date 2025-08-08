# 🗄️ 6. Modelo de Dados (DER / Lógico) - SIGVoIP

---

## Objetivo

Este documento consolida o modelo de dados para o sistema SIG VoIP. Ele descreve a **representação conceitual para persistência das entidades de domínio e dos Value Objects (VOs)**, bem como das entidades de infraestrutura, estabelecendo uma base clara para a arquitetura do banco de dados.

---

## 1. Entidades de Persistência e Mapeamento

Esta seção detalha as entidades que serão persistidas no banco de dados, incluindo as entidades de domínio e as entidades de infraestrutura.

### Entidades de Domínio

* **`ProcessamentoProposta`** (Aggregate Root)
    * **Descrição:** Representa uma proposta aprovada do DocSales e seu status de processamento e integração no SIG VoIP.
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `ProposalDocSalesID`: BIGINT
        * `ProposalDocSalesCode`: VARCHAR
        * `ClienteSigID`: GUID (FK)
        * `DataRegistro`: DATETIME
        * `StatusGeral`: VARCHAR (Enum: 'Pendente', 'Processando Cliente Omie', 'Falha Omie', 'Falha Next', 'Concluido')
        * `StatusClienteSIG`: VARCHAR (Enum)
        * `MensagemErroClienteSIG`: TEXT
        * `StatusAssinanteNext`: VARCHAR (Enum)
        * `AssinanteNextID`: GUID (FK, Nullable)
        * `MensagemErroAssinanteNext`: TEXT
        * `StatusClienteOmie`: VARCHAR (Enum)
        * `MensagemErroClienteOmie`: TEXT
        * `StatusContaReceberOmie`: VARCHAR (Enum)
        * `MensagemErroContaReceberOmie`: TEXT
        * `DataUltimaTentativa`: DATETIME
        * `NumeroTentativas`: INTEGER
        * `DadosOriginaisPropostaDocSalesJson`: JSON/TEXT (Nullable)

* **`ClienteSIG`** (Aggregate Root)
    * **Descrição:** Centraliza as informações do cliente no SIG VoIP. É a **fonte única de verdade** para os dados do cliente, mantendo referências a IDs de sistemas integrados.
    * **Unicidade:** Documento (CPF/CNPJ).
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK) - Identificador único do ClienteSIG.
        * `Documento_Valor`: VARCHAR(14) - `Documento` (VO).
        * `TipoDocumentoFiscal`: VARCHAR (Enum: 'CPF' ou 'CNPJ' - derivado de `Documento.Valor`).
        * `NomePF_NomePrincipal`: VARCHAR (Nullable - se PJ)
        * `NomePF_NomeDoMeio`: VARCHAR (Nullable)
        * `NomePF_SobreNome`: VARCHAR (Nullable - se PJ)
        * `NomePJ_RazaoSocial`: VARCHAR (Nullable - se PF)
        * `NomePJ_NomeFantasia`: VARCHAR (Nullable)
        * `NomeExibicao`: VARCHAR (Propriedade computada/derivada).
        * `Email_Valor`: VARCHAR (para **email principal** - `Email` VO).
        * `ContatoTelefonico_Tipo`: VARCHAR (Enum - para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Uso`: VARCHAR (Enum - para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_EhWhatsapp`: BOOL (para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Telefone_DDD`: VARCHAR (para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Telefone_Numero`: VARCHAR (para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Telefone_Ramal`: VARCHAR (Nullable - para **contato principal** - `ContatoTelefonico` VO).
        * `Endereco_Logradouro`: VARCHAR
        * `Endereco_Numero`: VARCHAR
        * `Endereco_Complemento`: VARCHAR (Nullable)
        * `Endereco_Bairro`: VARCHAR
        * `Endereco_Cidade`: VARCHAR
        * `Endereco_Estado`: VARCHAR
        * `Endereco_CEP_Valor`: VARCHAR(8)
        * `DataCriacao`: DATETIME
        * `DataAtualizacao`: DATETIME
        * `UsuarioSigID`: GUID (FK para UsuarioSIG, Nullable)
        * `CustomerDocSalesID`: BIGINT (Nullable) - Referência ao ID do cliente/proposta no DocSales.
        * `ClienteOmieID`: BIGINT (Nullable) - Referência ao ID do cliente no Omie.

* **`UsuarioSIG`** (Entidade de Domínio)
    * **Descrição:** Representa um usuário do sistema SIG VoIP sob a perspectiva do domínio, com credenciais e perfis. Esta entidade de domínio é mapeada para a entidade de infraestrutura e persistência `UsuarioSigBd`.
    * **Regras:** Não pode existir sem ligação a um `ClienteSIG` OU um `Funcionario`. `UserName` e `PasswordHash` devem ser únicos globalmente. Dependência de ativação/existência do `ClienteSIG` ou `Funcionario` associado.
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `UserName`: VARCHAR
        * `PasswordHash`: VARCHAR
        * `Email_Valor`: VARCHAR - Email do usuário (`Email` VO).
        * `Ativo`: BOOL
        * `ClienteSigID`: GUID (FK para ClienteSIG, Nullable - se o usuário for um funcionário)
        * `FuncionarioID`: GUID (FK para Funcionario, Nullable - se o usuário for um cliente)
        * `DataCriacao`: DATETIME
        * `Perfis`: LIST Enum `PerfilUsuario` (Ex: 'Administrador', 'Financeiro').

* **`ServidorNext`** (Aggregate Root)
    * **Descrição:** Gerencia as informações de cada servidor Next (NextBilling/NextRouter), incluindo capacidade.
    * **Unicidade:** Nome.
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `Nome`: VARCHAR (Unique Index)
        * `TipoServidor`: VARCHAR (Enum: 'NextBilling', 'NextRouter', 'NextRouterDID')
        * `UrlApi`: VARCHAR
        * `ChaveApi`: VARCHAR
        * `IpPrincipal`: VARCHAR
        * `IpsSecundarios`: VARCHAR/JSON
        * `CapacidadeTotalClientes`: INTEGER
        * `CapacidadeTotalDIDs`: INTEGER
        * `ClientesAtuais`: INTEGER
        * `DIDsAtuais`: INTEGER
        * `Ativo`: BOOLEAN
        * `DataUltimaAtualizacaoDados`: DATETIME (Nullable)

* **`AssinanteNext`** (Entidade)
    * **Descrição:** Representa um serviço específico contratado por um `ClienteSIG` e suas configurações no sistema Next. Substitui o conceito genérico de `ServicoContratado`.
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `AssinanteID`: VARCHAR (ID do assinante no Next para este serviço)
        * `SufixoAssinante`: VARCHAR (Sufixo no nome do cliente que o diferencia no Next, ex: "XYZ - DID")
        * `NomeClienteNoNext`: VARCHAR (O nome do cliente com o sufixo que o diferencia no Next)
        * `IpServidorNext`: VARCHAR (O IP do servidor Next onde este cadastro específico está alocado - **a ser consolidado/melhor estudado com `ServidorIPs`**).
        * `ClienteSigID`: GUID (FK para ClienteSIG).
        * *Outras colunas para detalhes do serviço (tipo, plano, ramais, URAs, DIDs) - não especificadas em detalhes.*

* **`Contato`** (Entidade)
    * **Descrição:** Representa uma **Pessoa** que é um contato de referência para um `ClienteSIG` (funcionário do cliente PJ, ou contato pessoal para PF) ou para um `Funcionario` (contato de emergência/pessoal).
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `ClienteSigID`: GUID (FK para ClienteSIG, Nullable - se for contato de ClienteSIG)
        * `FuncionarioId`: GUID (FK para Funcionario, Nullable - se for contato de Funcionario)
        * `NomePF_NomePrincipal`: VARCHAR
        * `NomePF_NomeDoMeio`: VARCHAR (Nullable)
        * `NomePF_SobreNome`: VARCHAR (Nullable)
        * `Departamento`: VARCHAR (Nullable - ex: "Financeiro" para PJ)
        * `Cargo`: VARCHAR (Nullable - ex: "Gerente de Contas")
        * `Observacao`: TEXT (Nullable)

* **`Funcionario`** (Entidade)
    * **Descrição:** Representa um funcionário da empresa.
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `NomePF_NomePrincipal`: VARCHAR
        * `NomePF_NomeDoMeio`: VARCHAR (Nullable)
        * `NomePF_SobreNome`: VARCHAR
        * `Documento_Valor`: VARCHAR(14) - `Documento` VO.
        * `Endereco_Logradouro`: VARCHAR
        * `Endereco_Numero`: VARCHAR
        * `Endereco_Complemento`: VARCHAR (Nullable)
        * `Endereco_Bairro`: VARCHAR
        * `Endereco_Cidade`: VARCHAR
        * `Endereco_Estado`: VARCHAR
        * `Endereco_CEP_Valor`: VARCHAR(8)
        * `Email_Valor`: VARCHAR (para **email principal** - `Email` VO).
        * `ContatoTelefonico_Tipo`: VARCHAR (Enum - para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Uso`: VARCHAR (Enum - para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_EhWhatsapp`: BOOL (para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Telefone_DDD`: VARCHAR (para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Telefone_Numero`: VARCHAR (para **contato principal** - `ContatoTelefonico` VO).
        * `ContatoTelefonico_Telefone_Ramal`: VARCHAR (Nullable - para **contato principal** - `ContatoTelefonico` VO).
        * `DataCriacao`: DATETIME
        * `DataAtualizacao`: DATETIME
        * `Cargo`: VARCHAR (Nullable)
        * `Ativo`: BOOL
        * `UsuarioSigBdID`: GUID (FK para UsuarioSigBd, Nullable)
        * `DepartamentoId`: GUID (FK para Departamento, Nullable)

* **`Departamento`** (Entidade)
    * **Descrição:** Representa os departamentos da empresa.
    * **Atributos Conceituais (Colunas para Persistência):**
        * `Id`: GUID (PK)
        * `NomeDepartamento`: VARCHAR (Unique)

### Entidades de Infraestrutura para Persistência

* **`UsuarioSigBd`** (Herda de `IdentityUser`)
    * **Descrição:** Representa um usuário para autenticação/autorização via ASP.NET Core Identity. Esta entidade é a representação persistente do `UsuarioSIG` de domínio.
    * **Atributos:**
        * `Id`: GUID (PK)
        * `UserName`: VARCHAR
        * `NormalizedUserName`: VARCHAR
        * `Email`: VARCHAR
        * `NormalizedEmail`: VARCHAR
        * `EmailConfirmed`: BOOL
        * `PasswordHash`: VARCHAR
        * `SecurityStamp`: VARCHAR
        * `ConcurrencyStamp`: VARCHAR
        * `TwoFactorEnabled`: VARCHAR
        * `LockoutEnd`: DATETIMEOFFSET (Nullable)
        * `LockoutEnabled`: BOOL
        * `AccessFailedCount`: INT
        * `UsuarioSIGId`: GUID (FK para UsuarioSIG) - **Observação:** Este FK vinculará esta entidade de infraestrutura à entidade de domínio `UsuarioSIG`.
        * `Ativo`: BOOLEAN
        * `ClienteSigID`: GUID (FK para ClienteSIG, Nullable)
        * `FuncionarioID`: GUID (FK para Funcionario, Nullable)

* **`IdentityUserRole`** (Tabela de Junção)
    * **Descrição:** Representa o relacionamento N:N entre usuários e perfis no ASP.NET Core Identity.
    * **Atributos:**
        * `RoleId`: GUID (FK)
        * `UserId`: GUID (FK)
        * **PK Composta:** (`RoleId`, `UserId`)

* **`IdentityRole`**
    * **Descrição:** Representa um papel/perfil de usuário no ASP.NET Core Identity.
    * **Atributos:**
        * `Id`: GUID (PK)
        * `Name`: VARCHAR (Unique Index)
        * `NormalizedName`: VARCHAR (Unique Index)

### Tabelas de Junção para Coleções de Value Objects

* **`Contato_ContatosTelefonico`**
    * **Descrição:** Tabela de junção para a lista de telefones de um `Contato`.
    * **Colunas:**
        * `ContatoId`: GUID (FK para Contato)
        * `Tipo`: VARCHAR (Enum)
        * `Uso`: VARCHAR (Enum)
        * `EhWhatsapp`: BOOL
        * `Telefone_DDD`: VARCHAR
        * `Telefone_Numero`: VARCHAR
        * `Telefone_Ramal`: VARCHAR (Nullable)
        * **PK Composta:** (`ContatoId`, `Telefone_DDD`, `Telefone_Numero`)

* **`Contato_Emails`**
    * **Descrição:** Tabela de junção para a lista de e-mails de um `Contato`.
    * **Colunas:**
        * `ContatoId`: GUID (FK para Contato)
        * `Email_Valor`: VARCHAR
        * **PK Composta:** (`ContatoId`, `Email_Valor`)

* **`AssinanteNextServidorIPs`**
    * **Descrição:** Tabela de junção para associar IPs de servidores a `AssinanteNext`s.
    * **Colunas:**
        * `AssinanteNextId`: GUID (FK para AssinanteNext)
        * `ServidorNextId`: GUID (FK para ServidorNext)
        * `IP`: VARCHAR (Pode ser um atributo, ou uma FK para uma tabela de IPs, dependendo do detalhamento futuro)
        * **PK Composta:** (`AssinanteNextId`, `ServidorNextId`, `IP`) - *A ser refinada conforme detalhamento do ServidorNext*.

---

## 2. Value Objects (VOs)

Os **Value Objects (VOs)** descrevem ou quantificam, são imutáveis e comparáveis por seus valores. Na persistência, VOs são geralmente "achatados" em colunas da entidade que os contém ou, se forem coleções, representados por tabelas de junção, como detalhado acima.

* **`NomePF`**
    * **Descrição:** Encapsula NomePrincipal, NomeDoMeio e SobreNome para Pessoa Física.
    * **Atributos:** `NomePrincipal`, `NomeDoMeio` (opcional), `SobreNome`.

* **`NomePJ`**
    * **Descrição:** Encapsula Razão Social e Nome Fantasia para Pessoa Jurídica.
    * **Atributos:** `RazaoSocial`, `NomeFantasia` (opcional).

* **`Documento`**
    * **Descrição:** CPF ou CNPJ. Objeto imutável com lógica de validação interna.
    * **Atributos:** `Valor` (VARCHAR(14) - apenas dígitos).

* **`ContatoTelefonico`**
    * **Descrição:** Representa um ponto de contato telefônico específico, agregando um `Telefone` (VO) e metadados.
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
    * **Descrição:** Representa um CEP validado. Objeto Imutável.
    * **Atributos:** `Valor` (VARCHAR(8) - apenas dígitos).

### Enums de Domínio Persistidos

* **`UsoTelefone`**
    * **Descrição:** Enum para classificar o uso de um telefone.
    * **Valores:** `NaoEspecificado`, `Comercial`, `Pessoal`, `Outro`.

* **`TipoTelefone`**
    * **Descrição:** Enum para classificar a tecnologia ou tipo de linha de um telefone.
    * **Valores:** `NaoEspecificado`, `Fixo`, `Celular`, `Outro`.

---

## 3. Relacionamentos

Esta seção descreve as associações entre as entidades e Value Objects, conforme mapeado para o banco de dados.

* `ProcessamentoProposta` (N) para `ClienteSIG` (1): Um `ClienteSIG` pode ter múltiplos `ProcessamentoProposta`.
* `ProcessamentoProposta` (N) para `AssinanteNext` (0 ou 1): Um `ProcessamentoProposta` pode estar relacionado a um `AssinanteNext` (se bem-sucedido).
* `ClienteSIG` (1) para `UsuarioSIG` (N): Um `UsuarioSIG` pode ser cliente do `ClienteSIG`.
* `ClienteSIG` (1) para `AssinanteNext` (N): Um `ClienteSIG` pode ter múltiplos `AssinanteNext` (serviços Next).
* `ClienteSIG` (1) para `Contato` (N): Um `ClienteSIG` pode ter múltiplos `Contatos` (Pessoas de referência). **Para PJ, ao menos um `Contato` é obrigatório para comunicação.**
* `UsuarioSigBd` (1) para `UsuarioSIG` (1): `UsuarioSigBd` representa a persistência do `UsuarioSIG` (relação 1:1 de persistência).
* `UsuarioSigBd` (N) para `IdentityRole` (N): Muitos para muitos via tabela de junção `IdentityUserRole`.
* `Funcionario` (1) para `UsuarioSigBd` (N): Um `UsuarioSigBd` pode estar relacionado a um `Funcionario`.
* `Funcionario` (N) para `Departamento` (1): Múltiplos `Funcionario`s podem pertencer a um `Departamento`.
* `Funcionario` (1) para `Contato` (N): Um `Funcionario` pode ter múltiplos `Contatos` (contatos de referência pessoais).
* `Contato` (1) para `Contato_ContatosTelefonico` (N): Um `Contato` pode ter múltiplos telefones.
* `Contato` (1) para `Contato_Emails` (N): Um `Contato` pode ter múltiplos e-mails.
* `AssinanteNext` (N) para `ServidorNext` (N): Muitos para muitos via tabela de junção `AssinanteNextServidorIPs`.
* `ClienteSIG` armazena diretamente as referências a IDs de clientes em sistemas externos (`CustomerDocSalesID`, `ClienteOmieID`).

---

### **Instruções para Arquivos Visuais:**

O diagrama DER (Diagrama de Entidade-Relacionamento) correspondente a este modelo de dados será gerado usando PlantUML e armazenado como imagem PNG na pasta `/docs/arquitetura/visual/`.

* `06_modelo_dados.plantuml` (código-fonte)
* `06_DER.png` (visual gerado)