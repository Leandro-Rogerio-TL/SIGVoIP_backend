# 🚧 9. Plano de Desenvolvimento - Sistema Integrado de Gerenciamento de Voz sobre IP (SIGVoIP)

---

## Objetivo

Este documento detalha o plano de desenvolvimento para o sistema SIGVoIP, abordando metodologia, princípios de qualidade, ferramentas, fases e a abordagem estratégica para a implementação. O objetivo é fornecer uma visão clara do caminho a ser seguido para a construção de um sistema robusto, manutenível e de alta performance, priorizando a entrega de valor de forma pragmática.

---

## 1. Metodologia de Desenvolvimento

O projeto SIGVoIP adota uma **Metodologia Ágil**, combinando elementos de **Scrum adaptado** e o framework **Kanban** para gerenciar o fluxo de trabalho. Esta abordagem visa proporcionar flexibilidade, rápida adaptação a mudanças e entregas incrementais e contínuas, mesmo em um contexto de desenvolvimento individual com sprints flexíveis. O desenvolvimento será **funcionalidade-centrado e iterativo**, focando nas necessidades mais imediatas do projeto para entregar valor em ciclos curtos.

---

## 2. Qualidade e Boas Práticas de Engenharia

O desenvolvimento do sistema SIGVoIP priorizará a criação de um **código de alto padrão** e a aderência rigorosa a princípios de engenharia de software comprovados, garantindo a sustentabilidade, manutenibilidade e a evolução do projeto:

* **Arquitetura Orientada ao Domínio (DDD - Domain-Driven Design):** Foco na modelagem do domínio de negócio para criar um software que reflete a complexidade e a linguagem do negócio, com uma modelagem rigorosa de Entidades, Value Objects e Aggregates.
* **Princípios SOLID:** Aplicação dos cinco princípios SOLID para garantir um código mais flexível, extensível e fácil de manter, com particular ênfase no **Princípio da Inversão de Dependência (DIP)**.
* **Disciplina de Escrita:** Adoção de práticas como **Object Calisthenics** e **Clean Code** para promover um código legível, coeso, com baixo acoplamento e fácil de entender.
* **Estilo Funcional (FP):** Priorizar a imutabilidade e o uso de funções puras quando apropriado, contribuindo para um código mais previsível e testável.
* **Padrões de Projeto:** Utilização de padrões de projeto (Design Patterns) apropriados para endereçar desafios comuns de software com soluções comprovadas.
* **Testes Automatizados:** Implementação abrangente de **testes unitários** e **testes de integração** para garantir a correção do comportamento do sistema e a validação das interações entre componentes.
* **CI/CD (Integração Contínua e Entrega Contínua):** Automação dos processos de build, teste e deployment para garantir entregas frequentes e confiáveis.
* **Refatoração Contínua:** Adoção de uma prática constante de refatoração para melhorar a estrutura interna do código sem alterar seu comportamento externo, gerenciando o débito técnico. A refatoração será realizada conforme a identificação de necessidade e oportunidade, e não como uma etapa fixa pós-fase.

---

## 3. Cronograma Previsto e Fases do Projeto

O projeto está estruturado em fases, com foco inicial no planejamento avançado, design detalhado e estabelecimento do núcleo do sistema. As datas são fictícias e servem como guia para a estruturação das fases, adaptando-se ao progresso real do desenvolvimento. A estratégia inicial focará nas funcionalidades essenciais que impulsionam a automação da integração DocSales → Omie/Next.

| Fase                     | Período Estimado      | Descrição da Fase                                                                                                                                                                             |
| :----------------------- | :-------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Levantamento** | Semana 1 - 2          | Compreensão aprofundada dos requisitos de negócio, processos e integrações necessárias.                                                                                                |
| **Modelagem e Arquitetura** | Semana 3 - 5          | Modelagem conceitual do domínio, design da arquitetura técnica, definição de tecnologias e padrões.                                                                                   |
| **Desenvolvimento Módulo 1** | Semana 6 - 10         | Codificação das funcionalidades e componentes da primeira fase, incluindo as integrações DocSales, Omie e Next.                                                                        |
| **Testes e Ajustes Módulo 1** | Semana 11 - 12        | Realização de testes unitários, de integração e ponta a ponta, além de ajustes e correções.                                                                                           |
| **Desenvolvimento Módulo 2** | Semana 13 - 17        | Codificação de funcionalidades adicionais e aprofundamento das integrações (e.g., painel administrativo avançado, novas APIs).                                                           |
| **Testes e Ajustes Módulo 2** | Semana 18 - 19        | Testes e refinamentos do Módulo 2.                                                                                                                                                   |
| **Implantação Inicial** | Semana 20             | Deployment da solução em ambiente de produção (fase beta/MVP).                                                                                                                         |
| **Otimização e Expansão**| A partir da Semana 21 | Monitoramento, otimizações de performance, melhorias contínuas, implementação de microsserviços/serverless quando justificado e expansão para novas funcionalidades. |

