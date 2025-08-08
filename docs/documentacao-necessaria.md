# 📚 Base de Documentos para o Projeto de Integração e Gestão Centralizada (SIGVoIP)

A organização e a clareza da documentação são pilares fundamentais para o sucesso do Projeto SIG VoIP. Este guia detalha o propósito de cada documento e propõe uma estrutura de pastas otimizada para facilitar o acesso, a manutenção e a colaboração.

---

## I. Catálogo Detalhado de Documentos do Projeto SIGVoIP

Este catálogo fornece a "identidade" de cada documento, descrevendo seu objetivo, o conteúdo essencial que deve abranger e o formato recomendado.

### 1. Visão Geral do Projeto
* **Nome Proposto:** `01_visao_geral_projeto.md`
* **Objetivo:** Servir como o ponto de partida para qualquer pessoa que busca compreender o projeto. Apresenta a motivação, o escopo macro e as expectativas em alto nível.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Nome do projeto, descrição geral, justificativa (problemas resolvidos), objetivos (gerais e específicos), escopo inicial (com delimitações), principais stakeholders, premissas e restrições.
* **Estrutura:** Dividido em seções claras e concisas, com uso de títulos e listas para facilitar a leitura.

### 2. Documento de Requisitos
* **Nome Proposto:** `02_documento_requisitos.md`
* **Objetivo:** Definir as funcionalidades, comportamentos e características que o sistema deve possuir para atender às necessidades do negócio e dos usuários.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Requisitos Funcionais (RFs), Requisitos Não Funcionais (RNFs), Regras de Negócio e Critérios de Aceitação para cada requisito.
* **Estrutura:** Organizado por tipo de requisito ou por módulo/funcionalidade, utilizando listas claras.

### 3. Casos de Uso
* **Nome Proposto:** `03_casos_de_uso.md`
* **Objetivo:** Representar as interações entre os atores (usuários ou sistemas externos) e o sistema, descrevendo como o sistema reage a essas interações.
* **Formato:** Markdown (`.md`) para a descrição textual. Diagrama UML visual (`.png`) para representação gráfica.
* **Conteúdo Esperado:** Identificação de atores, Diagrama de Casos de Uso (UML), e para cada caso de uso: objetivo, ator(es), pré/pós-condições, fluxo principal de eventos e fluxos alternativos/exceções.
* **Estrutura:** Seções dedicadas a cada caso de uso, com referências claras ao diagrama visual.

### 4. Protótipo de Telas (Wireframes/Mockups)
* **Nome Proposto:** `04_prototipos_telas.pdf`
* **Objetivo:** Fornecer uma visão visual das interfaces de usuário propostas, auxiliando na validação de requisitos e no entendimento do fluxo de navegação.
* **Formato:** Preferencialmente PDF (`.pdf`) para fácil visualização e compartilhamento. Pode ser HTML/ferramenta de prototipagem para interatividade.
* **Conteúdo Esperado:** Wireframes (baixa fidelidade) ou Mockups (média/alta fidelidade) das telas e um diagrama/descrição do fluxo de navegação entre elas.
* **Estrutura:** Um único arquivo PDF contendo todas as telas ou um índice com links, organizado por funcionalidade.

### 5. Diagrama de Classes (UML)
* **Nome Proposto:** `05_diagrama_classes.md` (descrição)
* **Objetivo:** Estruturar os principais objetos, entidades do domínio e suas relações, fornecendo a base para a implementação do código.
* **Formato:** Markdown (`.md`) para a descrição textual. Código PlantUML (`.plantuml`) para a fonte e PNG (`.png`) para a visualização.
* **Conteúdo Esperado:**
    * Classes/Entidades Principais: Foco em Aggregate Roots como `ClienteSIG`, e Value Objects como `DocumentoFiscal`, `Endereco`, `Email`, `Telefone`, além de entidades como `UsuarioSIG`, `AssinanteNext` (substituindo `ServicoContratado`).
    * Relacionamentos: Associação, agregação, composição, herança, dependência.
    * Atributos e Métodos Relevantes: Visibilidade, tipo de dado e comportamento essencial.
