# 🧭 Visão Geral do Projeto - SIGVoIP

Este documento oferece uma visão concisa do Projeto SIGVoIP, detalhando seu propósito, objetivos, escopo inicial e principais direcionadores estratégicos. Ele serve como uma referência fundamental para todos os stakeholders interessados em compreender a essência e o alcance inicial do sistema.

---

## 1. Contexto do Projeto

### 1.1. Nome do Projeto

**SIGVoIP – Sistema Integrado de Gestão para Empresa Prestadora de Telefonia e Call Center**

### 1.2. Descrição Geral

O **SIGVoIP** é um sistema interno projetado para **centralizar, padronizar e automatizar o fluxo de informações comerciais, técnicas e financeiras** em uma empresa prestadora de serviços de **Telefonia VoIP, Call Center e Internet**.

Atuando como um **núcleo integrador e orquestrador**, o sistema garante consistência, rastreabilidade e governança sobre os dados transacionados entre os principais sistemas envolvidos no ciclo de vida do cliente: **DocSales** (propostas comerciais aprovadas), **NextBilling e NextRouter** (gestão técnica e de serviços, referidos coletivamente como “Nexts”), e **Omie** (gestão financeira e de faturamento – ERP).

O SIGVoIP visa otimizar processos, reduzir a intervenção manual e garantir a consistência dos dados entre as plataformas, impedindo que clientes sejam faturados sem estarem devidamente ativados ou que serviços sejam ativados sem previsão de faturamento.

### 1.3. Justificativa

O fluxo atual de integração é descentralizado, manual e sujeito a falhas, gerando impactos operacionais e financeiros significativos, tais como: erros por omissões/preenchimentos incorretos, falta de sincronização entre faturamento e ativação de serviços, faturamento indevido ou ativação sem previsão financeira, cadastros redundantes/conflitantes, e ausência de rastreabilidade para suporte e auditoria.

A implantação do SIGVoIP soluciona esses problemas ao **automatizar o processo, consolidar os dados em uma estrutura única (`ClienteSIG`) e garantir o controle total das integrações realizadas**. O projeto busca eliminar erros, acelerar o processo de ativação de clientes e serviços, e liberar recursos humanos para tarefas mais estratégicas.

### 1.4. Objetivos

#### Objetivo Geral

Automatizar o processamento de propostas comerciais aprovadas, integrando os sistemas envolvidos de forma padronizada, segura e auditável, otimizando processos e consolidando informações.

#### Objetivos Específicos

* Automatizar a integração de propostas aprovadas, via API e painel administrativo.
* Processar propostas aprovadas do DocSales.
* Criar ou atualizar cadastros de clientes e serviços nas plataformas Nexts.
* Configurar serviços VoIP no Next, incluindo a escolha inteligente de servidores.
* Criar, atualizar ou prorrogar contas a receber no Omie, com uso de recorrência.
* Lançar contas a receber recorrentes no Omie.
* Consolidar os dados na entidade `ClienteSIG`, com **unicidade por CPF/CNPJ**.
* Sincronizar e armazenar identificadores externos: `IdDocSales`, `IdOmie`, `IdNext`, `IPServidorNext`.
* Permitir ajustes manuais antes do envio aos sistemas externos.
* Fornecer um painel intuitivo para visualização e seleção das propostas.
* Permitir edições posteriores diretamente nas plataformas Nexts via SIGVoIP.
* Registrar logs detalhados de ações e falhas, com rastreabilidade total.
* Manter um registro detalhado de logs para todas as operações.
* Implementar autenticação e autorização por perfis (Admin / Financeiro).
* Habilitar o reprocessamento de propostas que apresentaram falha.
* Prevenir faturamento sem ativação de serviços e vice-versa.
* Reduzir erros e retrabalho por entrada manual.
* Promover governança e confiabilidade entre os sistemas.
* Assegurar a resiliência do sistema frente a erros de integração.

---

## 2. Escopo Inicial – Etapa 1

