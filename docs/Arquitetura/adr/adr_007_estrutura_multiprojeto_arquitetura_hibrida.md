# 📝 ADR 007: Estrutura Multi-projeto e Arquitetura Híbrida

* **Data da Decisão:** [Inserir data de registro]
* **Status:** Aceito
* **Contexto:** Definir a organização física do código-fonte e os princípios arquiteturais de alto nível para o sistema SIG VoIP. O objetivo é garantir flexibilidade, testabilidade e manutenibilidade.
* **Decisão Tomada:** Estrutura multi-projeto definida seguindo um modelo **híbrido Camadas + Hexagonal**. Início do registro de Decisões de Arquitetura (ADRs) para documentar essas escolhas.
* **Implicações Positivas:**
    * Isola a lógica de negócio central de detalhes de infraestrutura e tecnologia externas.
    * Garante flexibilidade para trocar implementações de infraestrutura.
    * Melhora a testabilidade do domínio e da aplicação.
    * Facilita a manutenção e a escalabilidade.
    * Segue a Regra de Dependência (dependências apontam para dentro).
* **Implicações Negativas:**
    * Curva de aprendizado inicial para a equipe que não está familiarizada com Arquitetura Hexagonal.
    * Overhead na criação e gerenciamento de múltiplos projetos.
* **Alternativas Consideradas:**
    * Arquitetura em camadas puras. (Rejeitado pela menor flexibilidade e acoplamento a detalhes de infraestrutura).
    * Arquitetura monolítica. (Rejeitado pela dificuldade de escalabilidade e manutenção em longo prazo).
* **Pontos Pendentes/Em Progresso:**
    * Refinamento contínuo da organização dos projetos e módulos.
    * Garantir que a regra de dependência seja estritamente seguida.