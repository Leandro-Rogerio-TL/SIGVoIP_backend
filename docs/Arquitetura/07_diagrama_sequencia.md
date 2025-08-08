# 🔁 7. Diagrama de Sequência - Processamento e Integração de Proposta DocSales

---

## Objetivo

Este documento detalha as interações dinâmicas entre os componentes do Sistema SIGVoIP e as APIs externas (DocSales, Omie, Next) através de Diagramas de Sequência. Ele especifica o "como" as operações de processamento de propostas aprovadas são realizadas, incluindo tratamento de erros e garantia de idempotência, complementando o Caso de Uso principal de "Processar Propostas Aprovadas e Integrar com Sistemas Externos".

---

## 1. Participantes (Lifelines)

Os principais atores e sistemas envolvidos no fluxo de integração são:

* **Usuário Interno (Administrador/Financeiro):** Representa o papel do usuário que interage com o SIG VoIP para iniciar, revisar e monitorar o processo.
* **Sistema SIG VoIP:** O sistema central responsável por orquestrar a integração, gerenciar dados locais, a lógica de negócio e interagir com as APIs externas.
    * **Módulos Internos do SIG VoIP:**
        * **Orquestração:** Coordena o fluxo geral.
        * **Integração Omie:** Gerencia a comunicação com a API Omie.
        * **Integração Next:** Gerencia a comunicação com a API Next.
        * **Persistência:** Responsável por operações no Banco de Dados Local.
        * **Logging:** Componente para registro detalhado de eventos.
* **API DocSales:** A interface para consulta de propostas aprovadas e fonte das propostas.
* **API Omie:** A interface para operações de cliente e lançamento de contas a receber.
* **API Next (ou NextBilling/NextRouter):** A interface para operações de cliente VoIP e configuração de serviços.
* **Banco de Dados Local:** Onde o SIG VoIP armazena e consulta seus dados internos, como `ClienteSIG`, `AssinanteNext` e o status de propostas já processadas.
* **Logs Internos:** Onde o SIG VoIP registra o status e detalhes das operações, servindo como componente para registro detalhado de eventos.

---

## 2. Fluxo Principal e Cenários Detalhados de Processamento de Proposta

A seguir, a sequência de interações para o processamento de uma proposta aprovada do DocSales, com detalhamento de cenários-chave:

### 2.1. Início do Processamento: Consulta e Seleção de Propostas

1.  **Consulta de Propostas Aprovadas (Periódica ou por Demanda):**
    * **[Sistema SIG VoIP]** consulta a **[API DocSales]** com a requisição: `GET /proposals?status=approved`.
    * **[API DocSales]** retorna as propostas aprovadas para o **[Sistema SIG VoIP]**.
    * **[Sistema SIG VoIP]** filtra os documentos retornados, verificando no **[Banco de Dados Local]** quais propostas já foram processadas anteriormente (`ProcessamentoProposta`).

2.  **Interação com o Usuário:**
    * **[Sistema SIG VoIP]** exibe a lista de propostas aprovadas não processadas para o **[Usuário Interno]**.
    * **[Usuário Interno]** seleciona uma ou mais propostas para integração, enviando a seleção para o **[Sistema SIG VoIP]**.

### 2.2. Processamento Individual da Proposta Selecionada (Loop para cada documento)

Para cada documento selecionado, o **[Sistema SIG VoIP]** executa o seguinte fluxo:

1.  **Extração e Verificação do Cliente SIG:**
    * **[Sistema SIG VoIP]** extrai o CPF/CNPJ da proposta.
    * **[Sistema SIG VoIP]** busca o `ClienteSIG` correspondente no **[Banco de Dados Local]**.
    * **[Banco de Dados Local]** retorna o `ClienteSIG` (ou nulo) para o **[Sistema SIG VoIP]**.
    * **[Sistema SIG VoIP]** Se o `ClienteSIG` não existir, um novo é criado no **[Banco de Dados Local]**.

