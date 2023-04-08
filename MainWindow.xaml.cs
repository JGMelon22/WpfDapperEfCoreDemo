using System.Windows;
using System.Windows.Controls;
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

	private void Window_Initialized(object sender, System.EventArgs e)
	{
		listBox1.Items.Clear();
	}

	private async void button1_Click(object sender, RoutedEventArgs e)
	{
		button1.IsEnabled = false;

		Mouse.OverrideCursor = Cursors.Wait;

		listBox1.Items.Clear();

		var pessoas = await _pessoaRepository.GetPessoas();

		foreach (var pessoa in pessoas)
			listBox1.Items.Add(pessoa.PessoaId + " - " + pessoa.Nome);

		Mouse.OverrideCursor = null;

		button1.IsEnabled = true;
	}

	private async void button2_Click(object sender, RoutedEventArgs e)
	{
		button2.IsEnabled = false;

		Mouse.OverrideCursor = Cursors.Wait;

		listBox1.Items.Clear();

		var pessoas = await _pessoaRepository.GetPessoasEfCore();

		foreach (var pessoa in pessoas)
			listBox1.Items.Add(pessoa.PessoaId + " - " + pessoa.Nome);

		Mouse.OverrideCursor = null;

		button2.IsEnabled = true;
	}


	private void button3_Click(object sender, RoutedEventArgs e)
	{
		button3.IsEnabled = false;

		Mouse.OverrideCursor = Cursors.Wait;

		listBox1.Items.Clear();

		Mouse.OverrideCursor = null;

		button3.IsEnabled = true;
	}
}
