using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Auditer;
using AutoMapper;
using Crud.Migrations;

namespace Crud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Contexto contexto;
        private readonly ViewModel dtx;

        public MainWindow()
        {
            InitializeComponent();
            Mapper.CreateMap<Pessoa, Pessoa>();
            contexto = new Contexto();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Contexto,Configuration>());
            dtx = new ViewModel { Pessoas = contexto.Pessoas.ToList(), Log = "LOG INIT\n" };
            Action<AuditEntry> log = entry => dtx.Log += entry.ToString() + Environment.NewLine;
            DbInterception.Add(new AuditingCommandInterceptor(log, s => dtx.Log += s + Environment.NewLine));
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = dtx;
        }

        private void DataGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var pessoa = e.Row.DataContext as Pessoa;
            if (e.Row.IsNewItem)
            {
                contexto.Pessoas.Add(pessoa);
            }
            else if (e.Row.IsNewItem == false)
            {
                var pessoaBanco = contexto.Pessoas.Find(pessoa.Id);
                Mapper.Map(pessoa, pessoaBanco);
            }
            contexto.SaveChanges();
        }

        private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var dataGrid = (DataGrid)sender;
                var pessoa = (Pessoa)dataGrid.SelectedItem;
                contexto.Pessoas.Remove(contexto.Pessoas.Find(pessoa.Id));
                contexto.SaveChanges();
            }
        }
    }
}
