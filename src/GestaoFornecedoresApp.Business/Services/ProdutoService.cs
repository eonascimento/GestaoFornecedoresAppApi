using GestaoFornecedoresApp.Business.Interfaces;
using GestaoFornecedoresApp.Business.Interfaces.Respositories;
using GestaoFornecedoresApp.Business.Interfaces.Services;
using GestaoFornecedoresApp.Business.Models;
using GestaoFornecedoresApp.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace GestaoFornecedoresApp.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoService(IProdutoRepository produtoRepository,
                              INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
        }
        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
            await _produtoRepository.Adicionar(produto);

        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
            await _produtoRepository.Atualizar(produto);
        }


        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);
        }
        public void Dispose()
        {
            _produtoRepository.Dispose();
        }
    }
}
