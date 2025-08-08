# 🏛️ 8. Documento e Diagrama de Arquitetura do Sistema - SIG VoIP

---

## Objetivo

Este documento consolida a arquitetura técnica do sistema SIG VoIP, detalhando seus componentes principais, tecnologias utilizadas, padrões de design, estrutura geral da solução e o fluxo de dados e interações com sistemas externos. Ele serve como o guia fundamental para o desenvolvimento, garantindo flexibilidade, alta testabilidade e facilitando a manutenção e a evolução do sistema ao longo do tempo.

---

## 1. Visão Geral da Arquitetura

O sistema SIG VoIP adotará uma **arquitetura híbrida**, combinando o modelo tradicional em camadas com a aplicação rigorosa dos princípios da **Clean Architecture** e **Arquitetura Hexagonal (Ports and Adapters)**. O objetivo principal é isolar o core da aplicação (Domínio e Lógica de Negócio) de detalhes de infraestrutura e tecnologia externas. A estrutura física será organizada em múltiplos projetos .NET, seguindo a **Regra de Dependência**.

Para uma representação visual de alto nível da arquitetura e seu contexto no ecossistema de sistemas, consulte o diagrama: [arquitetura-alto-nivel.png](/docs/arquitetura/visual/arquitetura-alto-nivel.png)

### 1.1. Análise e Avaliação do Diagrama de Arquitetura

O diagrama de alto nível ("arquitetura-alto-nivel.png") foi avaliado e aprovado, destacando-se por sua clareza e por reforçar os princípios arquiteturais do SIG VoIP.

* As setas tracejadas com rótulos como "Consome API", "Usa" e "Implementa" esclarecem a direção e a natureza das dependências entre as camadas, tornando o fluxo de comunicação visualmente intuitivo.
* A camada de Infraestrutura implementa interfaces definidas nas camadas Domínio e Aplicação, o que está em total conformidade com o princípio da Inversão de Dependência.
* A camada de Adaptadores / Apresentação corretamente consome as APIs expostas pela camada de Aplicação, servindo como o ponto de entrada externo do sistema.
* As dependências entre camadas superiores e inferiores estão corretamente indicadas, reforçando o fluxo esperado de uma arquitetura em camadas com core hexagonal.
* O layout vertical do diagrama contribui para a fácil compreensão da hierarquia e independência das camadas internas em relação às externas.

A adição dos rótulos nas setas melhora significativamente a clareza e comunicação da arquitetura, servindo como uma base sólida e aprovada para o desenvolvimento do backend em .NET do SIG VoIP.

---

## 2. Camadas da Arquitetura e Regra de Dependência

A arquitetura é claramente dividida em quatro camadas principais, organizadas do centro para fora, e aderindo estritamente à **Regra de Dependência** (dependências de código sempre apontam para as camadas mais internas).

### 2.1. Domínio (.NET 8 C#)

* **Localização:** É a camada mais interna e o coração do sistema.
* **Responsabilidade:** Contém a **lógica de negócio central** da aplicação, incluindo as regras de negócio mais importantes e as invariantes do sistema. A validação de invariantes de domínio é interna aos objetos de domínio, no construtor ou métodos, usando helpers estáticos internos do projeto.
* **Características:** É agnóstica a frameworks, bancos de dados, UIs ou quaisquer tecnologias de infraestrutura/aplicação externa. Define **interfaces (Portas)** para comunicação com o exterior (e.g., interfaces para Repositórios ou para Domain Services que precisam de recursos externos).
* **Conteúdo:**
    * **Entidades:** `ClienteSIG` (Aggregate Root), `ProcessamentoProposta` (Aggregate Root, entidade gerencial do fluxo de propostas do DocSales), `UsuarioSIG`, `AssinanteNext`, `ServidorNext`, `Contato`, `Funcionario`, `Departamento`.
    * **Value Objects:** `Documento`, `Endereco`, `Email`, `Telefone`, `ContatoTelefonico`, `NomePF`, `NomePJ`, `CEP`.
    * **Domain Services:** Serviços que orquestram a lógica de negócio que não pertence a uma única Entidade.
    * **Interfaces de Repositório:** (e.g., `IClienteSIGRepository`, `IProcessamentoPropostaRepository`, `IAssinanteNextRepository`, etc.) - Abstraem a persistência de dados.
    * **Interfaces para Domain Services:** (quando aplicável).
    * **Exceções de Domínio:** (e.g., `InvalidDocumentException`, `InvalidValueObjectException`, `InvalidCepException`, `InvalidAddressException`, `InvalidEmailException`, `InvalidTelefoneException`).
    * **Helpers Internos:** (e.g., `DomainValidationHelpers`).
    * **Enums de Domínio:** (e.g., `UsoTelefone`, `TipoTelefone`, `PerfilUsuario` - para `UsuarioSIG`).

