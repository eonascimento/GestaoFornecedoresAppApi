using GestaoFornecedoresApp.Business.Models;
using GestaoFornecedoresApp.Business.Interfaces.Respositories;
using GestaoFornecedoresApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFornecedoresApp.Data.Repositories
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(GestaoFornecedoresContext context) : base(context)
        {

        }
        public async Task<Fornecedor> ObterFornecedorProdutosEnderecos(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(c => c.Produtos)
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Fornecedor>> ObterFornecedoresEndereco()
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(e => e.Endereco)
                .ToListAsync();
        }
    }
}
