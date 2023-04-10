using System.Collections.Generic;
using System.Threading.Tasks;
using WpfDapperEfCoreDemo.ViewModels;

namespace WpfDapperEfCoreDemo.Interfaces;

public interface IPessoaRepository
{
	Task<List<PessoaToListViewModel>> GetPessoas();
	// Task<List<PessoaToListViewModel>> GetPessoasJoinDapper();
	Task<IEnumerable<PessoaTelefoneDetalheToListViewModel>> GetPessoasJoinEfCore();
	Task<List<PessoaToListViewModel>> GetPessoasEfCore();
	Task<List<PessoaToListViewModel>> GetPessoasCompiledQuery();
}
