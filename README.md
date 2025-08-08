# SIG VoIP – Sistema de Integração e Gestão para Telefonia VoIP

[![Status](https://img.shields.io/badge/Status-Planejamento_Avançado_e_Design-yellow)](https://github.com/Leandro-Rogerio-TL/sig-voip)
[![Backend](https://img.shields.io/badge/Backend-.NET_8-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![Frontend](https://img.shields.io/badge/Frontend-TypeScript_|_Vue.js-brightgreen)](https://vuejs.org/)
[![Database](https://img.shields.io/badge/Database-MySQL-orange)](https://www.mysql.com/)

## 📌 Visão Geral

O **SIG VoIP** é um sistema de integração e gestão desenvolvido para automatizar e centralizar o fluxo de trabalho entre os sistemas **DocSales** (CRM), **Omie** (ERP) e **NextBilling/NextRouter** (plataformas VoIP). O projeto visa otimizar o processo pós-venda, reduzir retrabalho manual, evitar perdas financeiras e consolidar dados de forma segura, estruturada e unificada.

Para mais detalhes, confira a [Visão Geral Detalhada](/01_visao_geral.md).

## 🎯 Objetivos

- Automatizar o fluxo de propostas aprovadas entre DocSales, Omie e Next.
- Centralizar dados de clientes via entidade `ClienteSIG`, com governança e rastreabilidade.
- Reduzir erros manuais e tempo de ativação de serviços.
- Disponibilizar painel administrativo para acompanhamento e controle das integrações.
- Garantir segurança com autenticação/autorização robustas e controle de acesso por perfil.

## ⚙️ Funcionalidades da Fase 1

- Consulta periódica e manual de propostas aprovadas na API DocSales.
- Seleção manual de documentos para integração.
- Integração com **Omie**:
  - Cadastro e/ou atualização de clientes.
  - Criação de contas recorrentes via API.
- Integração com **NextBilling/NextRouter**:
  - Cadastro e/ou atualização de clientes.
  - Configuração automática de serviços.
  - Escolha automática de servidor Next com base na capacidade.
- Armazenamento centralizado de identificadores externos na entidade `ClienteSIG`.
- Registro de logs de sucesso e falha em integrações.
- Reprocessamento de propostas com erro.
- Autenticação (Identity + JWT) e autorização por perfil (Administrador, Financeiro).
- Painel administrativo básico para gerenciamento das integrações.

Para uma lista completa, veja os [Requisitos da Fase 1](/02_requisitos_fase_1.md).

## 🧱 Arquitetura

O SIG VoIP adota uma arquitetura em camadas, baseada nos princípios da **Clean Architecture** e **Hexagonal Architecture (Ports and Adapters)**, com o **Domínio** isolado de tecnologias externas. Adaptadores e conectores de APIs de terceiros são implementados na camada de Infraestrutura.

Para um mergulho mais profundo, consulte a seção de [Arquitetura no Guia Técnico Consolidado](/14_wiki_tecnica.md#3-tecnologias-e-arquitetura-detalhada).

## 🗃️ Modelo de Dados

O sistema utiliza entidades como `ClienteSIG`, `PedidoDocSales`, `UsuarioSIG` e `ServidorNext`, e Value Objects como `DocumentoFiscal`, `Endereco`, `Contato`, `Email`, `Telefone`, `CEP`. A modelagem relacional será implementada com **MySQL** e detalhada na infraestrutura.

Mais detalhes sobre a modelagem de dados estão na seção de [Modelagem do Domínio e Entidades Principais no Guia Técnico Consolidado](/14_wiki_tecnica.md#4-modelagem-do-domnio-e-entidades-principais).

## 🧪 Tecnologias

- **Backend:** .NET 8 (C#), EF Core (Pomelo MySQL), Microsoft Identity, JWT, AutoMapper, Serilog
- **Frontend:** TypeScript, Vue.js *(em planejamento)*
- **Banco de Dados:** MySQL
- **APIs Externas:** DocSales, Omie, NextBilling/NextRouter
- **Comunicação:** RESTful API (JSON)
- **Ferramentas:** Draw.io (diagramas), Markdown (documentação)

## 🛠️ Diretrizes de Desenvolvimento

O projeto adota boas práticas como DDD, SOLID (ênfase em DIP), Clean Code, Object Calisthenics, estilo funcional, padrões de projeto, testes automatizados, CI/CD e refatoração contínua.

Saiba mais sobre nossas [Diretrizes de Desenvolvimento no Guia Técnico Consolidado](/14_wiki_tecnica.md#2-diretrizes-de-desenvolvimento).

## 🧭 Metodologia

Adotamos a metodologia **Ágil** com fluxo **Kanban**, focando em entregas incrementais e priorização dinâmica com base em validação contínua.

## 🚧 Status Atual

- 🔄 **Fase atual:** Planejamento Avançado / Design Detalhado
- ✅ Requisitos da Fase 1 definidos
- ✅ Informações das APIs externas levantadas
- ✅ Arquitetura conceitual estruturada
- ✅ Modelagem do domínio inicial concluída

## 🗓️ Próximos Passos

A ordem priorizada das etapas pode ser consultada em [Próximos Passos do Projeto](/07_proximos_passos.md). O primeiro passo será a **Avaliação Final e Aprovação do Diagrama de Arquitetura Estrutural**.

## 🤝 Como Contribuir *(futuro)*

Se você estiver interessado em contribuir para o projeto, em breve publicaremos diretrizes detalhadas de configuração de ambiente, execução local, execução de testes e como submeter suas contribuições. Fique atento!

## 📞 Contato

Para dúvidas, sugestões ou informações adicionais, entre em contato com a equipe de desenvolvimento.

---

© Projeto desenvolvido por **Leandro Rogerio**