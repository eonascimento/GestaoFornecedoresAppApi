using GestaoFornecedoresApp.Business.Models;
using GestaoFornecedoresApp.Business.Interfaces.Respositories;
using GestaoFornecedoresApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFornecedoresApp.Data.Repositories
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(GestaoFornecedoresContext context) : base(context)
        {

        }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
        }
    }
}
