
namespace worker_estudos;
using System;
	public class Scheduler : IScheduler
	{
		//armazeno jobs na fila de prioridade com relação a dataHora
		private PriorityQueue<Job, DateTime> jobs = new PriorityQueue<Job, DateTime>();

		//condicao de corrida
		private readonly object lockObject = new object();
		
		// proximo a ser processado (data hora)
		private DateTime proximoProcessado = DateTime.MinValue;

		//source para cancelar e criar token
		private CancellationTokenSource? source=null;

		//token para definir o delay a ser iniciado ou cancelado
		private CancellationToken token;

		public Scheduler()
		{
		}

		public void AddJob(Job job)
		{

            Console.WriteLine("[SCHEDULE] ADD JOB");

            // permito acesso a fila de prioridade 1 por vez
            lock (lockObject)
			{
				this.jobs.Enqueue(job, job.data);  //11:30 12:00
				
				if (job.data<this.proximoProcessado  && source != null) //horario menor ao proximo a ser chamado  E existir token de Delay
				{
					// cancelationToken do delay
					this.source.Cancel();
				}
			}
		}

		//evento que ocorre de Delay e CancelTokenDelay
		public async Task Escalonar(CancellationToken stoppingToken)
		{
            Console.WriteLine("[SCHEDULE] ESCALONAR");

			while (!stoppingToken.IsCancellationRequested)
			{
				DateTime proximoHorario = DateTime.MinValue;

				// permito acesso a fila de prioridade 1 por vez
				Job? tarefaWork=null;

				// lock - unlock
				lock (lockObject)
				{
					// verificar se está vazia
					if (this.jobs.Count != 0) {
						Job job = jobs.Peek();
						if (job.data <= DateTime.Now) //tem de verificar o status (neste caso nao, mas é bom lembrar)
						{
							//desempilho e mudo status
							job.StatusExecutar(); //muda status
							tarefaWork = jobs.Dequeue(); //tira da fila de prioridade

							//pego o proximo para salvar o horario/data a ser processado
							if (this.jobs.Count != 0) 
							{ 
								job = jobs.Peek();

                            }
						}

						//se existir um token e source já instanciado, para evitar ter mais de um dele
						if (source != null)
						{
							this.source.Cancel(); // mudar status do token para cancelado (apenas para garantir)
							this.source.Dispose(); // libera o objeto (nao vou usar mais essa instancia)
						}

						// novo source e token, porque haverá delay e cancelamento
                        this.source = new CancellationTokenSource();
                        this.token = this.source.Token;

						this.proximoProcessado = job.data;
						proximoHorario = job.data; //evitar abrir outro lock
						
					}
					else
					{
                        // novo source e token, porque haverá delay e cancelamento 
						// e precisa ter algo pra cancelar o recesso quando a fila de prioridade estiver vazia
                        this.source = new CancellationTokenSource();
                        this.token = this.source.Token;

                        DateTime recesso = DateTime.Now.AddSeconds(5);
						this.proximoProcessado = recesso;
                        proximoHorario = recesso;

					} 
				}

				if (tarefaWork != null) {
					//ESSE É O EXECUTAR REAL DO EXMEPLO
					//mandando pro worker (exemplo) / executo
					Console.WriteLine(tarefaWork.ToString());
				}

				// para a proxima rodada do while ate (aparecer um novo) ou (horario do proximo ser processado)
				TimeSpan TempoProximo = proximoHorario.Subtract(DateTime.Now); 

				// se for negativo é porque a próxima ação ainda é antes de DateTime.Now
				if (TempoProximo.TotalMilliseconds > 0) {
					try
					{
						await Task.Delay((int)TempoProximo.TotalMilliseconds,this.token);  //token adicionado para permitir sem cancelado o delay
					}catch(Exception e)
					{
						Console.WriteLine("CATCH DO DELAY"+e.Message);
					}
				}
			}
		}

	}