### 2.2. Aplicação (.NET 8 C#)

* **Localização:** Envolve a camada de Domínio.
* **Responsabilidade:** Contém a **lógica de orquestração dos casos de uso** (Application Services, Use Cases). Coordena as interações entre as entidades/serviços do Domínio e as interfaces (Portas) de infraestrutura. Não contém lógica de negócio primária, apenas orquestração e validações de entrada/saída.
* **Características:** Depende do Domínio e define Interfaces (Portas) que a Infraestrutura implementa.
* **Conteúdo:**
    * **Application Services (Use Cases):** (e.g., `ProcessamentoPropostaAppService`, `ClienteAppService`) - Implementam os casos de uso do sistema.
    * **Casos de uso/comandos:** (e.g., `CriarClienteCommand`, `ProcessarPropostaCommand`).
    * **Interfaces para Serviços de Integração:** (e.g., `IDocSalesIntegrationService`, `IOmieIntegrationService`, `INextBillingIntegrationService`) - Abstraem a comunicação com sistemas externos, sendo implementadas pela camada de Infraestrutura.
    * **DTOs (Data Transfer Objects):** Para entrada e saída de dados da Aplicação, garantindo a tipagem segura e a validação no contexto dos casos de uso.

### 2.3. Infraestrutura (.NET 8 C#)

* **Localização:** Envolve a camada de Aplicação.
* **Responsabilidade:** **Implementa as interfaces** (Portas) definidas nas camadas mais internas (Domínio e Aplicação) para acessar recursos externos. É onde residem os detalhes de implementação de tecnologias específicas.
* **Características:** Depende das camadas mais internas (Aplicação, Domínio). É onde frameworks e tecnologias externas são utilizadas.
* **Conteúdo:**
    * **Repositórios:** Implementações concretas de ORMs (como Entity Framework Core com provedor Pomelo para MySQL) para acesso a banco de dados, implementando as interfaces de repositório definidas no Domínio.
    * **Contexto do Entity Framework:** (DbContext).
    * **Adaptadores de API:** Implementações concretas das interfaces de serviços de integração (e.g., `DocSalesApiAdapter`, `OmieApiAdapter`, `NextBillingApiAdapter`) para comunicar com sistemas externos via HTTP/APIs. Estes adaptadores são responsáveis pela tradução de dados entre os **Integration Models** (e.g., `ProposalDocSales`) e os objetos de domínio/aplicação, autenticação/autorização externa e tratamento de erros.
    * **Implementações de Logging:** (e.g., Serilog).
    * **Implementações de serviços de segurança:** (ASP.NET Core Identity, JWT). A entidade de persistência `UsuarioSigBd` (que herda de `IdentityUser`) é definida aqui e mapeia para a entidade de domínio `UsuarioSIG`.
    * **Implementações de Unit of Work.**
    * **Configuração de Injeção de Dependência:** Para componentes de Infraestrutura.
    * **Outros componentes de infraestrutura:** Caching, etc.

### 2.4. Adaptadores / Apresentação (.NET 8 C#)