* **Estrutura:** Uma descrição textual no `.md` que complementa e referencia o diagrama visual gerado.

### 6. Modelo de Dados (DER / Lógico)
* **Nome Proposto:** `06_modelo_dados.md` (descrição)
* **Objetivo:** Definir a estrutura do banco de dados, incluindo tabelas, colunas, tipos de dados e os relacionamentos.
* **Formato:** Markdown (`.md`) para a descrição textual. Código PlantUML (`.plantuml`) para a fonte e PNG (`.png`) para a visualização do DER.
* **Conteúdo Esperado:** Entidades/tabelas, atributos/colunas (nomes, tipos, restrições), relacionamentos (PK, FK, cardinalidade), índices.
* **Estrutura:** Tabela descritiva no `.md` e um diagrama DER claro e conciso gerado a partir do PlantUML.

### 7. Diagrama de Sequência e Documento
* **Nome Proposto:** `07_diagrama_sequencia.md` (descrição)
* **Objetivo:** Representar a interação dinâmica entre objetos ou componentes ao longo do tempo para a execução de uma funcionalidade específica.
* **Formato:** Markdown (`.md`) para a descrição textual. Código PlantUML (`.plantuml`) para a fonte e PNG (`.png`) para a visualização.
* **Conteúdo Esperado:** Lifelines (objetos envolvidos), mensagens trocadas, fragmentos (loops, condicionais), e mapeamento para cenários de casos de uso.
* **Estrutura:** Uma descrição textual do fluxo no `.md`, acompanhada do diagrama visual.

### 8. Diagrama de Arquitetura do Sistema (ou Componentes) e Documento de Arquitetura
* **Nome Proposto:** `08_diagrama_arquitetura.md` (descrição)
* **Objetivo:** Definir e comunicar a estrutura de alto nível do sistema, incluindo suas camadas, módulos, principais componentes e suas interações.
* **Formato:** Markdown (`.md`) para a descrição textual. Código PlantUML (`.plantuml`) para a fonte e PNG (`.png`) para a visualização.
* **Conteúdo Esperado:** Visão geral da arquitetura (híbrida em camadas + Hexagonal), componentes principais, tecnologias e frameworks, comunicação entre módulos e diagramas de contexto/componentes.
* **Estrutura:** Seções que detalham cada aspecto da arquitetura, complementadas por diagramas visuais.

### 9. Plano de Desenvolvimento
* **Nome Proposto:** `09_plano_desenvolvimento.md`
* **Objetivo:** Descrever a estratégia e os procedimentos para a execução do desenvolvimento do software.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Metodologia (Kanban, etc.), papéis e responsabilidades, fluxo de trabalho (gerenciamento de tarefas), cronograma macro e backlog inicial.
* **Estrutura:** Seções claras para cada aspecto do planejamento do desenvolvimento.

### 10. Plano de Testes
* **Nome Proposto:** `10_plano_testes.md`
* **Objetivo:** Detalhar a estratégia, abordagens e atividades para garantir a qualidade do software através de testes.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Estratégia de testes (unitários, integração, funcionais, etc.), ambiente de testes, ferramentas, casos de teste por funcionalidade e critérios de sucesso/falha.
* **Estrutura:** Seções claras para cada tipo de teste e para os casos de teste.

### 11. Registro de Decisões Técnicas (ADRs)
* **Nome Proposto (Índice):** `11_registro_decisoes_tecnicas_indice.md`
* **Objetivo:** Documentar decisões arquiteturais e técnicas significativas para rastreabilidade, contexto e onboarding.
* **Formato:** Markdown (`.md`). Um arquivo principal (`11_registro_decisoes_tecnicas_indice.md`) servirá como índice para múltiplos arquivos Markdown individuais (`.md`) de ADRs.
* **Conteúdo Esperado (para cada ADR individual):** Título, data, status, contexto, decisão, implicações, alternativas consideradas, pontos pendentes/em progresso.
* **Estrutura:** O índice conterá uma lista de links para cada ADR individual, organizados cronologicamente ou por tema.

