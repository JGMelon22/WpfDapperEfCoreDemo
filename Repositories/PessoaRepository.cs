using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WpfDapperEfCoreDemo.Data;
using WpfDapperEfCoreDemo.Interfaces;
using WpfDapperEfCoreDemo.Models;
using WpfDapperEfCoreDemo.ViewModels;

namespace WpfDapperEfCoreDemo.Repositories;

public class PessoaRepository : IPessoaRepository
{
	private readonly IDbConnection _dbConnection;
	private readonly AppDbContext _dbContext;
	public PessoaRepository(IDbConnection dbConnection, AppDbContext dbContext)
	{
		_dbConnection = dbConnection;
		_dbContext = dbContext;
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

	public async Task<List<PessoaToListViewModel>> GetPessoasEfCore()
	{
		var pessoas = await _dbContext.Pessoas.AsNoTracking().ToListAsync();
		var pessoasViewModel = pessoas.Select(x => new PessoaToListViewModel()
		{
			PessoaId = x.PessoaId,
			Nome = x.Nome
		}).ToList();

		return pessoasViewModel;
	}

	// Compiled Query
	private static readonly Func<AppDbContext, IAsyncEnumerable<Pessoa>> AllPessoasCompiled =
		EF.CompileAsyncQuery((AppDbContext context) =>
			context.Pessoas);

	public async Task<List<PessoaToListViewModel>> GetPessoasCompiledQuery()
	{
		var result = new List<PessoaToListViewModel>();

		await foreach (var pessoa in AllPessoasCompiled(_dbContext))
		{
			var viewModel = new PessoaToListViewModel
			{
				PessoaId = pessoa.PessoaId,
				Nome = pessoa.Nome
			};
			result.Add(viewModel);
		}

		return result;
	}
}