### 3.1. Backlog Detalhado

Este backlog lista as principais tarefas a serem executadas, agrupadas por macroetapa, incluindo tanto as prioridades iniciais quanto as etapas mais avançadas que farão parte do desenvolvimento contínuo. A modelagem de domínio e a configuração do ambiente estão em fase final/concluídas, mas são incluídas para completude. O início estratégico prioriza a configuração e persistência das entidades diretamente ligadas ao fluxo de integração, estabelecendo a base de dados fundamental.

**Fase 1: Estabelecimento do Núcleo e Integrações Iniciais (Prioridade Atual)**

1.  **Configuração do Ambiente de Desenvolvimento:**
    * Configurar ambientes de desenvolvimento local para o Backend (.NET 8).
    * Instalar o SDK .NET 8.0.404.
    * Configurar o MySQL local ou acesso a um ambiente de desenvolvimento.
    * Criar a estrutura inicial de pastas e projetos (.NET Solutions) para as camadas da arquitetura (Domínio, Aplicação, Infraestrutura, Apresentação).
    * Adicionar as referências de pacotes NuGet essenciais (EF Core, Pomelo, Identity, JWT, AutoMapper, Serilog).
    * Configurar o `.editorconfig` para padronização de código.
    * Configurar arquivos de settings (`.json`) para ambiente local e outros.
    * Configurar Dockerfile(s) para a aplicação backend e banco de dados (se aplicável).
    * Escrever scripts básicos para build e run (se necessário fora do IDE).
2.  **Modelagem Detalhada do Domínio (Refinamento Contínuo):**
    * Implementar as classes concretas para as Entidades (`ClienteSIG`, `ProcessamentoProposta` - entidade gerencial do fluxo de propostas DocSales no SIGVoIP, `UsuarioSIG`, `ServidorNext`, `Contato`, `Funcionario`, `Departamento`, `AssinanteNext`) e Value Objects (`Documento`, `Endereco`, `Email`, `Telefone`, `ContatoTelefonico`, `NomePF`, `NomePJ`, `CEP`) na camada de Domínio, com suas propriedades e validações de negócio encapsuladas.
    * Garantir a imutabilidade dos Value Objects.
    * Implementar a lógica de Aggregate Root em `ClienteSIG` (garantindo a consistência do agregado).
    * Refinar validações de Value Objects existentes (Documento, CEP, Endereço, Email, Telefone), garantindo o uso consistente de `Helpers de Validação` e o lançamento de `Exceções de Domínio` customizadas (e.g., `InvalidDocumentException`, `InvalidValueObjectException`, etc.), encapsulando `InnerExceptions` com mensagens coerentes.
    * Modelar as entidades `AssinanteNext` e `ServidorNext`, definindo que `AssinanteNext` se relaciona com um único `ServidorNext`, enquanto `ServidorNext` possui múltiplos IPs (principal e de reserva) e pode ter vários `AssinanteNext`s.
3.  **Avaliação Final e Aprovação do Diagrama de Arquitetura Estrutural:**
    * Revisar o diagrama de arquitetura existente (`arquitetura-alto-nivel.png`).
    * Garantir que ele reflita corretamente as camadas, a Regra de Dependência e os componentes principais.
4.  **Implementação da Camada de Infraestrutura (Persistência):**
    * Configurar o contexto do Entity Framework Core para acessar o banco de dados MySQL (utilizando Pomelo provider).
    * Definir as classes de mapeamento (Entity Configurations) para as Entidades do Domínio.
    * Configurar as Migrações do EF Core para o banco MySQL.
    * Implementar as classes concretas dos Repositórios (e.g., `ClienteSIGRepository`, `ProcessamentoPropostaRepository`) na camada de Infraestrutura, seguindo o padrão Repository.
    * Implementar Unit of Work (se necessário para controle transacional explícito).
    * Implementar a lógica inicial para criação do banco de dados e migrações (Code-First).