### 12. Roadmap de Etapas Futuras
* **Nome Proposto:** `12_roadmap_futuro.md`
* **Objetivo:** Esboçar a visão de longo prazo do projeto, identificando possíveis módulos, funcionalidades e integrações futuras após a conclusão da fase inicial.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Fases futuras, módulos potenciais, integrações previstas e áreas para melhorias contínuas.
* **Estrutura:** Uma linha do tempo ou lista de tópicos que forneça uma visão clara das próximas direções.

### 13. Resumo Executivo
* **Nome Proposto:** `13_resumo_executivo.md`
* **Objetivo:** Fornecer uma visão de alto nível do projeto, seus objetivos e status para um público executivo ou não-técnico, focando em aspectos de negócio e impacto.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Breve introdução ao projeto, principais objetivos/benefícios, status atual, próximos passos cruciais e métricas de sucesso.
* **Estrutura:** Documento conciso (1-2 páginas) com linguagem clara e direta.

### 14. Wiki Técnica (Consolidada)
* **Nome Proposto:** `14_wiki_tecnica.md`
* **Objetivo:** Ser o repositório central e dinâmico de informações técnicas detalhadas do projeto, consolidando diretrizes de desenvolvimento, princípios arquiteturais, tecnologias e boas práticas. É o "go-to" para desenvolvedores.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Visão geral e contexto técnico, diretrizes de desenvolvimento (qualidade de código, padrões, princípios), tecnologias e arquitetura detalhada, modelagem de entidades chave, integrações, boas práticas de engenharia (testes, logging, CI/CD, segurança), e pontos de configuração.
* **Estrutura:** Documento extenso, bem estruturado com títulos e subtítulos para navegação fácil.

### 15. Documento Formal do Projeto
* **Nome Proposto:** `15_documento_formal_projeto.docx`
* **Objetivo:** Um documento abrangente e formal para apresentações a stakeholders externos ou internos que necessitam de um compilado estruturado e padronizado.
* **Formato:** Word (`.docx`) para edição, ou PDF (`.pdf`) para distribuição.
* **Conteúdo Esperado:** Uma versão formalizada e integrada dos principais pontos da Visão Geral, Requisitos (alto nível), Arquitetura (alto nível), Roadmap, e Resumo Executivo. Pode incluir apêndices com diagramas.
* **Estrutura:** Formato de relatório formal, com seções padronizadas e sumário executivo.

### 16. README
* **Nome Proposto:** `README.md`
* **Objetivo:** O primeiro ponto de contato para desenvolvedores e colaboradores do projeto. Fornece uma visão rápida de como iniciar, o que é o projeto e onde encontrar mais informações.
* **Formato:** Markdown (`.md`).
* **Conteúdo Esperado:** Título do projeto, breve descrição, instruções básicas de setup/contribuição, estrutura do repositório e links úteis para a documentação completa.
* **Estrutura:** Conciso, direto ao ponto, com links para documentação mais detalhada.

### 17. Contexto de Sistemas (Diagrama Visual)
* **Nome Proposto:** `17_contexto_sistemas.png`
* **Objetivo:** Diagrama visual que representa o SIG VoIP no contexto do ecossistema de sistemas existentes, mostrando as integrações principais.
* **Formato:** PNG (`.png`).
* **Conteúdo Esperado:** Representação gráfica das caixas de sistemas (DocSales, Omie, Next, SIG VoIP) e suas interconexões de alto nível.
* **Estrutura:** Arquivo de imagem.

### 18. Modelo de Domínio (Diagrama Visual)
* **Nome Proposto:** `18_modelo_dominio.png`
* **Objetivo:** Diagrama visual que ilustra as principais entidades e Value Objects do domínio, e seus relacionamentos, focando na lógica de negócio e nos conceitos do DDD.
* **Formato:** PNG (`.png`).
* **Conteúdo Esperado:** Representação das Aggregate Roots, Entidades, Value Objects e suas associações no contexto do DDD.
* **Estrutura:** Arquivo de imagem.

