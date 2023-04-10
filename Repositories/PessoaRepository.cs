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

	public async Task<IEnumerable<PessoaTelefoneDetalheToListViewModel>> GetPessoasJoinEfCore()
	{
		var pessoasDetalhesTelefones = await (from p in _dbContext.Pessoas
											  join d in _dbContext.Detalhes on p.PessoaId equals d.PessoaId
											  join t in _dbContext.Telefones on p.PessoaId equals t.PessoaId
											  select new PessoaTelefoneDetalheToListViewModel()
											  {
												  PessoaId = p.PessoaId,
												  Nome = p.Nome,
												  TelefoneTexto = t.TelefoneTexto,
												  Ativo = t.Ativo,
												  DetalheTexto = d.DetalheTexto
											  })
									.Select(x => new PessoaTelefoneDetalheToListViewModel()
									{
										PessoaId = x.PessoaId,
										Nome = x.Nome,
										TelefoneTexto = x.TelefoneTexto,
										Ativo = x.Ativo,
										DetalheTexto = x.DetalheTexto
									}).Take(500).AsNoTracking().ToListAsync();

		return pessoasDetalhesTelefones;
	}

	public async Task<List<PessoaTelefoneDetalheToListViewModel>> GetPessoasJoinDapper()
	{
		#region
		// Ordinary dapper join method
		var getPessoasTelefonesQuery = @"SELECT TOP 500 p.PessoaId,
										 		        p.Nome,
										 		        t.TelefoneId,
										 		        t.TelefoneTexto,
										 		        t.PessoaId,
										 		        t.Ativo,
														d.DetalheId,
										 			    d.DetalheTexto
										 FROM Pessoas p
										 INNER JOIN Telefones t
										 	ON p.PessoaId = t.PessoaId
										 INNER JOIN Detalhes d
										 	ON p.PessoaId = d.PessoaId
										 ORDER BY p.PessoaId ASC;";

		_dbConnection.Open();

		var pessoas = await _dbConnection.QueryAsync<Pessoa, Telefone, Detalhe, Pessoa>(getPessoasTelefonesQuery, (pessoa, telefone, detalhe) =>
		{
			// Verifica se o Objeto telefone foi inicializado, se não, inicia
			if (pessoa.Telefones == null)
			{
				pessoa.Telefones = new List<Telefone>();
			}

			if (pessoa.Detalhes == null)
			{
				pessoa.Detalhes = new List<Detalhe>();
			}

			pessoa.Telefones.Add(telefone);
			pessoa.Detalhes.Add(detalhe);
			return pessoa;
		},

		splitOn: "TelefoneId, DetalheId");
		// Till here, than we map :D
		#endregion
		var result = pessoas.Select(x => new PessoaTelefoneDetalheToListViewModel()
		{
			PessoaId = x.PessoaId,
			Nome = x.Nome,
			TelefoneTexto = x.Telefones.Select(x => x.TelefoneTexto).FirstOrDefault(),
			Ativo = x.Telefones.Select(x => x.Ativo).FirstOrDefault(),
			DetalheTexto = x.Detalhes.Select(x => x.DetalheTexto).FirstOrDefault(),
			IdDetalhe = x.Detalhes.Select(x => x.IdDetalhe).FirstOrDefault(),
			IdTelefone = x.Telefones.Select(x => x.IdTelefone).FirstOrDefault()
		}).ToList();

		_dbConnection.Close();

		return result;

	}
}