5.  **Implementação da Camada de Aplicação (Casos de Uso):**
    * Definir as interfaces dos Repositórios na camada de Domínio (e.g., `IClienteSIGRepository`, `IProcessamentoPropostaRepository`).
    * Implementar os Application Services (e.g., `ClienteAppService`, `ProcessamentoPropostaAppService`) para os casos de uso principais da Fase 1 (integração DocSales -> Omie/Next).
    * Implementar a lógica de orquestração dos casos de uso, utilizando as Entidades do Domínio e os Repositórios.
    * Definir os DTOs (Data Transfer Objects) para entrada e saída de dados da Aplicação.
    * Implementar o mapeamento entre Entidades e DTOs (AutoMapper).
6.  **Implementação dos Adaptadores de API (Infraestrutura):**
    * Implementar os Adaptadores para as APIs externas (DocSales, Omie, NextBilling/NextRouter), encapsulando a lógica de comunicação com essas APIs.
    * Tratar a autenticação, serialização/desserialização de dados e tratamento de erros de cada API.
    * Mapear os DTOs da Aplicação para os formatos de dados das APIs externas (Integration Models - e.g., `ProposalDocSales` para o modelo de integração da proposta).
7.  **Implementação da Camada de Apresentação (API RESTful):**
    * Criar os Controllers da API RESTful.
    * Definir os endpoints da API e seus verbos HTTP.
    * Implementar a lógica de autenticação (Identity) e autorização (JWT). Uma **segurança iterativa** será aplicada, com a autenticação inicial implementada de forma mais simples, e a evolução posterior para regras de autorização mais refinadas baseadas em perfis conforme a necessidade.
    * Configurar a injeção de dependência final no Startup (ou Program.cs no .NET 6+).
    * Configurar Middlewares (tratamento de erros, autenticação, autorização).
    * Configurar o Serilog para logging.
    * Implementar o tratamento de erros global na API.
    * Implementar Middleware na camada de Apresentação/API para capturar `Exceções de Domínio` e mapeá-las para respostas HTTP apropriadas (ex: HTTP 400 Bad Request para validações).
    * Garantir que as mensagens de erro retornadas ao cliente sejam informativas, mas seguras.
8.  **Implementação da Lógica de Negócio Específica (Domínio):**
    * Implementar as regras de negócio e validações nas Entidades e Value Objects do Domínio.
    * Implementar a lógica para seleção do servidor Next com base na capacidade (serviço de domínio).
    * Implementar a lógica para identificação e centralização de clientes por CPF/CNPJ.
9.  **Integração DocSales → Omie:**
    * Implementar a lógica para cadastrar/atualizar clientes no Omie.
    * Implementar a lógica para criar contas a receber recorrentes no Omie (utilizando a funcionalidade de recorrência da API Omie).
10. **Integração DocSales → NextBilling/Router:**
    * Implementar a lógica para cadastrar/atualizar clientes no NextBilling/Router.
    * Implementar a lógica para configurar serviços no NextBilling/Router.
    * Implementar a lógica para seleção do servidor Next com base na capacidade.
11. **Implementação do Painel Administrativo Básico (Backend):**
    * Criar os endpoints da API para suportar a visualização de status e o gerenciamento das integrações.
    * Implementar a lógica para listar documentos do DocSales, selecionar documentos para processamento e exibir o status de processamento.
    * Implementar a lógica para retentativas manuais de integrações.
    * Estudar e iniciar integração de rede via Shell/API, com uma **análise de escopo detalhada** para determinar a melhor forma de integração e execução.
12. **Configuração e Implementação de Logging (Serilog):**
    * Configurar o Serilog na aplicação.
    * Configurar sinks (Console, File, etc.).
    * Configurar context properties para logs estruturados.

**Fases Posteriores e Refinamentos Contínuos (Backlog de Evolução)**

13. **Segurança Refinada e Autorização Baseada em Perfis:**
    * Desenvolver um sistema de controle de acesso baseado em funções (Role-Based Access Control - RBAC).
    * Implementar políticas de autorização mais granulares.
    * Gerenciar permissões para diferentes tipos de usuários e operações.
14. **Filas de Mensagens e Processamento Assíncrono:**
    * Introduzir um sistema de mensageria (RabbitMQ, Kafka, Azure Service Bus ou Amazon SQS) para desacoplar e garantir a entrega de mensagens entre serviços.
    * Implementar processamento assíncrono para operações de longa duração (e.g., integração de propostas, sincronização de dados).
    * Utilizar frameworks de mensageria como MassTransit ou NServiceBus para abstrair a complexidade.
