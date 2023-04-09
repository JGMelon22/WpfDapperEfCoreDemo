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

	private static readonly Func<AppDbContext, IAsyncEnumerable<Pessoa>> AllPessoasAsync =
		EF.CompileAsyncQuery(
			(AppDbContext context) =>
				context.Pessoas);

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
	private static readonly Func<AppDbContext, List<Pessoa>> AllPessoasCompiled =
		EF.CompileQuery((AppDbContext context) =>
			context.Pessoas.ToList());

	public async Task<List<PessoaToListViewModel>> GetPessoasCompiledQuery()
	{
		var pessoasMapped = AllPessoasCompiled(_dbContext)
				.Select(x => new PessoaToListViewModel()
				{
					PessoaId = x.PessoaId,
					Nome = x.Nome
				}).ToList();

		return await Task.FromResult(pessoasMapped);
	}
}