A primeira etapa do projeto abrange a automação do fluxo iniciado por propostas aprovadas no DocSales, com monitoramento e supervisão do operador através de interface administrativa. O foco principal é a automação da integração, a configuração inicial e a persistência das entidades primárias e secundárias que suportam este fluxo de negócios. Isso inclui:

* Consulta e listagem de propostas aprovadas ainda não processadas.
* Interface para seleção e processamento individual ou em lote.
* Criação da entidade `ClienteSIG`, que centraliza os dados do cliente e seus vínculos externos.
* Verificação de unicidade por CPF/CNPJ para evitar duplicidades.
* Criação ou atualização de **`AssinantesNext`**, diferenciados por tipo de serviço e nome com sufixo.
* Relacionamento de um `ClienteSIG` com múltiplos `AssinantesNext`, cada um associado a um `ServidorNext` e seu respectivo IP.
* Criação ou atualização de cadastros e contas a receber no Omie com recorrência configurada.
* Distribuição inteligente de novos assinantes entre servidores Next com base na capacidade (clientes e DIDs).
* Registro detalhado de logs de integração e falhas, com tratamento robusto de erros.
* Autenticação com ASP.NET Identity e controle de acesso baseado em perfis.
* Painel administrativo para gestão e acompanhamento das integrações.

A Etapa 1 abrange exclusivamente fluxos iniciados por propostas aprovadas no DocSales e todos os fluxos obrigatoriamente têm como ponto de partida o DocSales. O escopo inicial concentra-se no desenvolvimento do backend necessário para orquestrar essa comunicação, incluindo a modelagem e implementação de **Value Objects essenciais** (`DocumentoFiscal`, `CEP`, `Endereco`, `Email`, `Telefone`), que já foram concluídas e refinadas.

---

## 3. Premissas e Abordagem Geral

O projeto busca resolver problemas de ineficiência e inconsistência de dados, com a equipe de desenvolvimento comprometida em construir um **código de alta qualidade** e um sistema **manutenível, testável e robusto**.

### 3.1. Premissas

* As APIs RESTful dos sistemas DocSales, Omie e Nexts estão estáveis, acessíveis e documentadas.
* A empresa possui as credenciais e permissões adequadas para autenticação nas integrações.
* Os dados das propostas aprovadas são válidos e completos para disparar o fluxo.
* O ambiente de execução do SIGVoIP tem conectividade com os servidores Next.
* As especificações das APIs externas serão fornecidas ou poderão ser inferidas pela documentação existente.

---

## 4. Stakeholders

| Papel                   | Nome/Função                                        |
| :---------------------- | :------------------------------------------------- |
| **Sponsor do Projeto**  | Leandro Rogerio                                    |
| **Responsável Técnico** | Leandro Rogerio                                    |
| **Desenvolvimento**     | Leandro Rogerio                                    |
| **Usuários Finais**     | Equipe Administrativa e Financeira                 |
| **Usuário Interno**     | Representado por perfis como Financeiro e Administrador, que interagem diretamente com o sistema para gerenciar e monitorar as integrações.|

---

## 5. Restrições e Delimitações

* A Etapa 1 abrange exclusivamente fluxos iniciados por propostas aprovadas no DocSales.
* Todos os fluxos obrigatoriamente têm como ponto de partida o DocSales.
* Funcionalidades como autoatendimento, dashboards e portal de revenda serão tratadas em fases futuras.
* O desenvolvimento inicial será realizado por um único responsável (Leandro Rogerio). A capacidade de configuração de rede via Shell/API será analisada e seu escopo definido futuramente.

---

## 6. Considerações Futuras (Roadmap)

O SIGVoIP será expandido progressivamente, contemplando:

* Portal de autoatendimento para clientes e revendedores.
* Painéis de gestão técnica, financeira e operacional.
* Dashboards e relatórios analíticos.
* Integrações adicionais com ferramentas de marketing, suporte e atendimento.
* Gerenciamento granular de acesso com controle por módulo, função e permissão.