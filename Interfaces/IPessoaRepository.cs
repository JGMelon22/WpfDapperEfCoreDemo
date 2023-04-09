using System.Collections.Generic;
using System.Threading.Tasks;
using WpfDapperEfCoreDemo.Models;
using WpfDapperEfCoreDemo.ViewModels;

namespace WpfDapperEfCoreDemo.Interfaces;

public interface IPessoaRepository
{
	Task<List<PessoaToListViewModel>> GetPessoas();
	Task<List<PessoaToListViewModel>> GetPessoasEfCore();
	Task<List<Pessoa>> GetPessoasCompiledQuery();
}
