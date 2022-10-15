using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceitasMarombaApi.Models
{
    public class Receita
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Foto { get; set; }
        public bool RefeicaoLiquida { get; set; }
        public string? ModoPreparo { get; set; }
        public double Calorias { get; set; }
        public double Proteinas { get; set; }
        public double Carboidratos { get; set; }
        public double Gordura { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}