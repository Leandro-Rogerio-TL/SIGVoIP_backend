# 🧪 10. Plano de Testes - SIGVoIP (Fase 1)

## Objetivo
Este documento define a estratégia e os casos de teste para a Fase 1 do projeto SIGVoIP, focando na integração DocSales → Omie e Next, visando garantir a qualidade e a funcionalidade do sistema.

---

## 1. Tipos de Teste

A estratégia de testes será abrangente e incluirá diferentes níveis para assegurar a qualidade do software:

-   **Testes Unitários**: Focados na validação de funções e métodos individuais, especialmente aqueles responsáveis pela transformação e validação de dados nas camadas de Domínio e Aplicação. Isso garante que os componentes menores funcionem conforme o esperado em isolamento.
-   **Testes de Integração**: Verificação da comunicação entre os componentes internos do SIGVoIP e, crucialmente, as chamadas para as **APIs externas (DocSales, Omie, Next)**. Podem ser realizados com chamadas reais às APIs ou com o uso de mocks/simulações quando apropriado.
-   **Testes Funcionais**: Validação dos fluxos principais via API RESTful, visando simular o **fluxo completo de negócio**, desde a consulta de propostas DocSales até a integração final com Omie e Next, utilizando propostas de teste para validar o comportamento do sistema de ponta a ponta.
-   **Testes de Interface**: Aplicáveis à futura interface web para edição e gerenciamento de dados antes do envio para os sistemas externos.
-   **Testes de Autenticação e Autorização**: Garantia de acesso controlado conforme perfil do usuário, focados em verificar as funcionalidades de autenticação (Identity + JWT) e autorização por perfil, garantindo que apenas usuários com as permissões corretas possam acessar determinadas funcionalidades.
-   **Testes de Validação de Regras de Negócio**: Cobertura de regras específicas de consistência e comportamento esperados nas operações do sistema.

---

## 2. Estratégia de Testes

-   Testar módulos individualmente por meio de testes unitários.
-   Validar a integração com APIs externas utilizando ambientes de homologação.
-   Simular cenários com dados fictícios durante a fase inicial de testes.
-   Verificar os principais fluxos de negócio, como a seleção e o processamento de propostas.
-   Garantir idempotência nas operações, evitando duplicações em múltiplas execuções.
-   Registrar erros e logs estruturados, com persistência no banco de dados para rastreabilidade.
-   Cobrir autenticação via Identity e proteção de endpoints com base nas permissões do usuário.
-   Preparar o ambiente de interface para testes funcionais em etapas posteriores.

---

## 3. Ambiente de Testes

Os testes serão executados em ambientes específicos para garantir a isolamento e a validade dos resultados:

-   **Ambiente de Desenvolvimento Local**: Utilizado para **Testes Unitários** e grande parte dos **Testes de Integração** com banco de dados local (MySQL configurado) e uso de mocks/simulações para APIs externas quando necessário.
-   **Ambiente de Homologação/Sandbox**: Para **Testes de Integração** e **Testes Funcionais**, serão utilizados ambientes de sandbox ou homologação fornecidos pelas APIs externas (DocSales, Omie, Next) para validação da comunicação real.
-   **Dados de Teste**: Serão criados e utilizados **dados fictícios e isolados** para os testes, garantindo que as operações não impactem dados de produção ou reais.
-   **Ferramentas de Suporte**: O uso de Docker pode ser considerado para o ambiente de banco de dados e a própria aplicação, facilitando a consistência e a replicação do ambiente de testes.

---

## 4. Casos de Teste

Os casos de teste detalham as entradas esperadas e os resultados para cada funcionalidade principal:

### 4.1. Consulta de Propostas Aprovadas

-   **Descrição:** Verificar se o sistema consulta, filtra e exibe corretamente as propostas aprovadas do DocSales.
-   **Entrada:** API do DocSales contendo propostas com status "Aprovado", algumas já processadas e outras não.
-   **Passos:**
    1.  Acionar a consulta de propostas no SIGVoIP.
    2.  Verificar a listagem exibida ao usuário.
-   **Resultado Esperado:** A listagem deve exibir corretamente apenas as propostas com status "Aprovado" que ainda não foram processadas pelo sistema.
-   Testar o tratamento de falhas na comunicação com a API DocSales.

### 4.2. Seleção e Processamento de Propostas

-   **Descrição:** Testar a capacidade do usuário de selecionar propostas para processamento. Validar a seleção múltipla de propostas para processamento.
-   **Entrada:** Lista de propostas exibida na interface do usuário.
-   **Passos:**
    1.  Selecionar individualmente uma proposta na interface.
    2.  Seleção múltipla de propostas para processamento.
    3.  Confirmar a criação e atualização correta dos registros no Omie e Next.
    4.  Garantir que o processo seja idempotente.
-   **Resultado Esperado:** As propostas selecionadas devem ser marcadas corretamente para integração, e o sistema deve exibir o status de seleção por item.

### 4.3. Registro e Vinculação de ClienteSIG

-   **Descrição:** Validar a identificação, criação e vinculação do `ClienteSIG` e suas entidades de integração.
-   **Entrada:** Dados de CPF/CNPJ de propostas (alguns já existentes no BD local, outros novos).
-   **Passos:**
    1.  Processar uma proposta com CPF/CNPJ inexistente no `ClienteSIG` local.
    2.  Processar uma proposta com CPF/CNPJ já existente no `ClienteSIG` local.