2.  **Verificação e Criação/Atualização de AssinanteNext:**
    * **[Sistema SIG VoIP]** verifica se já existe um `AssinanteNext` e seus dados de integração para os produtos da proposta associados ao `ClienteSIG`. Se não existir, um novo `AssinanteNext` é criado.

3.  **Integração com Sistemas Externos:**

    * **Integração com Next:**
        * **[Sistema SIG VoIP - Módulo Integração Next]** interage com a **[API Next]** (endpoint `/manageCustomers`) para criar ou editar o cliente e associar os serviços VoIP.
        * O **[Sistema SIG VoIP]** pode sugerir automaticamente o melhor `ServidorNext` com base na capacidade (`RN15`), informando à **[API Next]** (Opcional).
        * **[API Next]** retorna confirmação/IDs para o **[Sistema SIG VoIP]**.
        * O **[Sistema SIG VoIP]** garante que o sufixo seja adicionado ao nome do assinante Next (`RN04`).

    * **Integração com Omie:**
        * **[Sistema SIG VoIP - Módulo Integração Omie]** interage com a **[API Omie]** para criar ou atualizar o cliente (`/clientes/incluir` ou `/alterar`).
        * **[API Omie]** retorna confirmação/IDs para o **[Sistema SIG VoIP]**.
        * **[Sistema SIG VoIP - Módulo Integração Omie]** também interage com a **[API Omie]** (`/contacorrente/lancamentos/incluirconta`) para criar contas a receber, utilizando o recurso `repeticao` para configurar a recorrência.
        * **[API Omie]** retorna confirmação/IDs para o **[Sistema SIG VoIP]**.

4.  **Armazenamento de IDs Externos:**
    * **[Sistema SIG VoIP - Persistência]** armazena os IDs externos (retornados pelas APIs Omie e Next) como atributos nas entidades `ClienteSIG` e `AssinanteNext` no **[Banco de Dados Local]**.

5.  **Registro de Logs e Atualização de Status:**
    * **[Sistema SIG VoIP - Logging]** registra o sucesso ou erro da operação nos **[Logs Internos]** (`RF17`, `RNF03`, `RNF12`).
    * **[Sistema SIG VoIP - Orquestração]** atualiza o status da proposta (`ProcessamentoProposta`) na interface do usuário e no **[Banco de Dados Local]**.

### 2.3. Monitoramento e Reprocessamento (Cenários Adicionais)

* **Monitoramento:** **[Usuário Interno]** pode visualizar o status das integrações no **[Sistema SIG VoIP]** através de um painel de logs.
* **Tratamento de Falha e Reprocessamento:**
    * Em caso de erro na integração (Omie ou Next), **[Sistema SIG VoIP - Orquestração]** registra o erro e aciona a lógica de **retentativa com backoff exponencial** (`RNF07`, `RNF08`).
    * Se as retentativas esgotadas, a proposta é marcada com status de falha parcial e registra pendência.
    * **[Usuário Interno]** pode iniciar o reprocessamento de propostas que apresentaram falha, interagindo com o **[Sistema SIG VoIP]**.
    * Para garantir **idempotência** (`RNF05`), o **[Sistema SIG VoIP]** verifica o status anterior e os dados já enviados, retomando a integração do ponto de falha ou refazendo apenas as operações pendentes (ex: verificando se o cliente/serviço já existe nas APIs externas antes de tentar criar).

---

## 3. Visuais dos Diagramas de Sequência

Os diagramas de sequência correspondentes a esses cenários serão gerados usando PlantUML e armazenados como imagens PNG na pasta `/docs/arquitetura/visual/`.

* `07_diagrama_sequencia_processamento_sucesso.plantuml`
* `07_diagrama_sequencia_processamento_sucesso.png`
* `07_diagrama_sequencia_reprocessamento_falha.plantuml`
* `07_diagrama_sequencia_reprocessamento_falha.png`
* `07_diagrama_sequencia_integracao_omie.plantuml`
* `07_diagrama_sequencia_integracao_omie.png`
* `07_diagrama_sequencia_integracao_next.plantuml`
* `07_diagrama_sequencia_integracao_next.png`