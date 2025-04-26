using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Log
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Level { get; set; } // Information, Warning, Error...

        public string Message { get; set; }

        public string Usuario { get; set; }

        public string RequestId { get; set; }

        public string Ip { get; set; }

        public string Endpoint { get; set; }

        public string Dados { get; set; } // JSON serializado

        public string StackTrace { get; set; }

        public int? ExecucaoMs { get; set; }

        public string Ambiente { get; set; }
    }
}
