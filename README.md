# Sistema de Agendamento de Consultas - Healh&Med

## Introdução

Bem-vindo ao **sistema de agendamento de consultas da Health&Med**!

Este sistema foi desenvolvido para simplificar e digitalizar o processo de marcação de consultas médicas. Aqui, médicos e pacientes podem realizar todo o processo de cadastro, consulta de disponibilidade e agendamento de forma 100% online, rápida e segura.

Este documento tem como objetivo fornecer uma visão detalhada dos requisitos funcionais e não funcionais do projeto. Desde o cadastro de médicos e pacientes, até o agendamento das consultas e cadastro/edição dos horários disponíveis , a API busca oferecer uma experiência completa e personalizada.

Preparamos também uma descrição sucinta de como a autenticação e a segurança são tratadas, assim como a garantia de qualidade por meio do desenvolvimento de testes unitários e visibilidade com os relatórios de cobertura.

A seguir, você encontrará todas as instruções para utilizar o sistema, tanto para médicos quanto para pacientes.

---

---

---

## Requisitos Funcionais

### 1. Cadastro de Médicos

A API permite o cadastro de médicos que será realizado utilizando as seguintes informações:

- Nome - string
- CPF - string
- CRM (Cadastro no Conselho Regional de Medicina) - string
- Especialidade - string
- E-mail (usado para login e notificações) - string
- Senha - string

### 2. Cadastro de Pacientes

A API permite o cadastro de pacientes que será realizado utilizando as seguintes informações:

- Nome - string
- CPF - string
- E-mail - string
- Senha - string

### 3. Cadastro/Edição de Horários Disponíveis

Os médicos poderão cadastrar os horários em que estarão disponíveis para consultas:

1. Para a criação, será necessário ter as seguintes informações:
    - Número do CRM - string
    - Data completa - DateTime
    - Duração da consulta em minutos
2. Edição - Para editar horários, será necessário ter as seguintes informações:
    - Id do Agendamento - GUID
    - Nova data completa - DateTime
    - Duração da consulta em minutos
    

### 4. Busca por Médicos

Com o login feito, o paciente poderá procurar pelos médicos disponíveis pesquisando pelo número de registro (CRM) ou também poderá acessar os médicos disponíveis através da consulta por Médicos Disponíveis para o atendimento em uma determinada data.

### 5. Agendamento de Consultas

Para realizar o agendamento das consultas, deverá realizar os passos a seguir

1. Acessar a agenda do médico e visualizar os dias e horários disponíveis.
2. Escolher o ID do horário que mais se adeque às necessidades do paciente.
3. Confirmar o agendamento informando o Id do horário e o CPF do paciente. O sistema informará se o agendamento foi bem-sucedido e o horário será reservado exclusivamente para cada paciente.

### 6. Notificação de Consulta Marcada

Sempre que um paciente agendar uma consulta, o médico receberá uma notificação automática por e-mail com os seguintes detalhes:

- Título do e-mail: ”Health&Med - Nova consulta agendada”
- Corpo do e-mail:
    
    > ”Olá, Dr. {nome_do_médico}!
    Você tem uma nova consulta marcada!
    Paciente: {nome_do_paciente}.
    Data e horário: {data} às {horário_agendado}.”
    > 

## Requisitos Não Funcionais

### 1. Arquitetura Limpa e DDD

O projeto segue a arquitetura limpa, dividida em camadas, organizada conforme os princípios do DDD (Domain-Driven Design), garantindo uma arquitetura modular e coesa. 

As camadas incluem:

- **Apresentação (WebApi):** Consumers e Controllers.
- **Aplicação:** Lógica de aplicação e serviços.
- **Domínio:** Modelos de domínio e regras de negócios.
- **Infraestrutura:** Implementação de acesso a dados e integrações.

### 2. Testes Unitários

A aplicação possui uma cobertura abrangente de testes unitários, cobrindo partes críticas do código para garantir robustez e qualidade.