15. **Event Sourcing e CQRS (Command Query Responsibility Segregation):**
    * Avaliar a implementação de Event Sourcing para manter um histórico completo das mudanças de estado do domínio.
    * Separar os modelos de leitura e escrita (CQRS) para otimizar a performance e a escalabilidade, especialmente para consultas complexas e painéis.
16. **Monitoramento e Observabilidade Avançada:**
    * Integrar ferramentas de monitoramento (Prometheus, Grafana, Application Insights) para coletar métricas de performance, erros e uso.
    * Implementar Distributed Tracing para acompanhar o fluxo de requisições através de múltiplos serviços.
    * Configurar Health Checks detalhados para os serviços.
17. **Cache Distribuído:**
    * Implementar soluções de cache distribuído (e.g., Redis) para otimizar a performance de leituras frequentes e reduzir a carga sobre o banco de dados.
18. **Implementação de Testes Unitários Abrangentes:**
    * Escrever testes unitários para as classes da camada de Domínio (Value Objects, Entidades, Helpers) isolando dependências com mocks (Moq/NSubstitute).
    * Escrever testes unitários para a lógica dos Application Services/Use Cases (isolando dependências).
    * Garantir alta cobertura de testes para a lógica de negócio central.
19. **Implementação de Testes de Integração Robustos:**
    * Escrever testes de integração para verificar a interação entre as camadas (Aplicação -> Infraestrutura).
    * Testar a comunicação com o banco de dados (EF Core) usando um banco de dados de teste in-memory ou contêinerizado.
    * Testar a comunicação com os Adaptadores de API (usando serviços externos mockados ou ambientes de teste).
20. **Implementação de Testes End-to-End (Backend):**
    * Escrever testes que exercitam o fluxo completo de um caso de uso através da API (camada de Apresentação -> Aplicação -> Domínio -> Infraestrutura).
    * Utilizar frameworks de teste de integração para APIs (WebApplicationFactory).
21. **Configuração do Pipeline de CI/CD Completo:**
    * Configurar o pipeline no Azure DevOps, GitHub Actions ou GitLab CI.
    * Automatizar o build, restauração de pacotes e execução dos testes (unitários, integração, E2E backend).
    * Configurar a geração de artefatos de deployment (Docker images).
22. **Configuração de Deployment Automatizado:**
    * Definir e configurar o ambiente de staging/produção (Azure App Services, Kubernetes, etc.).
    * Automatizar o deployment dos artefatos gerados pelo CI para os ambientes de destino.
23. **Implementação de Padrões de Resiliência e Tolerância a Falhas (Polly):**
    * Avaliar o uso de padrões como Circuit Breaker, Retry, Timeout para chamadas a APIs externas (DocSales, Omie, Next).
    * Considerar bibliotecas como Polly para implementar esses padrões na camada de Infraestrutura.
24. **Implementação de Testes de Contrato (Pact):**
    * Implementar testes de contrato para garantir a compatibilidade entre a API e os clientes (Pact ou similar).
    * Automatizar a execução dos testes de contrato no pipeline de CI/CD.
25. **Implementação de Arquitetura de Microsserviços (Avaliação e Evolução):**
    * Avaliar a viabilidade e necessidade de adotar uma arquitetura de microsserviços para domínios específicos.
    * Decompor a aplicação em microsserviços com base nos domínios de negócio isolados.
    * Implementar a comunicação entre os microsserviços (e.g., API RESTful, gRPC, filas de mensagens).
    * Implementar mecanismos de service discovery e configuração distribuída.
    * Implementar padrões de resiliência e tolerância a falhas em microsserviços (e.g., circuit breaker, retry, bulkhead).
26. **Implementação de Serverless (Avaliação e Otimização):**
    * Avaliar a viabilidade de utilizar tecnologias serverless (e.g., Azure Functions, AWS Lambda) para funcionalidades específicas e escaláveis.
    * Implementar partes da aplicação como funções serverless.
    * Configurar triggers e bindings para as funções serverless.
    * Implementar mecanismos de monitoramento e logging para as funções serverless.
27. **Configuração de Gestão de Segredos:**
    * Implementar uma solução para gestão segura de segredos (e.g., Azure Key Vault, HashiCorp Vault) para credenciais de API, chaves de banco de dados, etc.
