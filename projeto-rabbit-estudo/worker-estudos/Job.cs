using System;

namespace worker_estudos;

	public class Job
	{
		public string conteudo { get; private set; }
		public int id { get; private set; }
		public DateTime data { get; private set; }
		public bool executada { get; private set; }

		public Job(string conteudo, int id)
		{
			this.conteudo = conteudo;
			this.id = id;
			if (id % 2 == 0) 
			{ 
				this.data = DateTime.Now.AddSeconds(-(id*10));
			}
			else
			{
                this.data = DateTime.Now.AddSeconds(id*10);
            }
            this.executada = false;
		}

		public void StatusExecutar()
		{
			this.executada = true;
		}
		
		public override string ToString()
		{
			return "[conteudo: " + this.conteudo + "]\n[id: "+this.id+"]\n";
		}

		public static List<Job> CriandoSituacao()
		{
            List<Job> listaJob = new List<Job>();
            for (int i = 0; i < 10; i++)
            {
                listaJob.Add(new Job("t" + i, i));
            }

            //foreach (var job in listaJob)
            //{
           //     Console.WriteLine(job.ToString());
           // }

            return listaJob;
        }
	}
