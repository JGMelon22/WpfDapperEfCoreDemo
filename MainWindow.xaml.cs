using System.Windows;
using System.Windows.Input;
using WpfDapperEfCoreDemo.Interfaces;

namespace WpfDapperEfCoreDemo;

public partial class MainWindow : Window
{
	private readonly IPessoaRepository _pessoaRepository;
	public MainWindow(IPessoaRepository pessoaRepository)
	{
		InitializeComponent();
		_pessoaRepository = pessoaRepository;
	}

	private async void button1_Click(object sender, RoutedEventArgs e)
	{
		button1.IsEnabled = false;

		Mouse.OverrideCursor = Cursors.Wait;

		listBox1.Items.Clear();

		var pessoas = await _pessoaRepository.GetPessoas();

		foreach (var pessoa in pessoas)
			listBox1.Items.Add(pessoa.PessoaId + " - " + pessoa.Nome);

		Mouse.OverrideCursor = Cursors.No;

		button1.IsEnabled = true;
	}
}
