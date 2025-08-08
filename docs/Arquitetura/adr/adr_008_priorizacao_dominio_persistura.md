# 📝 ADR 008: Priorização da Modelagem do Domínio sobre Implementação da Persistência

* **Data:** 2025-07-01 (Data de criação do ADR)
* **Status:** Proposto / Aceito
* **Contexto:** No início do projeto SIGVoIP, foi identificado que a complexidade do domínio de negócio (clientes, serviços VoIP, integrações) exigia uma modelagem robusta e clara para garantir a coerência e a escalabilidade do sistema. Ao mesmo tempo, a implementação do banco de dados (persistência) poderia adicionar uma camada de complexidade adicional se abordada simultaneamente à definição do domínio.
* **Decisão Tomada:** Decidiu-se priorizar a modelagem e a implementação das **entidades e Value Objects do domínio**, com foco em sua lógica de negócio e comportamentos. A implementação detalhada da camada de **persistência (banco de dados)** será adiada para uma etapa posterior, permitindo que o modelo de domínio amadureça e se estabilize antes de seu mapeamento final ao banco de dados.
* **Implicações Positivas:**
    * Maior clareza e solidez no modelo de domínio.
    * Redução da complexidade inicial do desenvolvimento.
    * Facilita a validação das regras de negócio independentemente da tecnologia de persistência.
* **Implicações Negativas:**
    * Requer ferramentas ou abordagens para simular a persistência no desenvolvimento inicial (e.g., repositórios em memória, bancos de dados embutidos).
    * A definição exata de tipos de colunas e otimizações de banco de dados será postergada.
* **Alternativas Consideradas:**
    * **Desenvolvimento Paralelo:** Modelagem de domínio e persistência simultaneamente. Rejeitado devido ao risco de maior complexidade e interdependência precoce, dificultando alterações em qualquer uma das camadas.
    * **Database First:** Iniciar pela modelagem do banco de dados e derivar o domínio a partir dela. Rejeitado por ir contra os princípios de Domain-Driven Design (DDD), onde o domínio deve ser a "fonte de verdade".
* **Pontos Pendentes/Em Progresso:**
    * Definir a estratégia exata para persistência (ORM, tipo de banco de dados) na próxima fase.
    * Estabelecer o cronograma para a fase de implementação da persistência.