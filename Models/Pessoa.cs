using System.Collections.Generic;

namespace WpfDapperEfCoreDemo.Models;

public class Pessoa
{
	public int PessoaId { get; set; }
	public string Nome { get; set; } = string.Empty!;
	List<Telefone> Telefones { get; set; }
	List<Detalhe> Detalhes { get; set; }
}