* **Localização:** É a camada mais externa.
* **Responsabilidade:** Expõe a **API RESTful** da aplicação para o mundo externo (Interfaces de Usuário, outros sistemas). É responsável por receber requisições, converter os dados de entrada para o formato usado pela camada de Aplicação e formatar as respostas.
* **Características:** Depende das camadas internas (Aplicação). Atua como o ponto de entrada e saída do sistema. Configura o pipeline da aplicação (DI, middleware).
* **Conteúdo:**
    * **Controllers da API RESTful:** Gerenciam as requisições HTTP e invocam os Application Services.
    * **DTOs (Data Transfer Objects):** Para mapeamento de dados de entrada (requisições) e saída (respostas da API). Pode haver tráfego e possíveis alterações de informações provenientes dos **Integration Models** aqui, antes de serem enviadas para a camada de Aplicação.
    * **Middlewares:** Para tratamento de requisições, autenticação/autorização, tratamento de erros, etc.
    * **Configuração da Injeção de Dependência:** (ligando Aplicação e Infraestrutura).
    * **Configuração de autenticação/autorização:** (JWT).
    * **Configuração de Logging e Monitoring:** (e.g., Serilog hooks, Health Checks).

---

## 3. Componentes Principais e Tecnologias

O sistema SIG VoIP é composto pelos seguintes módulos e serviços, seguindo a estrutura arquitetural definida:

