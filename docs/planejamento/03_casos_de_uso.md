# 🎭 Casos de Uso - SIGVoIP

## Objetivo
Este documento detalha os principais casos de uso relacionados ao processamento de propostas aprovadas do DocSales e sua integração com os sistemas Omie e Next, focando nas interações entre os atores e o sistema SIGVoIP para a Etapa 1 do projeto.

---

## 1. Atores

Os participantes que interagem com o sistema ou são parte integrante dos fluxos são:

* **Usuário Interno (Administrador/Financeiro):** Representa os usuários humanos que interagem diretamente com a interface do SIGVoIP para iniciar, revisar e gerenciar o processamento das propostas aprovadas, bem como tratar eventuais falhas.
* **Sistema SIGVoIP:** O core da solução, responsável por orquestrar toda a integração, validar dados, gerenciar o fluxo de processamento e interagir com as APIs externas e o banco de dados local.
* **API DocSales:** Sistema externo que atua como fonte de dados para as propostas aprovadas.
* **API Omie:** Sistema externo de gestão financeira (ERP) que atua como destino para dados de clientes e contas a receber.
* **API Next (NextBilling/NextRouter):** Sistema externo de gestão técnica de telecomunicações que atua como destino para dados de clientes e configurações de serviço VoIP/Call Center.

---

## 2. Casos de Uso Principais – Etapa 1

Os casos de uso a seguir descrevem as funcionalidades essenciais do sistema SIGVoIP para a Etapa 1, com foco no fluxo de processamento de propostas DocSales.

### Caso de Uso 01 – Consultar e Listar Propostas DocSales Aprovadas

* **Atores:** Usuário Interno, Sistema SIGVoIP, API DocSales
* **Descrição:** O sistema consulta a API do DocSales para obter propostas com status "Aprovado" que ainda não foram processadas pelo SIGVoIP e as exibe para o usuário em uma interface organizada.
* **Pré-condições:** N/A
* **Pós-condições:** Lista de propostas aprovadas e não processadas exibida na interface do SIGVoIP.
* **Fluxo Principal:**
    1.  O **Sistema SIGVoIP** (automaticamente ou por gatilho do usuário) consulta a **API DocSales** para recuperar propostas aprovadas.
    2.  O **Sistema SIGVoIP** filtra as propostas recebidas, excluindo aquelas já processadas ou com outro status que não seja "Aprovado" no Banco de Dados Local.
    3.  O **Sistema SIGVoIP** exibe a lista filtrada de propostas para o **Usuário Interno**.
* **Fluxos Alternativos:**
    * **A.1. Falha na Comunicação com DocSales ou Ausência de Propostas:**
        1.  Se o **Sistema SIGVoIP** não conseguir se comunicar com a **API DocSales** ou se nenhuma proposta "Aprovada" e não processada for encontrada, ele registra o evento nos logs.
        2.  O **Sistema SIGVoIP** exibe uma mensagem informativa na interface (ex: "Falha ao consultar DocSales" ou "Nenhuma proposta aprovada pendente encontrada").

### Caso de Uso 02 – Processar Propostas Aprovadas e Integrar com Sistemas Externos

* **Atores:** Usuário Interno, Sistema SIGVoIP, API DocSales, API Omie, API Next
* **Descrição:** O Usuário Interno seleciona uma ou mais propostas aprovadas para iniciar o processo de integração. Este caso de uso orquestra a criação/atualização de clientes no banco de dados local do SIGVoIP e nos sistemas Omie e Next, além da configuração dos serviços VoIP.
* **Pré-condições:** Propostas aprovadas consultadas e disponíveis para seleção.
* **Pós-condições:** Propostas selecionadas processadas, com registros criados/atualizados nos sistemas externos e status atualizado no SIGVoIP.
* **Fluxo Principal:**
    1.  O **Usuário Interno** visualiza a lista de propostas aprovadas e seleciona uma ou mais para processamento.
    2.  O **Usuário Interno** aciona o comando de "Processar".
    3.  Para cada proposta selecionada, o **Sistema SIGVoIP** executa os seguintes passos atômicos:
        * **3.1. Validação e Extração de Dados:** Extrai e valida os dados necessários (ex: CPF/CNPJ, nome, produtos) da proposta DocSales.
        * **3.2. Gerenciamento de Cliente SIG:**
            * Verifica a existência de um `ClienteSIG` correspondente no **Banco de Dados Local** (baseado no CPF/CNPJ).
            * Se o `ClienteSIG` não existir, o **Sistema SIGVoIP** cria um novo registro.
            * Se existir, atualiza dados se necessário.
        * **3.3. Integração com Omie:**
            * O **Sistema SIGVoIP** envia dados para a **API Omie** para criar ou atualizar o cliente Omie.
            * O **Sistema SIGVoIP** lança as contas a receber com recorrência via a **API Omie**.
            * O **Sistema SIGVoIP** armazena os IDs e dados de integração Omie no **Banco de Dados Local** (vinculados ao `ClienteSIG`).
        * **3.4. Integração com Next (NextBilling/NextRouter):**
            * O **Sistema SIGVoIP** seleciona o `ServidorNext` mais adequado (verificando capacidade e `TipoServidor`).
            * O **Sistema SIGVoIP** envia os dados do cliente e dos serviços para a **API Next** (para criar/atualizar o cliente Next e configurar os serviços VoIP).
            * O **Sistema SIGVoIP** armazena os IDs e dados de integração Next no **Banco de Dados Local** (vinculados ao `ClienteSIG` e `AssinanteNext`).
    4.  Após a conclusão de todas as integrações para uma proposta, o **Sistema SIGVoIP** atualiza o status de processamento da proposta na interface e no **Banco de Dados Local**.
    5.  O **Sistema SIGVoIP** registra logs detalhados de sucesso ou falha para todas as operações.
