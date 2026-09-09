# Scheduler .NET

Projeto desenvolvido para estudar e compreender conceitos relacionados a **processamento assíncrono, concorrência e execução de tarefas agendadas** utilizando C# e .NET.

A proposta foi construir o Scheduler de forma incremental, entendendo cada decisão antes de avançar para a próxima etapa.

## Objetivo

Criar um Scheduler capaz de receber Jobs e organizá-los de acordo com o horário em que devem ser processados.

O projeto também busca compreender como diferentes recursos do .NET podem trabalhar juntos em um cenário de processamento em segundo plano.

## Conceitos estudados

* C#
* .NET
* `PriorityQueue`
* `Task.Delay`
* `CancellationToken`
* `CancellationTokenSource`
* `lock` e condições de corrida
* `BackgroundService`
* Injeção de Dependência (DI)
* Singleton
* Interfaces e abstração
* Processamento assíncrono

## Funcionamento

O fluxo básico do projeto é:

```text
Job
 │
 ▼
Scheduler
 │
 ├── Organiza os Jobs por horário
 │
 ├── Identifica o próximo Job
 │
 ├── Aguarda até o momento de execução
 │
 └── Recalcula a espera caso um Job mais próximo seja adicionado
```

A `PriorityQueue` é utilizada para manter os Jobs organizados de acordo com sua prioridade, neste caso representada pelo horário de execução.

Quando não há um Job pronto para execução, o Scheduler utiliza `Task.Delay` para aguardar sem manter uma thread ocupada continuamente.

Caso um novo Job seja adicionado com horário de execução anterior ao próximo Job da fila, o período de espera pode ser interrompido utilizando `CancellationTokenSource`, permitindo que o Scheduler reavalie a fila.

##  Concorrência

Como diferentes fluxos podem acessar a fila de Jobs simultaneamente, o projeto utiliza `lock` para proteger operações críticas sobre o estado compartilhado.

O objetivo é evitar condições de corrida durante operações como:

* adicionar Jobs;
* consultar o próximo Job;
* remover Jobs da fila.

##  BackgroundService

O Scheduler é executado através de um `BackgroundService`.

Isso permite manter um processo de longa duração gerenciado pelo Host do .NET, separando o trabalho do Scheduler do fluxo principal da aplicação.

> `BackgroundService` não significa necessariamente criar uma thread exclusiva. O trabalho é executado de forma assíncrona e operações como `Task.Delay` não mantêm uma thread ocupada durante todo o período de espera.

##  Injeção de Dependência

O projeto utiliza Injeção de Dependência para separar as responsabilidades e permitir que o `BackgroundService` utilize o Scheduler através de uma interface.

A implementação do Scheduler é registrada como **Singleton**, permitindo que os diferentes fluxos dentro do mesmo processo compartilhem a mesma instância e, consequentemente, a mesma fila de Jobs.

##  Próximos passos

Este projeto faz parte de uma sequência de estudos sobre processamento assíncrono e arquitetura de aplicações.

Os próximos passos planejados são:

* estudar RabbitMQ de forma isolada;
* publicação e consumo de mensagens;
* ACK;
* tratamento de falhas;
* retry;
* Dead Letter Queue (DLQ);
* múltiplos Workers;
* controle de concorrência;
* prefetch;
* observabilidade;
* integração entre Scheduler e RabbitMQ.

A ideia é evoluir cada etapa separadamente para compreender não apenas **como implementar**, mas também **quando cada recurso realmente faz sentido em um sistema real**.

##  Contexto

Este repositório foi desenvolvido como parte dos meus estudos de **Back-End, .NET, processamento assíncrono, concorrência e arquitetura de aplicações**.

O projeto prioriza o aprendizado dos conceitos e das decisões de implementação, servindo como base para estudos posteriores envolvendo mensageria e processamento distribuído.
