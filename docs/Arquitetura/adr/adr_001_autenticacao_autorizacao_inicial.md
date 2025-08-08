# 📝 ADR 001: Abordagem de Autenticação e Autorização Inicial

* **Data da Decisão:** [Inserir data de registro]
* **Status:** Aceito
* **Contexto:** Definição da estratégia inicial para implementar a segurança de autenticação e autorização no sistema SIG VoIP. O objetivo é equilibrar a segurança com a agilidade na entrega das funcionalidades essenciais da Fase 1.
* **Decisão Tomada:** Optou-se por iniciar com uma implementação de segurança mais simples, utilizando **Identity e JWT** com usuários de acesso total.
* **Implicações Positivas:**
    * Acelera o desenvolvimento inicial, permitindo focar nas funcionalidades core de integração.
    * Reduz a complexidade da primeira fase do projeto.
* **Implicações Negativas:**
    * As regras de autorização mais refinadas (baseadas em perfis/permissões granulares) serão adiadas para uma fase posterior.
* **Alternativas Consideradas:**
    * Implementar um sistema de autenticação e autorização completo e refinado desde o início. (Rejeitado devido ao tempo e complexidade adicionais na fase inicial).
* **Pontos Pendentes/Em Progresso:**
    * A complexidade de regras de autorização mais refinadas e baseadas em perfis será tratada em uma evolução posterior, quando as necessidades se tornarem mais claras e o sistema base estiver estável.