using System;
using System.Linq;
using System.Text;

namespace GestaoFornecedoresApp.Business.Notifications
{
    public class Notificacao
    {
        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;
        }
        public string Mensagem { get; }
    }
}
