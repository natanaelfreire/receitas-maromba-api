using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceitasMarombaApi.Models
{
    public class IngredienteModel
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public ReceitaModel? Receita { get; set; }
        public int ReceitaId { get; set; }
    }
}