Além disso, a ferramenta ***Report Generator*** foi empregada para criar relatórios detalhados da cobertura de testes. Essa ferramenta oferece um relatório detalhado sobre a extensão da cobertura de testes, indicando áreas específicas do código que foram testadas. Esse relatório proporciona insights valiosos sobre a eficácia dos testes, ajudando a identificar lacunas na cobertura e garantindo uma abordagem mais abrangente.

### 3. Logs

O projeto está configurado para gerar logs, registrando atividades importantes em arquivo. Isso inclui informações sobre operações, erros e eventos relevantes para a aplicação.

### 4. Autenticação do Usuário

A API oferece um sistema para a autenticação de usuários, onde o token do tipo ***Bearer*** deve ser incluído nas chamadas dos endpoints para acessar recursos protegidos. Os usuários cadastrados podem se autenticar com base em dois tipos de roles: ***"medico"*** ou ***"paciente"***. 

O token de acesso é obtido por meio do endpoint de Login, onde os usuários fornecem suas credenciais (***e-mail*** e ***senha***). Após autenticação bem-sucedida, o token ***Bearer*** é gerado e deve ser incluído no cabeçalho das requisições.

A API valida a autenticação durante cada chamada em endpoint protegidos, verificando a presença e a validade do token ***Bearer***. Se o token não for válido ou expirar, a API retornará um status 401, indicando que o usuário não está autenticado.

### 5. Banco de Dados

A aplicação utiliza o MongoDB, um banco de dados NoSQL, para armazenar suas informações na nuvem. Essa escolha estratégica proporciona flexibilidade na modelagem de dados, sendo essencial para gerenciar as informações sobre os médicos, pacientes e agendamentos.

A arquitetura escalável do MongoDB garante eficiência e desempenho, enquanto a integração suave entre outros recursos asseguram a segurança e continuidade operacional da aplicação na nuvem. Em resumo, o MongoDB oferece uma solução robusta e adaptável para armazenamento e manipulação eficaz de dados.

### 6. Mensageria

A aplicação faz o uso do RabbitMQ como ferramenta de mensageria, possibilitando uma comunicação eficiente e assíncrona entre os diferentes componentes da aplicação. 

Essa escolha estratégica desse recurso foi necessária para atender ao requisito funcional 6 - Notificação de Consulta Marcada.

Um evento é enviado a uma fila do RabbitMQ chamada `notificacao-medico` e a intenção é que um serviço de e-mails esteja consumindo dessa fila para fazer o envio de e-mails.

Além disso, o RabbitMQ foi configurado para operar em um ambiente online e gratuito fornecido pela plataforma CloudAMQP, o que proporciona maior disponibilidade para a aplicação.

### 7. Processo de CI/CD

O processo de CI/CD (Integração Contínua e Entrega Contínua) é fundamental para garantir a qualidade e a entrega rápida de software e para automatizar esse processo, foi configurado um fluxo utilizando GitHub Actions no repositório GitHub da aplicação.

Esse fluxo permite a construção e deploy automáticos da aplicação a cada modificação no código-fonte, garantindo que as alterações sejam integradas e disponibilizadas de forma eficiente e segura. Após a conclusão bem-sucedida do processo de CI/CD, a aplicação é automaticamente publicada no portal do Azure, onde fica disponível para uso. 

Essa abordagem simplifica significativamente o ciclo de desenvolvimento, reduzindo o tempo necessário para implementar novas funcionalidades e correções, além de minimizar erros humanos durante o processo de deploy.

Neste sistema, a pipeline é configurada para ser acionada automaticamente ao completar um *Pull Request* para a *branch* `main`.

### 8. Validação de Conflito de Horários

Caso algum paciente selecione a opção de agendar em um horário que já foi agendado anteriormente, o sistema retornará uma mensagem de erro informando ao usuário que o apontamento já foi agendado.