* **Backend:**
    * **Plataforma/Linguagem:** Construído em **.NET 8 (C#)**.
    * **ORM/Acesso a Dados:** Utiliza **Entity Framework Core**, com o provedor **Pomelo para MySQL**.
    * **Mapeamento de Objetos:** Emprega **AutoMapper** para transformação de objetos.
    * **Logging:** Utiliza **Serilog** para registro detalhado de eventos e monitoramento.
    * **Segurança:** Implementa autenticação e autorização utilizando **Microsoft Identity** e **JWT Bearer** para proteger os endpoints RESTful.
    * **Função:** Atua como o orquestrador central do sistema na fase inicial, contendo a lógica de negócio, gerenciando a persistência de dados e coordenando as integrações com os sistemas externos.

* **Frontend (planejado):**
    * **Tecnologia:** Desenvolvido com **Vue.js** e **TypeScript**.
    * **Função:** Interface de usuário para interação com o sistema, incluindo visualização e seleção de propostas, e gerenciamento de integrações.

* **Banco de Dados:**
    * **Tecnologia:** **MySQL** (relacional).
    * **Função:** Armazenamento persistente de todos os dados internos do SIG VoIP, incluindo `ClienteSIG`, `ProcessamentoProposta` e dados de autenticação (`UsuarioSigBd`).

* **Comunicação:**
    * **Protocolo:** Baseada em **API RESTful**, com troca de dados no formato **JSON**.
    * **Função:** Interligação entre o Frontend e o Backend, e também entre o Backend e as APIs externas.

* **Componentes de Integração:**
    * **APIs Externas:** Integração direta com **API DocSales**, **API Omie** e **API Next**.
    * **Função:** Manipulação de dados para os sistemas externos através de adaptadores.
    * **Componente de Logs Internos:** Faz parte da estrutura de observabilidade do backend.
    * **Configuração de Rede:** Previsão de um possível componente para **Configuração de Rede via Shell/API**.

---

## 4. Princípios Arquiteturais e Fluxo de Comunicação

A arquitetura do SIG VoIP é fortemente baseada em princípios e padrões que promovem a manutenibilidade, testabilidade e escalabilidade:

* **Arquitetura Híbrida em Camadas com Core Hexagonal (Ports and Adapters):** Complementa a Clean Architecture ao definir portas (interfaces) que a aplicação expõe e adaptadores que implementam essas portas para interagir com o mundo externo (banco de dados, APIs, UI). Isso garante que o domínio não seja acoplado a tecnologias externas.
* **Domain-Driven Design (DDD):** Foco na modelagem do domínio de negócio para criar um software que reflete a complexidade e a linguagem do negócio.
* **SOLID (ênfase em DIP - Dependency Inversion Principle):** Aplicação dos princípios SOLID para garantir um código mais flexível, extensível e fácil de manter, com especial atenção à inversão de dependências.
    * **Dependence Inversion Principle (DIP):** Camadas internas não dependem de camadas externas, mas de abstrações (interfaces) definidas nas camadas internas ou na Application.
* **Clean Architecture:** Organização do código em camadas concêntricas, onde as dependências apontam para o núcleo (core) da aplicação, isolando a lógica de negócio de detalhes de infraestrutura e frameworks.
* **Orientação de Dependências:** Consistente com a **Clean Architecture** e **Arquitetura Hexagonal**, todas as dependências da aplicação apontam para o núcleo (core) da aplicação, garantindo o isolamento da lógica de domínio.
* **Tratamento de Integrações:** As interações com sistemas externos como DocSales, Omie e Next são gerenciadas através de **adaptadores**, que traduzem as chamadas do domínio para o formato exigido pelas APIs externas, desacoplando o core da aplicação dessas dependências.
* **Orquestrador Central:** O **Backend** atua como o orquestrador central de todo o fluxo de integração, coordenando as interações entre os componentes internos e externos.

### 4.1. Fluxo de Requisição Típico

Este é o fluxo simplificado de uma requisição que entra no sistema:

1.  Uma requisição (e.g., HTTP GET/POST) chega à camada de **Adaptadores / Apresentação** (especificamente a um Controller da API RESTful).
2.  O **Controller** da API recebe a requisição, valida os DTOs de entrada e a encaminha para o **Application Service** apropriado na camada de **Aplicação**.
3.  O **Application Service** orquestra a lógica de negócio do caso de uso. Ele interage com as **Entidades** e **Value Objects** na camada de **Domínio**, geralmente através das **Interfaces de Repositório** (definidas no Domínio).
4.  Para acessar o banco de dados (persistência), a camada de **Infraestrutura** implementa as **Interfaces de Repositório** definidas no Domínio.
5.  Para integrar com sistemas externos, a camada de **Infraestrutura** também implementa as **Interfaces de Integração** (definidas na camada de Aplicação) para comunicar com as APIs externas (DocSales, Omie, Next).
6.  Após o processamento, a resposta é construída e segue o caminho inverso: da **Aplicação** para a camada de **Adaptadores / Apresentação**, que a formata (usando DTOs de saída) e a retorna ao cliente.

### 4.2. Autenticação e Autorização

* **Autenticação:** Gerenciada pelo ASP.NET Core Identity na camada de Infraestrutura. Usuários SIG VoIP (`UsuarioSIG`) são mapeados para entidades de persistência (`UsuarioSigBd`) e armazenados no banco de dados interno. Tokens JWT são gerados na camada de Apresentação/API após autenticação via Identity.
* **Autorização:** Implementada via Policies e Roles usando ASP.NET Core Authorization na camada de Apresentação/API, verificando claims do token JWT derivado dos Roles do `UsuarioSIG`.

---

## 5. Abordagem de Desenvolvimento e Entrega

O projeto prioriza a entrega pragmática da Fase 1, focando em funcionalidades essenciais para automação da integração DocSales → Omie / Next.

* Embora a Clean Architecture seja adotada, o desenvolvimento será funcionalidade-centrado e iterativo.
* Modelagens e implementações ocorrerão conforme necessidade das funcionalidades, iniciando pelas entidades e processos relacionados à integração.
* Autenticação e autorização serão implementadas inicialmente de forma simples, com expansão planejada para futuras fases.
* Refatoração será realizada conforme identificação de necessidade, não como etapa fixa pós-fase.

Essa abordagem visa balancear princípios arquiteturais com a entrega rápida de valor para o solicitante do projeto.

---

## 6. Visuais e Código-Fonte dos Diagramas de Arquitetura

Os diagramas de arquitetura correspondentes a esses cenários serão gerados usando PlantUML e Draw.io, e armazenados como imagens PNG na pasta `/docs/arquitetura/visual/`. Os arquivos-fonte serão mantidos nas pastas `/docs/arquitetura/plantuml/` e `/docs/fontes_diagramas/`.

* `08_diagrama_arquitetura.plantuml` (para diagramas mais técnicos de componentes)
* `08_diagrama_arquitetura.png` (imagem gerada do PlantUML)
* `arquitetura-alto-nivel.png` (diagrama de alto nível, fonte em `19_origem_diagramas.drawio`)
* `19_origem_diagramas.drawio` (arquivo fonte para o diagrama de alto nível)