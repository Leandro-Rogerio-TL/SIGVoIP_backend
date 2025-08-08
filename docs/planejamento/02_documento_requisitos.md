# 📋 Documento de Requisitos – Etapa 1 do SIGVoIP

## Processamento de Propostas do DocSales com Integração ao NextBilling, NextRouter e Omie

---

## Objetivo
Este documento define as funcionalidades, comportamentos esperados e regras de negócio para o sistema SIGVoIP, servindo como base para o desenvolvimento e validação da Etapa 1 do projeto.

---

## ✅ Requisitos Funcionais (RF)

| Código | Requisito |
|--------|----------|
| RF01   | O sistema deve consultar periodicamente (ou por demanda do usuário) a API do DocSales para recuperar propostas com status “Aprovado” e que ainda não foram processadas. |
| RF02   | O sistema deve exibir lista de propostas aprovadas com status visual e permitir seleção manual (individual ou múltipla) para processamento. |
| RF03   | O sistema deve exibir uma lista clara e organizada de propostas aprovadas e não processadas para o usuário. |
| RF04   | O usuário deve ser capaz de selecionar individualmente ou em lote as propostas exibidas para iniciar o processo de integração. |
| RF05   | O sistema deve permitir intervenção manual para ajustes pontuais antes do envio (como nome do assinante, datas, produtos, sufixo, etc.). |
| RF06   | O sistema deve extrair automaticamente o CPF/CNPJ da proposta DocSales selecionada. |
| RF07   | O sistema deve verificar se já existe `ClienteSIG` por CPF/CNPJ e criar novo cliente caso não exista, garantindo unicidade documental no banco de dados local. |
| RF08   | O sistema deve atualizar ou criar clientes nos sistemas NextBilling/NextRouter via API REST (`/manageCustomers`) com os dados da proposta. |
| RF09   | O sistema deve configurar os serviços VoIP associados ao cliente na API Next. |
| RF10   | O sistema deve associar serviços contratados do cliente com base nos produtos da proposta, permitindo personalização do sufixo no nome do assinante. |
| RF11   | O sistema deve verificar automaticamente a capacidade dos servidores Next (clientes e DIDs) e selecionar aquele com maior disponibilidade. |
| RF12   | O sistema deve sugerir automaticamente o `ServidorNext` mais adequado para o serviço, com base em critérios como capacidade disponível. |
| RF13   | O sistema deve criar ou atualizar cliente no Omie (`/clientes/incluir` ou `/alterar`), com cadastro completo e vínculos à proposta. |
| RF14   | O sistema deve gerar contas a receber recorrentes no Omie com base nas parcelas da proposta, utilizando o recurso de `repeticao`, e permitindo configuração de campos como `codigo_categoria`, `codigo_tipo_documento` e `id_conta_corrente`. |
| RF15   | O sistema deve lançar contas a receber na API Omie (via `/contacorrente/lancamentos/incluirconta`), configurando a recorrência necessária para o serviço. |
| RF16   | O sistema deve armazenar e centralizar os identificadores externos (DocSales, Omie, Next) em estruturas aninhadas no `ClienteSIG` e `ServicoContratado`, associando ou atualizando as entidades de integração (`ClienteNextIntegracao`, `ClienteOmieIntegracao`) com o `ClienteSIG` (ou `ServicoContratado`). |
| RF17   | O sistema deve registrar logs detalhados (com status e mensagens) de todas as etapas do processamento, incluindo falhas. |
| RF18   | O sistema deve exibir uma interface para o usuário visualizar os logs de integração. |
| RF19   | O sistema deve permitir reprocessamento manual de propostas com falha anterior, sem duplicar dados já enviados com sucesso. |
| RF20   | O usuário deve ser capaz de acionar manualmente o reprocessamento de propostas que falharam na integração. |
| RF21   | O sistema deve autenticar usuários com login/senha e controlar permissões por perfil (“Administrador” com acesso total e “Financeiro” com acesso restrito a integrações e finanças). |
| RF22   | O sistema deve implementar um módulo de autenticação para usuários internos, utilizando Identity e JWT. |
| RF23   | O sistema deve aplicar controle de acesso por perfil (ex: Administrador, Financeiro) para determinadas funcionalidades e endpoints RESTful. |
| RF24   | O sistema deve exibir painel administrativo com resumo das integrações, status por proposta, links diretos para registros no Omie/Next e mensagens de erro ou sucesso. |
| RF25   | O sistema deve fornecer um painel administrativo básico para visualização e gerenciamento geral das operações. |
| RF26   | O sistema deve atualizar o status da proposta na interface do usuário e no banco de dados local após o processamento. |

---

## ⚙️ Requisitos Não Funcionais (RNF)

