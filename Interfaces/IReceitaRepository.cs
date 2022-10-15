using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReceitasMarombaApi.Models;

namespace ReceitasMarombaApi.Interfaces
{
    public interface IReceitaRepository : ICrudRepository<ReceitaModel>
    {
        
    }
}