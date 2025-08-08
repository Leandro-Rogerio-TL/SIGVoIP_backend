# 📝 ADR 005: Estrutura de Modelagem de Contatos no Domínio

* **Data da Decisão:** 2025-06-21
* **Status:** Aceito (com pontos pendentes para refinamento na implementação)
* **Contexto:** O domínio do SIG VoIP precisa modelar informações de contato para `ClienteSIG` (PJ/PF), incluindo múltiplos e-mails e telefones com metadados (uso, tipo, WhatsApp). A discussão focou na estrutura e tipologia.
* **Decisão Tomada:** Adotar uma estrutura granular de Value Objects (VOs) para modelar contatos:
    1.  **Value Object `Telefone`**: DDD, Número, Ramal (sem metadados de uso/tipo/WhatsApp). Validação básica no construtor.
    2.  **Value Object `Email`**: Endereço validado.
    3.  **Enums `UsoTelefone` e `TipoTelefone`**: Para classificar uso (Comercial, Pessoal) e tipo (Fixo, Celular).
    4.  **Value Object `ContatoTelefonico`**: Agrega `Telefone` VO e propriedades de tipologia (`Uso` - Enum, `Tipo` - Enum, `EhWhatsapp` - booleano), com validação de coerência.
    5.  **Value Object de Agrupamento (`ConjuntoDeContatos` - nome provisório)**: Agrupa um `Email` principal (opcional) e uma lista de `ContatoTelefonico` VOs.
    6.  **Value Object `PessoaContato`**: Nome, Cargo/Departamento (opcional), e seu próprio `ConjuntoDeContatos`.
    7.  **Entidade `ClienteSIG`**: Agrega `Endereco`, `ContatosGerais` (tipo `ConjuntoDeContatos`), e uma lista de `PessoasDeContato` (tipo `PessoaContato`). Será um Aggregate Root.
* **Implicações Positivas:**
    * Modelagem clara e expressiva dos conceitos de contato no domínio.
    * Imutabilidade e validação centralizada em VOs, garantindo consistência.
    * Flexibilidade para diferentes tipos de contatos (gerais vs. específicos).
    * Reuso de VOs.
* **Implicações Negativas:**
    * Aumento do número de classes pequenas, elevando o *overhead* inicial.
    * Requer sólido entendimento de DDD.
    * Mapeamento cuidadoso para persistência e DTOs.
* **Alternativas Consideradas:**
    * (Não explicitadas, mas implícita a alternativa de modelagem menos granular ou com menos VOs).
* **Pontos Pendentes/Em Progresso:**
    * **Refinamento da Entidade `ClienteSIG`**: Avaliação adicional dos comportamentos e invariantes como Aggregate Root durante a implementação.
    * **Validações do VO `ContatoTelefonico`**: Detalhar e implementar regras de coerência (ex: ramal só com Fixo).
    * **Redução de `if/else`**: Avaliar aplicação de Objetos Calistênicos e Programação Funcional para simplificar lógica.
    * **Tratamento Centralizado de Exceções**: Definir estratégia para lançamento e tratamento centralizado de exceções de domínio.
    * **Implementação Inicial**: Início/conclusão conceitual e de código inicial dos VOs de Contato e Enums.
* **Consequências:**
    * Criação de novos VOs (`ContatoTelefonico`, `ConjuntoDeContatos`, `PessoaContato`) e Enums (`UsoTelefone`, `TipoTelefone`).
    * Definição da nomenclatura final para o VO de agrupamento.
    * Mapeamento para DTOs e modelagem física do banco de dados precisarão refletir esta nova estrutura.
    * Continuação do investimento em discussões de design sobre padrões de código e tratamento de exceções.