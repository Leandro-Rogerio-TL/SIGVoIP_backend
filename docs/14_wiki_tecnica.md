# 📚 14. Wiki Técnica - SIG VoIP

---

## 🔹 1. Visão Geral e Contexto Técnico

O **SIG VoIP** é um sistema de integração centralizada, orquestrando serviços da DocSales, Omie e dos servidores Next (NextBilling e NextRouter). Seu principal objetivo é **centralizar e automatizar a integração** de dados entre esses sistemas para otimizar processos, reduzir erros e consolidar informações. A **Fase 1** do projeto foca na automação do fluxo de propostas aprovadas no DocSales, visando eliminar entradas manuais, reduzir erros e acelerar o processo de pós-venda e onboarding de clientes.

A motivação central do SIG VoIP é eliminar ineficiências operacionais, erros manuais e perdas financeiras resultantes de dados repetitivos e inconsistências entre sistemas legados. Buscamos a **redução de erros humanos**, o **aumento da eficiência no onboarding de clientes** e a **centralização da verdade dos dados** em um único ponto, a entidade **`ClienteSIG`**.

---

## 🔹 2. Diretrizes de Desenvolvimento

Como equipe de desenvolvimento do SIG VoIP, estamos empenhados em construir um software de alto padrão. Estas diretrizes estabelecem os princípios e práticas para garantir a qualidade, manutenibilidade e escalabilidade do nosso código. O projeto é conduzido por Leandro Rogerio, com o auxílio de ferramentas de IA, assegurando a adesão aos princípios de design e arquitetura e contribuindo ativamente na implementação.

### 2.1. Princípios de Design e Arquitetura

O SIG VoIP adota uma **arquitetura híbrida**, combinando um modelo em **camadas** com a aplicação rigorosa dos princípios da **Arquitetura Hexagonal (Ports and Adapters)** no core da aplicação (Domínio e Aplicação). As camadas de Infraestrutura e Apresentação/API atuam como adaptadores para tecnologias externas.

* **Arquitetura Orientada ao Domínio (DDD - Domain-Driven Design):** Nosso código reflete o domínio do negócio, utilizando uma linguagem ubíqua clara. Modelamos **Aggregates** (ex: `ClienteSIG`), **Entities**, **Value Objects** (ex: `DocumentoFiscal`, `Endereco`, `Contato`, `Email`, `Telefone`, `CEP`) e **Domain Services** para organizar a lógica de negócio.
* **Princípios SOLID:** Aplicamos os cinco princípios, com foco especial no **Princípio da Inversão de Dependência (DIP)**. As camadas internas (Domínio, Aplicação) dependem de abstrações, não de implementações concretas externas, garantindo um alto nível de desacoplamento.
* **Arquitetura Limpa (Clean Architecture / Hexagonal):** Mantemos a lógica de negócio (Domínio) no centro, independente de frameworks e tecnologias. A **Regra de Dependência** é estritamente seguida: dependências de código sempre apontam para as camadas mais internas. Há uma separação clara entre as camadas: Domínio, Aplicação (casos de uso) e Adaptadores/Apresentação (API).
    * **Nota da Fase 1:** Priorizamos a implementação do Domínio e das integrações, abstraindo a persistência. O Domínio é projetado para ser facilmente persistido no futuro.
* **Estilo Funcional (FP):** Priorizamos funções puras e imutabilidade em helpers de domínio para reduzir efeitos colaterais e aumentar a testabilidade, buscando reduzir a complexidade de `if/else` quando apropriado.
* **Padrões de Projeto:** Aplicamos soluções comprovadas (GoF, Fowler) para problemas comuns de design, buscando simplicidade e manutenibilidade.

### 2.2. Qualidade do Código

Buscamos um **código de alto padrão** através de:

* **Clean Code:** Código legível com nomes significativos, funções pequenas e focadas, comentários concisos explicando o "porquê" e formatação consistente.
* **Object Calisthenics:** Aplicamos os nove exercícios para promover código coeso, baixo acoplamento e encapsulamento.
* **DRY (Don't Repeat Yourself):** Evitamos duplicação de código, extraindo lógicas comuns para reuso.
* **KISS (Keep It Simple, Stupid):** Buscamos a solução mais simples e direta para cada problema.
* **YAGNI (You Ain't Gonna Need It):** Implementamos apenas o que é necessário para os requisitos atuais, evitando over-engineering.
* **Linguagem Ubíqua:** Usamos a linguagem do domínio em todo o código, nomeando classes, métodos e variáveis com termos do negócio.
* **Imutabilidade:** Priorizamos Value Objects imutáveis e controlamos estados internos de entidades via métodos.
* **Consistência Rigorosa:** Garantimos a aplicação uniforme dos padrões em todo o código, especialmente em VOs, Entidades, Repositórios e tratamento de exceções.
* **Value Objects:** Utilizamos Value Objects para encapsular conceitos de valor e suas validações. A validação de invariantes é feita no construtor ou métodos de fábrica do próprio VO, utilizando métodos estáticos privados ou helpers 'internal static' para lógica reutilizável. Campos opcionais (`string?`) em VOs devem ter validação de formato aplicada apenas se o valor não for `null`, vazio ou apenas `whitespace`.

### 2.3. Testes Automatizados

* **Testes Unitários:** Escrevemos testes para todas as camadas, com foco no Domínio e Aplicação, usando xUnit/NUnit. Aplicamos AAA (Arrange, Act, Assert) e isolamos dependências externas com Mocks/Stubs. Testamos classes e métodos das camadas de Domínio e Aplicação de forma isolada.
* **Testes de Integração:** Verificamos a interação entre diferentes partes do sistema (ex: Aplicação com Infraestrutura), podendo envolver um banco de dados de teste (mas não APIs externas nesta fase inicial). Testamos a interação entre componentes e sistemas externos como banco de dados e APIs externas.
* **Cobertura de Testes:** Buscamos uma cobertura de código acima de 80%, priorizando a lógica de negócio crítica.

### 2.4. Logging e Tratamento de Exceções

* **Logging Estruturado (Serilog):** Utilizamos Serilog para registrar eventos relevantes em diferentes níveis (Debug, Information, Warning, Error, Fatal). Incluímos contexto nas mensagens de log (IDs de pedido, IDs de cliente SIG VoIP, detalhes do erro, stack trace). Configuramos sinks apropriados e garantimos que informações sensíveis não sejam logadas. Registramos falhas de autenticação, requisições inválidas e problemas de mapeamento.
    * **Estratégia de Logging por Camada:**
        * **Domínio:** Exceções específicas e relevantes à lógica de negócio.
        * **Aplicação:** Início/fim de casos de uso, eventos de negócio importantes.
        * **Infraestrutura:** Erros de comunicação com APIs externas, problemas de persistência.
        * **Apresentação/API:** Requisições recebidas, erros de validação da API, falhas de autenticação/autorização.
* **Tratamento de Exceções:** Retornos apropriados são gerados para rastreamento e reprocessamento.
    * **Na camada de Domínio, é PROIBIDO capturar exceções genéricas (`catch (Exception)`).** Exceções capturadas no Domínio devem ser específicas (ex: `ArgumentException` de helpers, exceções de VOs aninhados) e devem ser relançadas como **Exceções de Domínio customizadas**, **SEMPRE encapsulando a exceção original como `InnerException`**.
    * É **CRÍTICO** garantir que a mensagem da exceção de domínio seja coerente, clara e contextualizada, **COMPOSTA CUIDADOSAMENTE** para evitar mensagens desconexas ao combinar a mensagem original da `InnerException` (ex: 'Documento fiscal inválido: [Mensagem do Helper ou VO Aninhado]').
    * Avaliamos a estratégia de tratamento centralizado de exceções no fluxo de entrada da aplicação (ex: middleware da API para serializar exceções de domínio em respostas HTTP 400).

### 2.5. CI/CD (Integração Contínua e Entrega Contínua)

Implementamos um pipeline de CI/CD (ex: GitHub Actions, Azure DevOps Pipelines) para automatizar o build, execução de testes automatizados e deploy da aplicação. Cada push ou Pull Request aciona o pipeline de CI, que garante a compilação, a aprovação dos testes e a qualidade do código antes do merge para a branch principal. O pipeline de integração contínua será configurado após a entrega da Fase 1. Implementaremos deploy automatizado para Homologação e Produção em fases futuras.

### 2.6. Refatoração Contínua

A refatoração é uma parte integrante do processo de desenvolvimento. Melhoramos a estrutura e legibilidade do código continuamente, sem alterar o comportamento funcional, utilizando métricas de qualidade e feedback dos testes para identificar áreas que precisam de refatoração.

### 2.7. Metodologia e Workflow da Equipe

Utilizamos uma **metodologia ágil** para o gerenciamento e execução do projeto, com o framework **Kanban** para gerenciar o fluxo de trabalho de forma eficiente.

### 2.8. Comunicação e Documentação

A comunicação e a documentação são essenciais e são gerenciadas através de:

* **Arquivos Markdown (`.md`)**: Formato primário para a documentação textual.
* **Arquivo de Configuração JSON (`projeto_sig_voip.json`)**: Atualizado de forma colaborativa para configurações do projeto.
* **Documentação de Decisões de Arquitetura (ADRs)**: Prática contínua para registrar e justificar decisões técnicas importantes.

### 2.9. Segurança

Seguimos as melhores práticas de segurança (OWASP) e protegemos dados sensíveis. Usamos **ASP.NET Core Identity** para autenticação e **JWT (JSON Web Tokens)** para autorização, protegendo endpoints da API RESTful através de **Policies e Roles**.

---

## 🔹 3. Tecnologias e Arquitetura Detalhada

### 3.1. Tecnologias Principais

* **.NET 8.0** (C#)
* **ASP.NET Core Web API**
* **Entity Framework Core** (provider Pomelo para MySQL)
* **MySQL**
* **JWT** para autenticação
* **AutoMapper** para mapeamento de objetos
* **Serilog** para logging
* **Swagger (Swashbuckle)** para documentação da API
* **Git** para controle de versão
* **Markdown** e **Diagramas `.drawio`** para documentação
* **TypeScript** (Frontend, desenvolvimento futuro)
* **Vue.js** (Frontend, desenvolvimento futuro)

### 3.2. Estrutura Arquitetural (Camadas)

A estrutura física será organizada em múltiplos projetos .NET, seguindo rigorosamente a **Regra de Dependência**.

* **Domínio (.NET 8 C#)**
    * **Função:** Contém a **lógica de negócio primordial** e as regras mais críticas do sistema. É o coração da aplicação, agnóstica a frameworks e tecnologias de infraestrutura/aplicação externa.
    * **Características:** **Independente** de qualquer framework, banco de dados ou interface de usuário. Define **interfaces (Portas)** que as camadas externas deverão implementar (e.g., `IClienteSIGRepository`). Validações de invariantes de domínio são internas aos objetos, no construtor ou métodos, usando helpers estáticos internos do projeto.
    * **Conteúdo:** **Entidades** (ex: `ClienteSIG`, `UsuarioSIG`, `ServidorNext`), **Value Objects** (ex: `DocumentoFiscal`, `Endereco`, `Contato`, `Email`, `Telefone`, `CEP`), **Domain Services**, **Interfaces de Repositório**, **Exceções de Domínio**.

* **Aplicação (.NET 8 C#)**
    * **Função:** Orquestra os **casos de uso** da aplicação. Não contém lógica de negócio primária, mas sim a lógica de *como* a lógica de negócio é utilizada.
    * **Características:** Depende da camada de Domínio. Define as operações (casos de uso) que o sistema pode realizar.
    * **Conteúdo:** **Application Services (Use Cases)** (ex: `ClienteAppService`, `PedidoDocSalesAppService`), **Interfaces para Serviços de Integração** (ex: `IDocSalesService`, `IOmieService`, `INextBillingService`).

* **Infraestrutura (.NET 8 C#)**
    * **Função:** **Implementa as interfaces** definidas nas camadas de Domínio e Aplicação. É onde os "adaptadores" para o mundo externo residem.
    * **Características:** Contém os detalhes de implementação de tecnologias específicas.
    * **Conteúdo:** **Implementações de Repositório** (usando ORM como Entity Framework Core), **Adaptadores de API** (consumindo e expondo APIs externas), **Configurações de Persistência**, **Implementações de Logging, Caching, etc.**

* **Adaptadores / Apresentação (.NET 8 C#)**
    * **Função:** É a porta de entrada e saída do sistema para usuários e outros sistemas.
    * **Características:** A camada mais externa. Depende da camada de Aplicação. Traduz requisições externas para o formato interno e as respostas internas para o formato externo.
    * **Conteúdo:** **Controllers da API RESTful**, **Data Transfer Objects (DTOs)**, **Middlewares**.

### 3.3. Regra de Dependência

A regra mais crucial da Clean Architecture: **as dependências de código SEMPRE apontam para o interior.**

* Apresentação `->` Aplicação
* Infraestrutura `->` Aplicação ou Domínio (implementa interfaces definidas no Domínio/Aplicação)
* Aplicação `->` Domínio

Isso garante que o Domínio (lógica de negócio) permaneça intocado por mudanças em tecnologias externas ou interfaces.

### 3.4. Fluxo de Requisição (Exemplo Básico)

1.  **Requisição chega:** Um cliente externo faz uma requisição HTTP para a **API RESTful** (Camada de Apresentação).
2.  **Encaminhamento para Aplicação:** O **Controller** relevante recebe a requisição, valida os dados e invoca o **Application Service** apropriado na camada de **Aplicação**.
3.  **Lógica de Negócio e Domínio:** O **Application Service** utiliza as **Entidades** e **Value Objects** do **Domínio** para executar a lógica de negócio. Se precisar de dados persistidos, ele usa a **interface de Repositório** (definida no Domínio).
4.  **Acesso à Infraestrutura:** A **Infraestrutura** (que implementa a interface de Repositório) é então chamada para buscar ou salvar dados no banco. Se houver integração externa, a **Infraestrutura** (que implementa a interface de Integração) se comunica com a API externa.
5.  **Resposta:** Após o processamento, a resposta é enviada de volta através das camadas (Aplicação para Apresentação), formatada em um DTO e retornada ao cliente.

### 3.5. Responsabilidades dos Adaptadores de API (Infraestrutura)

Esses adaptadores são a ponte para o mundo externo:

* **Tradução de Dados:** Convertem os modelos de dados da API externa para os modelos do nosso Domínio/Aplicação e vice-versa.
* **Autenticação e Autorização:** Gerenciam credenciais e tokens para se comunicar com as APIs externas.
* **Tratamento de Erros:** Implementam lógicas de resiliência (retentativas, circuit breakers) e registram falhas de comunicação.

### 3.6. Autenticação e Autorização Interna do Sistema

* **Autenticação:** Gerenciada pelo **ASP.NET Core Identity** na camada de Infraestrutura. Os usuários do sistema SIG VoIP (`UsuarioSIG`) terão suas credenciais armazenadas e validadas internamente.
* **Autorização:** Implementada via **JWT (JSON Web Tokens)**. Os tokens, contendo *claims* (informações) sobre as permissões e *roles* do `UsuarioSIG`, protegerão os endpoints da API RESTful, controlando o acesso aos recursos do sistema. A autorização é implementada via **Policies e Roles** usando ASP.NET Core Authorization na camada de Apresentação/API, verificando *claims* do token JWT derivado dos *Roles* do `UsuarioSIG`.

---

## 🔹 4. Modelagem do Domínio e Entidades Principais

### 4.1. Funcionalidades-Chave

* Importação automática de propostas aprovadas (DocSales).
* Mapeamento e integração com Omie e Next.
* Aplicação de regras de negócio personalizadas.
* Controle e rastreamento de status de integração.
* Persistência Local de `ClienteSIG`.

### 4.2. Entidades Principais

* `ClienteSIG`: Entidade persistida representando o cliente principal no SIG VoIP. É a **Aggregate Root central**.
* `ProposalDocSales`: Integration Model para trafegar e tratar a "proposta" obtida no DocSales.
* `UsuarioSIG`: Entidade para gerenciamento de usuários internos.
* `ServidorNext`: Entidade para controle dos servidores Next.
* `AssinanteNext`, `ClienteOmie`, `ContaReceberOmie`: Entidades externas utilizadas no mapeamento de saída.
* `ServicoContratado`: Nova entidade para gerenciar contratos/serviços específicos e suas integrações.

### 4.3. Value Objects (VOs)

* `DocumentoFiscal`
* `CEP`
* `Endereco`
* `Email`
* `Telefone`
* `Contato`
* `ContatoTelefonico`
* `ConjuntoDeContatos` (VO de Agrupamento)
* `PessoaContato`
* `ClienteNextIntegracao`
* `ClienteOmieIntegracao`

### 4.4. Regras de Negócio

* Apenas propostas assinadas e não processadas devem ser importadas.
* Um `ClienteSIG` pode ser mapeado para múltiplos sistemas externos.
* A persistência ocorre apenas para `ClienteSIG`.
* Regras específicas são aplicadas no momento da integração com cada sistema.

### 4.5. Detalhes da Entidade `ClienteSIG`

A entidade `ClienteSIG` é a **Aggregate Root central** para todas as informações de cliente no sistema SIG VoIP. Representa de forma unificada um cliente (Pessoa Física ou Jurídica) e gerencia seus dados de integração com plataformas externas.

#### Propriedades Essenciais:

* **`Id`** (`Guid`): Identificador único no sistema.
* **`DocumentoFiscal`** (`Value Object`): CPF ou CNPJ (`EhCPF`, `EhCNPJ`).
* **`NomePessoaFisica`** (`Value Object: NomePF?`): Nome completo PF (obrigatório se CPF).
* **`NomePessoaJuridica`** (`Value Object: NomePJ?`): Razão Social PJ (obrigatório se CNPJ).
* **`NomeExibicao`** (`string`): Propriedade computada para identificação amigável.
* **`Endereco`** (`Value Object: Endereco`).
* **`Email`** (`Value Object: Email?`): Email principal.
* **`ContatoTelefonico`** (`Value Object: ContatoTelefonico?`): Telefone principal.
* **`ContatosPessoais`** (`List<ContatoCliente>`):
    * **PJ:** Lista **obrigatória**, mínimo um `ContatoCliente`.
    * **PF:** Lista **opcional**.
* **`UsuarioSistema`** (`Entidade`): Referência 1:1 para o usuário de acesso ao SistemaSIG, com criação encapsulada no `ClienteSIG`.
* **`ServicosContratados`** (`List<ServicoContratado>`): Nova entidade para serviços/contratos específicos e suas integrações.
* **Auditoria:** `DataCriacao`, `DataAtualizacao` (`DateTime`).

#### Comportamentos e Validações:

* **Construtores:** `CriarClientePF(DocumentoFiscal cpf, NomePF nomePF, ...)` (validações para PF); `CriarClientePJ(DocumentoFiscal cnpj, NomePJ nomePJ, List<ContatoCliente> contatos, ...)` (validações para PJ, incluindo obrigatoriedade de contato).
* **Métodos de Gerenciamento:** `AtualizarDadosPrincipais`, `AlterarStatus`, `AdicionarContato`, `RemoverContato`, `AtualizarDetalhesContato`, `GerenciarEmailsContato`, `GerenciarContatosTelefonicosContato`, `AtualizarLoginUsuario`, `AtualizarEmailRecuperacaoUsuario`, `AlterarStatusUsuario`, `AdicionarServicoContratado`, `RemoverServicoContratado`, `AtualizarServicoContratado`.
* **Regras e Validações Gerais:** `ClienteSIG` é único por `DocumentoFiscal` (validação em nível de aplicação/repositório). Violações de regras lançam `InvalidClienteSIGException`. A consistência do agregado é garantida manipulando seus componentes apenas através da Aggregate Root.

### 4.6. Modelagem de Serviços e Integrações Externas

A entidade **`ServicoContratado`** gerencia a complexidade de múltiplos cadastros para o mesmo CPF/CNPJ em sistemas externos (Next, Omie).

#### Peculiaridades dos Sistemas Externos:

* **NextBilling/NextRouter:** Múltiplos cadastros por CNPJ para diferentes serviços, cada um com `ID` e `IP` de servidor próprios.
* **Omie:** Múltiplas representações/contas por cliente.
* **DocSales:** Múltiplos contratos/propostas vinculados ao cadastro principal.

#### Solução de Modelagem:

* **`ClienteSIG`**: Cliente único e central.
* **`ServicoContratado`** (associada ao `ClienteSIG`): Representa cada contrato/plano/serviço.
    * Contém `IdDocSales`.
    * **`ClienteNextIntegracao`** (`Value Object`): Encapsula `IdNext`, `NomeClienteNoNext`, `IpServidorNext`.
    * **`ClienteOmieIntegracao`** (`Value Object`): Encapsula `IdOmie`, `NomeClienteNoOmie` e outras informações relevantes.
    * Outras propriedades específicas do serviço.

Esta abordagem permite que o `ClienteSIG` mantenha sua identidade única enquanto `ServicoContratado` e seus VOs lidam com as representações e detalhes operacionais externos.

---

## 🔹 5. Registro de Decisões Técnicas (ADRs)

Este projeto utiliza Architectural Decision Records (ADRs) para documentar decisões arquiteturais e técnicas significativas, garantindo rastreabilidade e contexto para a equipe. Os ADRs individuais são armazenados em `/docs/arquitetura/adr/`.

### 5.1. ADR: Estrutura de Modelagem de Contatos no Domínio (Data da Decisão: 2025-06-21)

#### Contexto:
O domínio do SIG VoIP precisa modelar as informações de contato para a entidade `ClienteSIG`, considerando clientes PF e PJ, múltiplos telefones e metadados.

#### Decisão:
Adotar a seguinte estrutura conceitual para modelagem de contatos no domínio SIG VoIP:

1.  **Value Object `Telefone`**: DDD, Número, Ramal opcional. Validação interna de formato, lançando `InvalidTelefoneException` (encapsulando `ArgumentException`).
2.  **Value Object `Email`**: Endereço validado. Validação interna de formato, lançando `InvalidEmailException` (encapsulando `ArgumentException`).
3.  **Enums `UsoTelefone` e `TipoTelefone`**: Enums de Domínio para classificar uso (Comercial, Pessoal) e tipo (Fixo, Celular).
4.  **Value Object `ContatoTelefonico`**: Agrega um VO `Telefone` e propriedades para tipologia (`Uso`, `Tipo`, `EhWhatsapp`) e descrição livre. Validação de coerência interna, lançando `InvalidValueObjectException`.
5.  **Value Object de Agrupamento (`ConjuntoDeContatos`)**: Agrupa um VO `Email` principal (opcional) e uma lista de VOs `ContatoTelefonico`. Usado para contatos gerais de uma entidade ou de uma pessoa específica.
6.  **Value Object `PessoaContato`**: Representa uma pessoa de contato específica para um Cliente Empresa. Agrega Nome, Cargo/Departamento (opcional) e um VO de Agrupamento (`ConjuntoDeContatos`) para seus próprios canais de contato.
7.  **Entidade `ClienteSIG`**: Agrega os VOs de `Endereco`, um VO de Agrupamento (`ConjuntoDeContatos`) para seus contatos gerais (`ContatosGerais`), e uma lista de VOs `PessoaContato` (`PessoasDeContato`).

#### Implicações:
Modelagem clara e expressiva, imutabilidade dos VOs, validação centralizada e garantida, uso de exceções de domínio customizadas com encapsulamento de `InnerException` e mensagens coerentes. Flexibilidade para diferentes tipos de contatos.

#### Pontos Pendentes e Em Progresso:
* Refinamento da Entidade `ClienteSIG` como Aggregate Root.
* Validações internas do VO `ContatoTelefonico` (coerência entre Uso, Tipo, WhatsApp, Ramal) a serem detalhadas e implementadas.
* Avaliar aplicação de Objetos Calistênicos e Programação Funcional para reduzir `if/else`.
* Definição da estratégia de tratamento centralizado de exceções (ex: middleware da API).
* Implementação conceitual e inicial dos VOs de Contato e Enums.

---

## 🔹 6. Especificações de Integração com APIs Externas (Integration Models - IntegMod)

Estes modelos representam as estruturas de dados utilizadas para interagir com as APIs dos sistemas externos (DocSales, Omie, Next). Eles espelham os contratos das APIs e são cruciais para a comunicação e o mapeamento com as entidades de domínio do SIG VoIP.

### 6.1. DocSales IntegMod

Representam o payload recebido do DocSales.

* **`ProposalDocSales`**
    * `id` (long): ID no DocSales
    * `code` (string)
    * `status` (string): "approved"
    * `contract_start` (string): Data de início do contrato (DD/MM/YYYY)
    * `contract_months` (int)
    * `contract_end` (string): Data de fim do contrato (DD/MM/YYYY)
    * `parcels_count` (int)
    * `payment_month` (string)
    * `adjustment_renewal` (string)
    * `discount` (string)
    * `note` (string)
    * `mrr_value` (string)
    * `custom_data_de_vencimento_da_mensalidade` (string)
    * `payment_method_name` (string)
    * `custom_forma_de_pagamento_para_wholesale` (string)
    * `customer` (`CustomerDocSales`)
    * `products` (List<`ServiceDocSales`>)
    * `parcels` (List<`ParcelaDocSales`>): Parcelas da proposta

* **`CustomerDocSales`**
    * `id` (long): ID do cliente no DocSales
    * `code` (string)
    * `status` (string)
    * `name` (string)
    * `alias` (string)
    * `legal_code` (string): CPF/CNPJ
    * `address` (string): Endereço completo
    * `number` (string)
    * `complement` (string)
    * `district` (string)
    * `city` (string)
    * `state` (string)
    * `country` (string)
    * `postal_code` (string)
    * `phone` (string)
    * `custon_email` (string)
    * `create_at` (string)
    * `updated_at` (string)
    * `custom_inscricao_estadual` (string)
    * `custom_periodo_contrato_se_fidelidade_pos_pago` (string)
    * `contact` (`ContactDocSales`)

* **`ContactDocSales`**
    * `name` (string)
    * `profession` (decimal)
    * `phones` (List<string>)
    * `emails` (List<string>)

* **`ServiceDocSales`**
    * `ServiceId` (long)
    * `name` (string)
    * `description` (string)
    * `code` (string)
    * `obs` (string)
    * `quantity` (int)
    * `vigencia` (int)
    * `periodicity` (int)
    * `parcels_count` (int)
    * `value` (decimal)

* **`ParcelaDocSales`**
    * `value` (decimal)
    * `number` (int)
    * `formatted_date` (string): DD/MM/YYYY

### 6.2. Omie IntegMod

* **`ClienteOmie`**
    * `codigo_cliente_omie` (long)
    * `codigo_cliente_intg` (string)
    * `razao_social` (string): Razão Social (usar nome do cliente SIG)
    * `nome_fantasia` (string): Nome Fantasia (usar nome do cliente SIG)
    * `cnpj_cpf` (string): CPF/CNPJ (apenas dígitos)
    * `telefone1_ddd` (string): Mapeado do Contato/Telefone do ClienteSIG
    * `telefone1_numero` (string): Mapeado do Contato/Telefone do ClienteSIG
    * `contato` (string)
    * `endereco` (string): Mapeado do Endereço do ClienteSIG
    * `endereco_numero` (string)
    * `complemento` (string)
    * `bairro` (string): Mapeado do Endereço do ClienteSIG
    * `cidade` (string): Mapeado do Endereço do ClienteSIG
    * `estado` (string): UF (ex: SP, MG), mapeado do Endereço do ClienteSIG
    * `cep` (string)
    * `codigo_pais` (string): Apenas dígitos, mapeado do Endereço do ClienteSIG
    * `separar_endereco` (string): (S/N)
    * `pesquisar_cep` (string): (S/N)
    * `email` (string): Mapeado do Contato/Email do ClienteSIG
    * `inscricao_estadual` (string): Opcional para PJ. "ISENTO" se aplicável. Mapear do DocSales ou lógica.
    * `inscricao_municipal` (string)
    * `optante_simples_nacional` (string): (S/N)
    * `produtor_rural` (string): (S/N)
    * `contribuinte` (string): (S/N)
    * `observacao` (string)
    * `tags` (List<string>)
    * `valor_limite_credito` (decimal)
    * `bloquear_faturamento` (string): (S/N)
    * `inativo` (string): (S/N)

* **`ContaReceberOmie`**
    * `codigo_lancamento_omie` (long)
    * `codigo_lancamento_integ` (string)
    * `codigo_cliente` (long)
    * `codigo_cliente_integ` (string)
    * `data_vencimento` (string)
    * `valor_documento` (decimal)
    * `codigo_categoria` (string)
    * `data_previsao` (string)
    * `id_conta_corrente` (long)
    * `numero_documento` (string)
    * `codigo_tipo_documento` (string)
    * `observacao` (string)
    * `data_emissao` (string)
    * `id_origem` (string)
    * `operacao` (string)
    * `bloquear_exclusao` (string): (S/N)
    * `repeticao` (`RepeticaoOmie`)

* **`RepeticaoOmie`** (OBJETO CRUCIAL PARA A RECORRÊNCIA)
    * `antecipar` (string): (S/N)
    * `postegar` (string): (S/N)
    * `mensal` (`MensalOmie`)
    * `semanal` (`SemanalOmie`)
    * `especifico` (`EspecificoOmie`)

* **`MensalOmie`**
    * `repetir_todo_dia` (int)
    * `repetir_por` (int)

* **`SemanalOmie`**
    * `repetir_dia_semana` (string)
    * `repetir_por` (int)

* **`EspecificoOmie`**
    * `repetir_a_cada` (int)
    * `repetir_por` (int)

### 6.3. Next IntegMod

* **`AssinanteNext`**
    * `id_hie` (string)
    * `id_origem` (string)
    * `id_vinculo` (string)
    * `id_vendedor` (string)
    * `nome_fantasia` (string)
    * `razao_social` (string)
    * `cpf` (string)
    * `rg` (string)
    * `cep` (string)
    * `endereco` (string)
    * `complemento` (string)
    * `bairro` (string)
    * `cidade` (string)
    * `uf` (string)
    * `pais` (string)
    * `telefone` (string)
    * `ramal` (string)
    * `email` (string)
    * `contato` (string)
    * `tem_ramal` (string)
    * `tem_did` (string)
    * `tem_sms` (string)
    * `tem_escuta` (string)
    * `tem_ip_global` (string)
    * `tem_gravacao` (string)
    * `tem_sigame` (string)
    * `tem_fila` (string)
    * `tem_ura` (string)
    * `tem_portal` (string)
    * `tem_callingcard` (string)
    * `tem_grupo_captura` (string)
    * `tem_grupo_chamada` (string)
    * `tem_campanha_sms` (string)
    * `tem_campanha_voz` (string)
    * `tem_conferencia` (string)
    * `tem_provisionamento` (string)
    * `allow_change_callerid` (string)
    * `check_intra_network_call` (string)
    * `moeda` (string)
    * `status` (string)
    * `finance` (`FinanceNext`)
    * `user` (`UserNext`)

* **`FinanceNext`**
    * `tipo_tar` (string)
    * `id_plano` (string)
    * `limite_credito` (string)
    * `dia_vencimento` (string)
    * `dias_bloqueio` (string)
    * `alerta_status` (string)
    * `alerta_valor` (string)
    * `simultaneas` (string)
    * `ini_f` (string)
    * `inc_f` (string)
    * `tmp_f` (string)
    * `ini_c` (string)
    * `inc_c` (string)
    * `tmp_c` (string)
    * `ini_f_ddi` (string)
    * `inc_f_ddi` (string)
    * `tmp_f_ddi` (string)
    * `ini_c_ddi` (string)
    * `inc_c_ddi` (string)
    * `tmp_c_ddi` (string)
    * `tem_asr_anatel` (string)
    * `allow_loss_call` (string)
    * `id_perfil` (string)
    * `franquia_fixo_local` (string)
    * `franquia_movel_local` (string)
    * `franquia_fixo_ldn` (string)
    * `franquia_movel_ldn` (string)
    * `franquia_fixo_ddi` (string)
    * `franquia_movel_ddi` (string)
    * `disk_space` (string)
    * `call_only_local` (string)
    * `max_devices` (string)
    * `max_ivr` (string)
    * `max_queues` (string)

* **`UserNext`**
    * `username` (string)
    * `password` (string)

---

## 🔹 7. Estado Atual e Próximos Passos

O projeto encontra-se em **Planejamento Avançado / Design Detalhado**. O desenvolvimento será retomado a partir da avaliação e aprovação final do diagrama de arquitetura estrutural (em andamento) e da subsequente implementação do Domínio.

### 7.1. Avanços Chave:

* Modelagem e implementação dos Value Objects essenciais (`DocumentoFiscal`, `CEP`, `Endereco`, `Email`, `Telefone`) e suas Exceções de Domínio customizadas concluídas e refinadas.
* Alcançado alto nível de **consistência na validação, tratamento de exceções** (encapsulamento de `InnerExceptions`, mensagens coerentes) e aderência aos padrões de código e comentários (XML only), baseando-se na estrutura e nos construtores dos arquivos de referência de Telefone/InvalidTelefoneException.
* Estrutura multi-projeto definida seguindo modelo híbrido Camadas + Hexagonal.
* Início do registro de Decisões de Arquitetura (**ADRs**), incluindo um ADR documentando a estrutura de modelagem de contatos.
* Discussões ativas sobre a modelagem final de `ClienteSIG` e validações em `ContatoTelefonico` continuam.
* Decisão de priorizar a modelagem e implementação do Domínio, adiando a implementação da persistência (Banco de Dados) para uma etapa posterior.

### 7.2. Configuração do Ambiente:

* SDK .NET 8.0 instalado.
* Servidor MySQL configurado e em execução.
* Ambiente de desenvolvimento (Visual Studio ou VS Code) configurado.
* Variáveis de ambiente e strings de conexão ajustadas localmente.

### 7.3. Deploy e CI/CD (Status Atual)

* O pipeline de integração contínua será configurado após a entrega da Fase 1.
* Inicialmente, o deploy será realizado manualmente com base em versões estáveis do sistema.

---

## 🔹 8. Observações Finais

Esta Wiki é um documento vivo, mantido e evoluído conforme o projeto avança e novas necessidades ou informações técnicas são identificadas. O projeto está na **Fase 1/2**.

---