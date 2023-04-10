namespace WpfDapperEfCoreDemo.ViewModels;

public class PessoaTelefoneDetalheToListViewModel
{
	public int PessoaId { get; set; }
	public string Nome { get; set; } = string.Empty!;
	public int IdTelefone { get; set; }
	public string TelefoneTexto { get; set; } = string.Empty;
	public bool Ativo { get; set; }
	public int IdDetalhe { get; set; }
	public string DetalheTexto { get; set; } = string.Empty!;
}
