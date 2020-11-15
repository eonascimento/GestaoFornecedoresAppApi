using GestaoFornecedoresApp.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFornecedoresApp.Business.Interfaces.Respositories
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);
        Task<Fornecedor> ObterFornecedorProdutosEnderecos(Guid id);
        Task<List<Fornecedor>> ObterFornecedoresEndereco();
    }
}