-   **Resultado Esperado:**
    * Para CPF/CNPJ novo: Um novo registro de `ClienteSIG` deve ser criado e associado corretamente.
    * Para CPF/CNPJ existente: O `ClienteSIG` existente deve ser identificado corretamente, e as entidades de integração devem ser vinculadas ou atualizadas.

### 4.4. Integração com Sistemas Externos

-   **Descrição:** Abrange a verificação de criação/atualização de clientes e o lançamento de contas recorrentes no Omie, e a criação/atualização de clientes e a configuração de serviços VoIP no Next. Detectar clientes existentes em Omie e Next com base no CPF/CNPJ.
-   **Entrada:** Dados do cliente e proposta selecionada para integração.
-   **Passos:**
    1.  Processar uma proposta que exija a criação de um novo cliente no Omie.
    2.  Processar uma proposta que exija a atualização de um cliente existente no Omie.
    3.  Verificar o lançamento da conta recorrente no Omie.
    4.  Processar uma proposta que exija a criação de um novo cliente no Next.
    5.  Processar uma proposta que exija a atualização de um cliente existente no Next.
    6.  Verificar a configuração dos serviços VoIP associados.
    7.  Criar corretamente a entidade `ClienteSIG` com os identificadores externos.
    8.  Criar clientes e serviços no Next.
    9.  Criar contas a receber recorrentes no Omie.
    10. Orquestrar todo o fluxo de integração sem falhas.
-   **Resultado Esperado:** O cliente deve ser criado ou atualizado corretamente no Omie, e a conta recorrente deve ser lançada com a configuração de repetição adequada. O cliente deve ser criado ou atualizado corretamente no Next via `/manageCustomers`, e os serviços VoIP devem ser configurados conforme a proposta.

### 4.5. Monitoramento e Logs

-   **Descrição:** Verificar o registro de logs e o tratamento de falhas durante o processamento. Gerar logs estruturados em todos os cenários de integração (sucesso e falha).
-   **Entrada:** Processamento de propostas (cenários de sucesso e simulação de erros em APIs externas).
-   **Passos:**
    1.  Executar um fluxo de integração bem-sucedido.
    2.  Simular uma falha na API do DocSales (ex: indisponibilidade).
    3.  Simular uma falha na API do Omie (ex: dados inválidos).
    4.  Simular uma falha na API do Next (ex: erro de autenticação).
    5.  Confirmar que os logs são persistidos no banco de dados.
    6.  Validar exibição e filtragem dos registros de status e eventos na interface.
-   **Resultado Esperado:**
    * Para sucesso: Logs devem ser salvos com status de sucesso e timestamps.
    * Para falha: Logs devem ser salvos com status de erro, detalhes do erro e timestamps, indicando a etapa da falha e o estado do documento.

### 4.6. Reprocessamento Manual

-   **Descrição:** Testar a funcionalidade de reprocessamento para propostas que falharam anteriormente.
-   **Entrada:** Proposta que teve um erro registrado em uma tentativa anterior.
-   **Passos:**
    1.  Selecionar uma proposta com status de falha no painel administrativo.
    2.  Acionar a opção de reprocessamento manual.
-   **Resultado Esperado:** O sistema deve tentar novamente o processamento completo da proposta, e o status do documento deve ser atualizado.

### 4.7. Escolha Automática de Servidor Next

-   **Descrição:** Validar a lógica de seleção automática do melhor servidor Next.
-   **Entrada:** Proposta que requer alocação de servidor, com dados de capacidade de diferentes `ServidorNext` disponíveis.
-   **Passos:**
    1.  Processar uma proposta que necessite de um `ServidorNext`.
    2.  Alterar os dados de capacidade dos servidores simulados para testar diferentes cenários de escolha.
-   **Resultado Esperado:** O sistema deve selecionar automaticamente o servidor com base na lógica de otimização (ex: mais espaço disponível).

### 4.8. Autenticação e Autorização

-   **Descrição:** Garantir acesso controlado conforme perfil do usuário.
-   **Entrada:** Credenciais de usuário válidas e inválidas, acesso a endpoints protegidos.
-   **Passos:**
    1.  Verificar login com credenciais válidas.
    2.  Rejeitar acesso com credenciais inválidas.
    3.  Validar a geração e validade de tokens JWT.
    4.  Garantir que endpoints estejam protegidos por roles.
-   **Resultado Esperado:** Usuários com credenciais válidas devem acessar as funcionalidades permitidas por seu perfil, e o acesso deve ser negado para credenciais inválidas ou para funcionalidades sem permissão.

---

## 5. Ferramentas de Teste

-   **xUnit**: Ferramenta principal para testes automatizados em .NET 8.
-   **NUnit**: Alternativa complementar para testes unitários e de integração.
-   **Postman / Newman**: Para testes manuais e automatizados de APIs RESTful.
-   **Serilog**: Para geração de logs estruturados e rastreamento de falhas.
-   **Scripts SQL**: Para validação e auditoria dos dados persistidos durante os testes.

---

Este plano de testes visa cobrir as funcionalidades essenciais da Fase 1 do SIGVoIP.