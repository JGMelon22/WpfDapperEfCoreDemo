using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WpfDapperEfCoreDemo.Interfaces;
using WpfDapperEfCoreDemo.Models;
using WpfDapperEfCoreDemo.ViewModels;

namespace WpfDapperEfCoreDemo.Repositories;

public class PessoaRepository : IPessoaRepository
{
	private readonly IDbConnection _dbConnection;
	public PessoaRepository(IDbConnection dbConnection)
	{
		_dbConnection = dbConnection;
	}
	public async Task<List<PessoaToListViewModel>> GetPessoas()
	{
		var getPessoasQuery = @"SELECT PessoaId, 
                                       Nome 
                                FROM Pessoas;";

		_dbConnection.Open();

		var result = await _dbConnection.QueryAsync<Pessoa>(getPessoasQuery);
		var mappedResult = result.Select(x => new PessoaToListViewModel()
		{
			PessoaId = x.PessoaId,
			Nome = x.Nome
		}).ToList();

		_dbConnection.Close();

		return mappedResult;
	}
}
