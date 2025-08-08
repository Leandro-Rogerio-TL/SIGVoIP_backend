# 📈 12. Roadmap de Etapas Futuras - SIG VoIP

## Objetivo
Este documento esboça a visão de longo prazo do projeto SIG VoIP, identificando possíveis módulos, funcionalidades e integrações futuras após a conclusão da fase inicial, servindo como um guia para o desenvolvimento contínuo do sistema.

---

## 1. Expansões Imediatas (Pós-Fase 1)

Esta seção detalha as expansões e melhorias que complementarão as funcionalidades da Fase 1, visando otimizar a operação e usabilidade do SIG VoIP no curto prazo:

* **Edição Manual de Dados de Proposta:** Permitir que o **Usuário Interno** realize edições nos dados da proposta DocSales através de um formulário antes da integração com Omie/Next. Isso incluirá controle de campos por perfil de acesso para garantir a segurança e a integridade dos dados.
* **Seleção Manual de Servidor Next:** Implementar a capacidade para o **Usuário Interno** (Administrador) poder escolher manualmente o `ServidorNext` para um serviço. Esta funcionalidade complementará a lógica atual de escolha automática, oferecendo maior controle operacional e flexibilidade em cenários específicos.
* **Navegação Rápida para Registros Externos:** Geração e exibição de links diretos para os registros de cliente e/ou serviço correspondentes criados nos sistemas **Omie** e **Next**. O objetivo é melhorar a usabilidade e a eficiência do usuário, permitindo uma rápida verificação e navegação entre os sistemas.
* **Cadastro Direto de Cliente no SIG VoIP:** Permitir o cadastro manual de clientes diretamente no SIG VoIP, sem a necessidade de uma proposta aprovada do DocSales como origem. Essa funcionalidade expandirá o uso do sistema para cenários avulsos ou migração de dados históricos.
* **Envio de Dados do Cliente SIG para DocSales:** Criar funcionalidades onde o SIG VoIP poderá atuar como fonte de dados para o DocSales em cenários específicos, como atualizações de cadastro ou criação de novos contratos que não sigam o fluxo padrão de proposta.

---

## 2. Módulos e Funcionalidades Futuras

Esta seção descreve novos módulos e funcionalidades que serão considerados para fases subsequentes do projeto, visando ampliar as capacidades do sistema e a experiência do usuário:

* **Interface Frontend Completa:** Implementação de uma interface de usuário completa e rica, utilizando tecnologias modernas como **Vue.js + TypeScript**, para uma experiência de usuário aprimorada e intuitiva.
* **Dashboards e Painéis de Gestão:** Desenvolvimento de dashboards com indicadores chave de performance (KPIs), incluindo taxas de sucesso, falhas e tempo médio de integração, para monitoramento proativo do sistema. Além disso, a criação de painéis de gestão técnica, financeira e operacional para insights em tempo real sobre serviços e integrações.
* **Notificações e Alertas:** Criação de um sistema robusto de notificações e alertas para informar proativamente sobre falhas de integração, eventos importantes ou anomalias no sistema.
* **Módulo de Auditoria de Integrações:** Um módulo dedicado para rastrear e auditar todas as operações de integração, fornecendo um histórico completo e detalhado de todas as transações e eventos.
* **Gerenciamento de Múltiplas Contas/API Keys:** Implementação de uma funcionalidade para gerenciar múltiplas contas ou chaves de API para integrações externas. Isso permitirá maior flexibilidade e escalabilidade, suportando diferentes configurações por cliente ou ambiente.
* **Configuração de Rede (Expansão):** Desenvolvimento completo da capacidade de configuração de rede via Shell/API, com o escopo e a tecnologia sendo definidos após a fase de estudo inicial, que abordará a segurança e a viabilidade técnica.
* **Portal de Autoatendimento para Clientes e Revendedores:** Desenvolver uma interface externa segura para que clientes e/ou revendedores possam gerenciar seus próprios serviços, visualizar faturas e acessar seus dados diretamente.

---

## 3. Integrações Futuras

Esta seção lista outras plataformas e sistemas com os quais o SIG VoIP poderá se integrar no futuro, expandindo seu ecossistema e potencializando o valor de negócio:

* **Outras Plataformas VoIP ou ERPs:** Integração com sistemas adicionais de Voz sobre IP ou sistemas de planejamento de recursos empresariais (ERPs), conforme as necessidades estratégicas do negócio e o surgimento de novas parcerias.
* **API de Consulta de Documentos Fiscais:** Integração com APIs externas para consulta e validação de documentos fiscais (e.g., Receita Federal), para enriquecimento e verificação de dados de cliente, garantindo maior conformidade.
* **Ferramentas de BI ou CRM Internas/Externas:** Conectividade com ferramentas internas de Business Intelligence (BI) para análise de dados e geração de relatórios estratégicos, e com sistemas de Customer Relationship Management (CRM) para uma gestão de relacionamento com o cliente mais unificada.

---

## 4. Considerações Estratégicas

* **Priorização Contínua:** Todas as funcionalidades e integrações listadas neste roadmap serão priorizadas com base nas necessidades de negócio, feedback dos usuários, viabilidade técnica e impacto estratégico após a conclusão da Etapa 1.
* **Evolução da Autorização:** O roadmap prevê uma evolução do módulo de autenticação e autorização para implementar regras mais granulares e baseadas em perfis, indo além do acesso total inicial, garantindo segurança e aderência a diferentes papéis de usuário.
* **Abordagem de Desenvolvimento:** A abordagem iterativa e incremental de desenvolvimento será mantida para todas as fases futuras do projeto, garantindo flexibilidade, adaptação contínua e entrega de valor em ciclos curtos.
* **Escalabilidade e Manutenção:** As decisões arquiteturais e de design continuarão a priorizar a escalabilidade, a manutenibilidade e a resiliência do sistema em todas as futuras expansões.