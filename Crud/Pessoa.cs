using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Crud
{
    public class Pessoa : INotifyPropertyChanged
    {
        private string nome;
        private int idade;
        private long id;
        private DateTime nascimento;
        private bool temHabilitacao;
        private Sexo sexo;

        public long Id { get { return id; } set { id = value; OnPropertyChanged(); } }

        public string Nome { get { return nome; } set { nome = value; OnPropertyChanged(); } }

        public int Idade { get { return idade; } set { idade = value; OnPropertyChanged(); } }

        public DateTime Nascimento { get { return nascimento; } set { nascimento = value; OnPropertyChanged(); } }

        public bool TemHabilitacao { get { return temHabilitacao; } set { temHabilitacao = value; OnPropertyChanged(); } }

        public Sexo Sexo { get { return sexo; } set { sexo = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Sexo
    {
        Masculino,
        Feminino
    }
}