* **Fluxos Alternativos:**
    * **B.1. Intervenção Manual Pré-Processamento (RF05):**
        1.  Antes de acionar "Processar", o **Usuário Interno** pode acessar uma interface para ajustes pontuais nos dados da proposta (ex: nome do assinante, sufixo de serviço, datas de ativação).
        2.  O **Sistema SIGVoIP** valida as edições e as utiliza durante o processo de integração.
    * **B.2. Falha em uma ou mais Integrações (RN05, RNF07, RNF08):**
        1.  Se uma integração (Omie ou Next) falhar, o **Sistema SIGVoIP** registra o erro detalhadamente nos logs internos (com mensagem, status, `InnerException`).
        2.  O **Sistema SIGVoIP** **não** marca a proposta como totalmente processada.
        3.  O **Sistema SIGVoIP** atualiza o status da proposta na interface para indicar falha parcial/completa e a necessidade de reprocessamento manual.
        4.  Para o detalhamento técnico do tratamento de falhas e retentativas, consultar o **`07_diagrama_sequencia.md`**.

### Caso de Uso 03 – Visualizar Status e Reprocessar Integrações

* **Atores:** Usuário Interno, Sistema SIGVoIP
* **Descrição:** O Usuário Interno acessa um painel administrativo para visualizar o status de processamento das propostas, consultar logs detalhados de integração (sucesso e falha) e acionar o reprocessamento manual de propostas que apresentaram falha.
* **Pré-condições:** Processamentos anteriores executados.
* **Pós-condições:** Status das propostas e logs visíveis, com opção de reprocessamento para falhas.
* **Fluxo Principal:**
    1.  O **Usuário Interno** acessa o painel administrativo do **Sistema SIGVoIP**.
    2.  O **Sistema SIGVoIP** exibe um resumo das integrações, incluindo status por proposta (processada com sucesso, com falha, pendente), e links para logs detalhados e, quando aplicável, para os registros gerados nos sistemas externos (Omie/Next).
    3.  O **Usuário Interno** pode filtrar e visualizar os logs de integração, incluindo mensagens de erro e sucesso.
* **Fluxos Alternativos:**
    * **C.1. Reprocessar Proposta com Falha (RF19, RF20, RNF05):**
        1.  O **Usuário Interno** identifica uma ou mais propostas que falharam na integração.
        2.  O **Usuário Interno** aciona o comando de "Reprocessar" para as propostas selecionadas.
        3.  O **Sistema SIGVoIP** reinicia o processo de integração para a(s) proposta(s), garantindo idempotência e evitando duplicação de dados já enviados com sucesso.
        4.  Para o detalhamento técnico de como o reprocessamento gerencia a idempotência, consultar o **`07_diagrama_sequencia.md`**.

---

### **3. Considerações Futuras (Roadmap de Evolução)**

Os seguintes pontos representam funcionalidades que, embora importantes, não fazem parte do escopo da Etapa 1 e serão consideradas em futuras fases do projeto. Para mais detalhes, consulte o **`12_roadmap_futuro.md`**.

* **Acesso a Registros Criados (RF24/futuro):** O painel administrativo do SIGVoIP poderá, no futuro, exibir links diretos para os registros correspondentes criados nos sistemas Omie e Next, facilitando a navegação e verificação.
* **Edição de Dados da Proposta (RF05/futuro):** Embora já exista uma intervenção manual pontual, uma funcionalidade futura permitirá uma edição mais abrangente dos dados extraídos da proposta, com um formulário dedicado e controle de acesso por perfil.