### 19. Origem de Diagramas (Draw.io / SistemaSIGVoIP.drawio)
* **Nome Proposto:** `19_origem_diagramas.drawio`
* **Objetivo:** Arquivo fonte para edição de diagramas visuais do projeto, garantindo que as imagens geradas (PNGs) possam ser atualizadas e mantidas.
* **Formato:** `.drawio` (formato da ferramenta Draw.io/Diagrams.net).
* **Conteúdo Esperado:** Múltiplas páginas ou camadas contendo os diagramas de contexto, arquitetura de alto nível, modelo de domínio, etc., que são exportados para imagens.
* **Estrutura:** Arquivo único ou pasta de arquivos `.drawio`.

---

## II. Estrutura de Pastas Proposta para Documentação

Apresentamos a estrutura de pastas otimizada para o repositório de documentos do Projeto SIG VoIP, visando clareza e organização lógica.

/docs
├── README.md                                          # 16. README (já existente)
├── 01_visao_geral_projeto.md                          # 1. Visão Geral do Projeto (já existente)
├── 13_resumo_executivo.md                             # 13. Resumo Executivo (já existente)
├── 14_wiki_tecnica.md                                 # 14. Wiki Técnica (já existente)
├── 15_documento_formal_projeto.docx                   # 15. Documento Formal do Projeto (já existente)
│
├── /arquitetura
│   ├── 05_diagrama_classes.md                         # 5. Diagrama de Classes (já existente)
│   ├── 06_modelo_dados.md                             # 6. Modelo de Dados (já existente)
│   ├── 07_diagrama_sequencia.md                       # 7. Diagrama de Sequência (já existente)
│   ├── 08_diagrama_arquitetura.md                     # 8. Diagrama de Arquitetura do Sistema (já existente)
│   │
│   ├── /adr                                           # Pasta para ADRs individuais (NOVO)
│   │   ├── 11_registro_decisoes_tecnicas_indice.md    # 11. Registro de Decisões Técnicas (Índice - DEVE SER CRIADO)
│   │   ├── adr_001_modelagem_contatos.md              # Exemplo de ADR individual (DEVE SER CRIADO)
│   │   └── ...                                        # Outros ADRs futuros
│   │
│   ├── /plantuml                                      # Código-fonte para diagramas UML
│   │   ├── 05_diagrama_classes.plantuml               # Já existente
│   │   ├── 06_modelo_dados.plantuml                   # Já existente
│   │   └── 08_diagrama_estrutural.plantuml            # Já existente
│   │
│   └── /visual                                        # Imagens geradas de diagramas
│       ├── 05_diagrama_classes.png                    # DEVE SER CRIADO (a partir de plantuml)
│       ├── 06_DER.png                                 # Já existente (referenciado ao 06_modelo_dados)
│       ├── 07_diagrama_sequencia.png                  # DEVE SER CRIADO (a partir de plantuml)
│       ├── 08_diagrama_arquitetura.png                # DEVE SER CRIADO (a partir de plantuml)
│       ├── arquitetura-alto-nivel.png                 # Já existente (referenciado ao 08_diagrama_arquitetura)
│       ├── 17_contexto_sistemas.png                   # 17. Contexto de Sistemas (já existente, renomeado de contexto-sistemas.png)
│       └── 18_modelo_dominio.png                      # 18. Modelo de Domínio (já existente, renomeado de modelo-dominio.png)
│
├── /planejamento
│   ├── 02_documento_requisitos.md                     # 2. Documento de Requisitos (já existente)
│   ├── 03_casos_de_uso.md                             # 3. Casos de Uso (já existente)
│   ├── 09_plano_desenvolvimento.md                    # 9. Plano de Desenvolvimento (já existente)
│   ├── 10_plano_testes.md                             # 10. Plano de Testes (já existente)
│   └── 12_roadmap_futuro.md                           # 12. Roadmap de Etapas Futuras (já existente)
│
├── /prototipos                                        # Pasta dedicada a protótipos de tela (NOVA)
│   └── 04_prototipos_telas.pdf                        # 4. Protótipo de Telas (DEVE SER CRIADO)
│
└── /fontes_diagramas                                  # Pasta para arquivos fonte de ferramentas de diagramação (NOVA)
    └── 19_origem_diagramas.drawio                     # 19. Origem de Diagramas (já existente, renomeado de SistemaSIGVoIP.drawio)