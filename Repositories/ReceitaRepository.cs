using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ReceitasMarombaApi.Factory;
using ReceitasMarombaApi.Interfaces;
using ReceitasMarombaApi.Models;
using Dapper;

namespace ReceitasMarombaApi.Repositories
{
  public class ReceitaRepository : IReceitaRepository
  {
    private readonly IDbConnection _connection;
    public ReceitaRepository()
    {
        _connection = new SqlFactory().SqlConnection();
    }

    public bool Create(ReceitaModel data)
    {
        var query = @"INSERT INTO [tb_receitas]
        OUTPUT inserted.id
        VALUES
        (
            @titulo,
            @subtitulo,
            @fotoUrl,
            @refeicaoLiquida,
            @modoPreparo,
            @rendeQuanto,
            @calorias,
            @proteinas,
            @carboidratos,
            @gorduras,
            @dataCriacao
        )
        ";

        var parametros = new
        {
            titulo = data.Titulo,
            subtitulo = data.Subtitulo,
            fotoUrl = data.Foto,
            refeicaoLiquida = data.RefeicaoLiquida ? 1 : 0,
            modoPreparo = data.ModoPreparo,
            rendeQuanto = data.RendeQuanto,
            calorias = data.Calorias,
            proteinas = data.Proteinas,
            carboidratos = data.Carboidratos,
            gorduras = data.Gorduras,
            dataCriacao = DateTime.Now
        };

        var id = 0;

        using (_connection) 
        {
            _connection.Open();

            using (var tran = _connection.BeginTransaction())
            {
                id = _connection.QueryFirstOrDefault<int>(query, parametros, transaction: tran);

                if (data.Ingredientes != null && data.Ingredientes.Count > 0) 
                {
                    foreach (var ingrediente in data.Ingredientes)
                    {
                        var queryIngrediente = @"INSERT INTO [tb_ingredientes]
                        VALUES
                        (
                            @descricao,
                            @receitaId
                        )
                        ";

                        var parametrosIngrediente = new { descricao = ingrediente, receitaId = id };
                        _connection.Execute(queryIngrediente, parametrosIngrediente, transaction: tran);
                    }
                }

                tran.Commit();
            }
            
            _connection.Close();
        }

        return (id > 0 ? true : false);
    }

    public bool Delete(int id)
    {
        var query = @"DELETE 
        FROM [tb_receitas]
        WHERE [id] = @receitaId
        ";

        var queryIngrediente = @"DELETE 
        FROM [tb_ingredientes]
        WHERE [cl_receita_id] = @receitaId
        ";

        var queryAvaliacoes = @"DELETE 
        FROM [tb_avaliacoes]
        WHERE [cl_receita_id] = @receitaId
        ";

        var parametros = new { receitaId = id };
        var result = 0;

        using (_connection)
        {
            _connection.Open();

            using (var tran = _connection.BeginTransaction())
            {
                _connection.Execute(queryIngrediente, parametros, transaction: tran);
                _connection.Execute(queryAvaliacoes, parametros, transaction: tran);
                result = _connection.Execute(query, parametros, transaction: tran);

                tran.Commit();
            }

            _connection.Close();
        }

        return (result > 0 ? true : false);
    }

    public IEnumerable<ReceitaModel> GetAll()
    {
        var receitas = new List<ReceitaModel>();
        var query = @"SELECT 
        [id],
        [cl_titulo] AS Titulo,
        [cl_subtitulo] AS Subtitulo,
        [cl_foto] AS Foto,
        CASE
            WHEN [cl_refeicao_liquida] = 1 THEN 'true'
            ELSE 'false'
        END
        AS RefeicaoLiquida,
        [cl_modo_preparo] AS ModoPreparo,
        [cl_rende_quanto] AS RendeQuanto,
        [cl_calorias] AS Calorias,
        [cl_proteinas] AS Proteinas,
        [cl_carboidratos] AS Carboidratos,
        [cl_gorduras] AS Gorduras,
        [cl_created_at] AS DataCriacao
        FROM [tb_receitas]";

      using(_connection) 
      {
        receitas = _connection.Query<ReceitaModel>(query).ToList();
      }

      return receitas;
    }

    public ReceitaModel? GetOne(int id)
    {
        ReceitaModel receita;
        var query = @"SELECT
        [id],
        [cl_titulo] AS Titulo,
        [cl_subtitulo] AS Subtitulo,
        [cl_foto] AS Foto,
        CASE
            WHEN [cl_refeicao_liquida] = 1 THEN 'true'
            ELSE 'false'
        END
        AS RefeicaoLiquida,
        [cl_modo_preparo] AS ModoPreparo,
        [cl_rende_quanto] AS RendeQuanto,
        [cl_calorias] AS Calorias,
        [cl_proteinas] AS Proteinas,
        [cl_carboidratos] AS Carboidratos,
        [cl_gorduras] AS Gorduras,
        [cl_created_at] AS DataCriacao
        FROM [tb_receitas]
        WHERE 
        [id] = @receitaId
        ";

        var queryIngredientes = @"SELECT
        [cl_descricao] AS Descricao
        FROM [tb_ingredientes]
        WHERE
        [cl_receita_id] = @receitaId
        ";

        var parametros = new {
            receitaId = id
        };

        using (_connection) 
        {
            receita = _connection.QueryFirstOrDefault<ReceitaModel>(query, parametros);

            if (receita != null)
                receita.Ingredientes = _connection.Query<string>(queryIngredientes, parametros).ToList();
        }

        return receita;
    }

    public bool Update(int id, ReceitaModel data)
    {
        var query = @"UPDATE [tb_receitas]
        SET
        [cl_subtitulo] = @subtitulo,
        [cl_foto] = @fotoUrl,
        [cl_refeicao_liquida] = @refeicaoLiquida,
        [cl_modo_preparo] = @modoPreparo,
        [cl_rende_quanto] = @rendeQuanto,
        [cl_calorias] = @calorias,
        [cl_proteinas] = @proteinas,
        [cl_carboidratos] = @carboidratos,
        [cl_gorduras] = @gorduras
        WHERE
        [id] = @receitaId
        ";

        var parametros = new
        {
            titulo = data.Titulo,
            subtitulo = data.Subtitulo,
            fotoUrl = data.Foto,
            refeicaoLiquida = data.RefeicaoLiquida ? 1 : 0,
            modoPreparo = data.ModoPreparo,
            rendeQuanto = data.RendeQuanto,
            calorias = data.Calorias,
            proteinas = data.Proteinas,
            carboidratos = data.Carboidratos,
            gorduras = data.Gorduras,
            receitaId = id
        };

        var result = _connection.Execute(query, parametros);

        return (result > 0 ? true : false);
    }
  }
}