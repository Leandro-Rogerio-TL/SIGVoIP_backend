# 📝 ADR 002: Seleção de Ferramentas e Tecnologias Core

* **Data da Decisão:** [Inserir data de registro]
* **Status:** Aceito
* **Contexto:** Definição da stack tecnológica principal para o desenvolvimento do backend, banco de dados e integrações do projeto SIG VoIP. A escolha visa garantir robustez, performance, adequação ao ecossistema corporativo e familiaridade da equipe.
* **Decisão Tomada:**
    * **Linguagem de Programação:** .NET / C#
    * **Banco de Dados:** MySQL
    * **Integrações:** APIs REST para DocSales, Omie, Next; potencial uso de Shell/API para configuração de rede.
    * **Segurança:** Identity + JWT
    * **Logging:** Serilog
    * **Controle de Versão:** Git
    * **Documentação:** Markdown e Diagramas `.drawio`
* **Implicações Positivas:**
    * Aproveitamento da familiaridade do desenvolvedor principal (Leandro Rogerio) com as tecnologias.
    * Alinhamento com padrões de mercado e boas práticas para sistemas corporativos.
    * Boa compatibilidade e ecossistema para integrações.
* **Implicações Negativas:**
    * Nenhuma implicação negativa significativa identificada no momento da decisão, considerando o contexto do projeto.
* **Alternativas Consideradas:**
    * (Não explicitadas, mas implícitas nas escolhas padrão para o perfil do projeto).
* **Pontos Pendentes/Em Progresso:**
    * Manter a avaliação de novas tecnologias conforme o projeto evolui.