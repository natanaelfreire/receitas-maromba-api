using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceitasMarombaApi.Models
{
    public class AvaliacaoModel
    {
        public int Id { get; set; }
        public int QuantidadeEstrelas { get; set; }
        public ReceitaModel? Receita { get; set; }
        public int ReceitaId { get; set; }
    }
}