28. **Otimização de Performance em Banco de Dados:**
    * Analisar e otimizar queries complexas no MySQL.
    * Criar índices adequados para melhoria de performance.
    * Monitorar e ajustar a configuração do banco de dados.
29. **Versionamento de API:**
    * Implementar estratégias de versionamento de API para garantir compatibilidade retroativa (e.g., via URL, Header, Query String).
30. **Documentação de API (Swagger/OpenAPI):**
    * Gerar automaticamente documentação da API RESTful usando Swagger/OpenAPI.
    * Manter a documentação atualizada e interativa para facilitar o consumo.
31. **Segurança Avançada (OWASP Top 10):**
    * Revisar e aplicar as melhores práticas de segurança baseadas no OWASP Top 10 para APIs e aplicações web.
    * Implementar proteção contra injeção, XSS, CSRF, etc.
32. **Auditoria e Logs de Segurança:**
    * Implementar logs de auditoria detalhados para ações críticas do usuário e eventos de segurança.
    * Integrar com soluções de SIEM (Security Information and Event Management) se necessário.
33. **Tratamento de Exceções Centralizado e Robusto:**
    * Refinar o tratamento de exceções para garantir que todas as exceções não tratadas sejam logadas e apresentadas ao usuário de forma amigável e segura.
    * Mapear exceções para códigos de status HTTP apropriados.
34. **Internacionalização (i18n) e Localização (l10n) - Se Aplicável:**
    * Implementar suporte para múltiplos idiomas e culturas (mensagens, datas, moedas).
35. **Automação de Testes de Carga e Performance:**
    * Desenvolver scripts e configurar ferramentas para simular carga no sistema e identificar gargalos de performance.
36. **Melhoria Contínua do DevOps:**
    * Automatizar mais etapas do pipeline de CI/CD.
    * Implementar automação de infraestrutura (Infrastructure as Code - IaC) com ferramentas como Terraform ou ARM Templates.
37. **Implementação de Webhooks/Callbacks (Para DocSales/Outros):**
    * Configurar a aplicação para receber callbacks de sistemas externos (e.g., DocSales para atualização de status de propostas).
38. **Otimização de Mensagens de Erro (Detalhes para Desenvolvimento, Genéricas para Produção):**
    * Diferenciar as mensagens de erro retornadas em ambiente de desenvolvimento (detalhadas) e produção (genéricas e seguras).
39. **Estratégias de Cache no Cliente (Frontend - Futuro):**
    * Considerar estratégias de cache para o frontend para reduzir chamadas desnecessárias à API.
40. **Modelagem e Gerenciamento de Configurações Dinâmicas:**
    * Utilizar o sistema de configuração do .NET (Options Pattern) e considerar o uso de um serviço de configuração centralizado (e.g., Azure App Configuration) para configurações dinâmicas em runtime.
41. **Refatoração Contínua e Melhorias de Código:**
    * Revisar o código regularmente para identificar oportunidades de refatoração e melhoria de design, combatendo o débito técnico.

---

## 4. Papéis e Responsabilidades

* **Leandro Rogerio** – Responsável por toda a concepção, implementação e documentação do sistema. Atualmente, Leandro é o único desenvolvedor envolvido no projeto SIGVoIP.

---

## 5. Ferramentas e Tecnologias Planejadas

### Backend
* .NET 8 (C#)
* Entity Framework Core (MySQL Provider: Pomelo)
* Microsoft Identity
* JWT Bearer
* AutoMapper
* Serilog
* (Potenciais) MassTransit/NServiceBus (para mensageria), Polly (para resiliência), Redis (para cache)

### Frontend (Desenvolvimento Futuro)
* TypeScript
* Vue.js

### Banco de Dados
* MySQL relacional

### Comunicação
* API RESTful (JSON)

### Ferramentas
* Visual Studio Code / Visual Studio
* Terminal (.NET CLI)
* MySQL Workbench / DBeaver
* Postman / Insomnia (para testar APIs)
* Draw.io (para diagramas)
* Markdown (para documentação)
* `dotnet format` (formatação de código automatizada)
* Ferramentas de Teste (.NET Test Explorer, xUnit/NUnit/MSTest)
* Git
* (Potenciais) Docker, Kubernetes (para conteinerização e orquestração), Azure DevOps / GitHub Actions / GitLab CI (para CI/CD), Prometheus / Grafana / Application Insights (para monitoramento)