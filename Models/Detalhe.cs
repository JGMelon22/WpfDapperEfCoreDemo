namespace WpfDapperEfCoreDemo.Models;

public class Detalhe
{
	[Key]
	public int IdDetalhe { get; set; }
	public string DetalheTexto { get; set; } = string.Empty!;
	[ForeignKey(nameof(PessoaId))]
	public int PessoaId { get; set; }
	public Pessoa? Pessoa { get; set; }
}
