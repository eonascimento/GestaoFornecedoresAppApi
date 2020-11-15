using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using GestaoFornecedoresApp.Api.ViewModels;
using GestaoFornecedoresApp.Business.Interfaces;
using GestaoFornecedoresApp.Business.Interfaces.Respositories;
using GestaoFornecedoresApp.Business.Interfaces.Services;
using GestaoFornecedoresApp.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFornecedoresApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _ProdutoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(
            IProdutoRepository oProdutoRepository,
            IProdutoService produtoService,
            IMapper mapper,
            INotificador notificador) : base (notificador)
        {
            _ProdutoRepository = oProdutoRepository;
            _produtoService = produtoService;
            _mapper = mapper;   
        }
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
        {
            var produtosViewModel =_mapper.Map<List<ProdutoViewModel>>(await _ProdutoRepository.ObterTodos());

            if (produtosViewModel == null) return NotFound();

            return CustomResponse(produtosViewModel);
        }

        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produtoViewModel = await ObterPorId(id);
            if (produtoViewModel == null) return NotFound();
            return produtoViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
            if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustomResponse(produtoViewModel);
            }

            produtoViewModel.Imagem = imagemNome;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));
            return CustomResponse();
        }

        //[HttpPost]
        //public async Task<ActionResult<ProdutoViewModel>> AdicionarAlternativo(ProdutoViewModel produtoViewModel)
        //{
        //    if (!ModelState.IsValid) return CustomResponse(ModelState);

        //    var imgPrefixo = Guid.NewGuid() + "_";
        //    if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, imgPrefixo))
        //    {
        //        return CustomResponse(produtoViewModel);
        //    }

        //    produtoViewModel.Imagem = imgPrefixo+produtoViewModel.ImagemUpload.FileName;
        //    await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));
        //    return CustomResponse();
        //}

        [HttpDelete("id:guid")]
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produto = await ObterProduto(id);
            if (produto == null) return NotFound();
            await _produtoService.Remover(id);
            return CustomResponse(produto);
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _ProdutoRepository.ObterProdutoFornecedor(id));
        }
        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem para este produto");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imgPrefixo,
                arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com esse nome!");
                return false;
            }

            using (var stream = new FileStream(path,FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }
            return true;
        }

        public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                NotificarErro("Os Ids são distintos!");
                return CustomResponse();
            }

            var produtoAtualizacao = await ObterProduto(id);
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imageNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if(!UploadArquivo(produtoViewModel.ImagemUpload,imageNome))
                {
                    return CustomResponse(ModelState);
                }

                produtoViewModel.Imagem = imageNome;
            }

            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(produtoViewModel);
        }
    }
}
