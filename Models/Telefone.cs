namespace WpfDapperEfCoreDemo.Models;

public class Telefone
{
	[Key]
	public int IdTelefone { get; set; }
	public string TelefoneTexto { get; set; } = string.Empty;
	[ForeignKey(nameof(PessoaId))]
	public int PessoaId { get; set; }
	public Pessoa? Pessoa { get; set; }
	public bool Ativo { get; set; }
}