| Código | Requisito |
|--------|----------|
| RNF01  | O sistema deve seguir arquitetura baseada em DDD, Clean Architecture e Hexagonal. |
| RNF02  | A comunicação com DocSales, Omie e Next deve ocorrer via API RESTful utilizando JSON sobre HTTPS. |
| RNF03  | Toda requisição externa deve ser autenticada e possuir tratamento robusto de erro (status, logs, mensagens). |
| RNF04  | Logs devem ser registrados com Serilog, com persistência adequada para auditoria e rastreabilidade. |
| RNF05  | O sistema deve garantir idempotência nas integrações para evitar duplicações. |
| RNF06  | Interface web protegida por autenticação e autorização baseada em JWT (Identity), com carregamento rápido e UX fluida (< 2s por ação). |
| RNF07  | O sistema deve tolerar falhas de rede e APIs externas, utilizando retentativa com backoff exponencial. |
| RNF08  | O sistema deve ser resiliente a falhas temporárias nas APIs externas, permitindo retentativa das operações em caso de erro. |
| RNF09  | Banco de dados relacional (MySQL) com modelo consistente e documentado. |
| RNF10  | O frontend (a ser implementado) deve utilizar Vue.js e TypeScript, com foco em usabilidade por perfis não técnicos. |
| RNF11  | O sistema deve garantir a persistência e a integridade dos dados de cliente e status de integração. |
| RNF12  | O sistema deve prover logs compreensíveis e completos para facilitar o rastreamento, depuração e monitoramento das operações. |
| RNF13  | A arquitetura de segurança deve permitir uma autenticação inicial mais simples, mas com capacidade de evolução para regras de autorização mais refinadas no futuro. |
| RNF14  | O código-fonte e a arquitetura devem ser desenvolvidos de forma iterativa e incremental, facilitando futuras manutenções e evoluções. |

---

## 📐 Regras de Negócio

| Código | Regra |
|--------|-------|
| RN01   | O `ClienteSIG` é único por CPF/CNPJ e centraliza as integrações com Omie, DocSales e múltiplos assinantes Nexts. |
| RN02   | A proposta só poderá ser processada uma única vez pelo SIGVoIP; o sistema deve manter rastreabilidade de propostas já processadas. |
| RN03   | Um mesmo cliente pode ter múltiplos assinantes no Next, um para cada serviço contratado (diferenciados por sufixo). |
| RN04   | O nome do assinante no Next deve ser composto pelo nome base do cliente + sufixo de serviço (ex: “ACME - Voz IP”), sugerido automaticamente mas editável. |
| RN05   | O processamento será considerado concluído apenas se todas as integrações forem realizadas com sucesso; a falha parcial será registrada e permitirá reprocessamento manual. |
| RN06   | A geração da conta a receber no Omie poderá ocorrer após a criação do assinante no Next, considerando datas de ativação e vencimento. |
| RN07   | O sistema deve evitar criação de dados inconsistentes ou duplicados em caso de falha parcial, registrando pendências com clareza para o operador. |
| RN08   | Somente propostas com status "aprovado" na API do DocSales devem ser consideradas para integração. |
| RN09   | Clientes já existentes em sistemas externos (Omie/Next) devem ter seus dados atualizados, não duplicados, durante o processo de integração. |
| RN10   | O lançamento de contas a receber no Omie para serviços contratados deve ser configurado com o atributo de recorrência, conforme a natureza do serviço VoIP. |
| RN11   | O sistema deve prevenir faturamento sem ativação de serviços e vice-versa. |
| RN12   | A unicidade do `ClienteSIG` é definida pela combinação **DocumentoFiscal**. |
| RN13   | O sistema deve verificar se um cliente já existe no Omie ou Next (usando DocumentoFiscal e ServidorNextId como chaves de busca quando apropriado) antes de criar um novo, ou identificá-lo para atualização. |
| RN14   | O SIGVoIP deve armazenar os IDs de referência do cliente em cada sistema (ID Omie, ID Next, ID DocSales) na entidade `ClienteSIG`. |
| RN15   | A seleção do servidor Next para um novo cliente deve ser feita com base na capacidade disponível (clientes/DIDs) e no `TipoServidor`. |
| RN16   | Propostas DocSales aprovadas com o mesmo `ProposalDocSalesID` não devem ser processadas múltiplas vezes (controle feito via `ProcessamentoProposta`). |

---

## 🎯 Critérios de Aceitação

| Código | Critério |
|--------|----------|
| CA01   | Todas as propostas aprovadas e não processadas devem ser corretamente listadas e identificáveis na interface. |
| CA02   | O sistema deve permitir que o usuário processe uma ou mais propostas com sucesso completo (cadastro no Next e Omie), com ou sem edição prévia. |
| CA03   | Deve haver validação impeditiva para duplicação de `ClienteSIG` com mesmo CPF/CNPJ. |
| CA04   | Cada integração (Next e Omie) deve gerar logs claros e acessíveis com status (sucesso ou erro). |
| CA05   | Os assinantes criados no Next devem conter o sufixo configurado conforme proposta, visível no nome. |
| CA06   | Usuários com perfil “Financeiro” não devem conseguir alterar configurações administrativas ou técnicas. |
| CA07   | Todas as falhas de integração devem ser registradas de forma persistente e visível ao operador, com mensagem descritiva. |
| CA08   | Ao final do processamento, o sistema deve exibir resumo consolidado com status, logs e links diretos para os registros gerados em Next e Omie. |