using BlogAPI.Src.Modelos;
using BlogAPI.Src.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogAPI.Src.Controladores
{
    [ApiController]
    [Route("api/Postagens")]
    [Produces("application/json")]
    public class PostagemControlador : ControllerBase
    {
        #region Atributos

        private readonly IPostagem _repositorio;
        
        #endregion
        
        
        #region Construtores
        
        public PostagemControlador(IPostagem repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion


        #region Métodos

        /// <summary>
        /// Pegar todas as Postagens
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna as postagens</response>
        /// <response code="204">Nenhuma postagem criada</response>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> PegarTodasPostagensAsync()
        {
            var lista = await _repositorio.PegarTodasPostagensAsync();
            if (lista.Count < 1) return NoContent();
            return Ok(lista);
        }

        /// <summary>
        /// Pegar Postagem pelo Id
        /// </summary>
        /// <param name="idPostagem">Id da postagem</param>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna a postagem</response>
        /// <response code="404">Id não existente</response>
        [HttpGet("id/{idPostagem}")]
        [Authorize]
        public async Task<ActionResult> PegarPostagemPeloIdAsync([FromRoute] int idPostagem)
        {
            try
            {
                return Ok(await _repositorio.PegarPostagemPeloIdAsync(idPostagem));
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Criar nova Postagem
        /// </summary>
        /// <param name="postagem">Construtor para criar postagem</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        /// POST /api/Temas
        /// {
        /// "titulo": "Um dia na vida",
        /// "descricao": "Um pouco mais...",
        /// "foto": "URLFOTO",
        /// "criador": {
        ///         "id": 1
        ///     },
        /// "tema": {
        ///         "id": 1
        ///     }
        /// }
        ///
        /// </remarks>
        /// <response code="201">Retorna a postagem criada</response>
        /// <response code="400">Má requisição</response>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> NovaPostagemAsync([FromBody] Postagem postagem)
        {
            try
            {
                await _repositorio.NovaPostagemAsync(postagem);
                return Created($"api/Postagens", postagem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Atualizar Postagem
        /// </summary>
        /// <param name="postagem">Contrutor para atualizar postagem</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        /// POST /api/Usuarios/cadastrar
        /// {
        /// "titulo": "Outro dia na vida",
        /// "descricao": "Um pouco menos...",
        /// "foto": "URLFOTO",
        /// "tema": {
        ///         "id": 2
        ///     }
        /// }
        ///
        /// </remarks>
        /// <response code="200">Retorna a postagem atualizado</response>
        /// <response code="400">Má requisição</response>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> AtualizarPostagemAsync([FromBody] Postagem postagem)
        {
            try
            {
                await _repositorio.AtualizarPostagemAsync(postagem);
                return Ok(postagem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Excluir Postagem
        /// </summary>
        /// <param name="idPostagem">Id do postagem</param>
        /// <returns>ActionResult</returns>
        /// <response code="204">Postagem excluida</response>
        /// <response code="404">Id não existente</response>
        [HttpDelete("deletar/{idPostagem}")]
        [Authorize]
        public async Task<ActionResult> DeletarPostagem([FromRoute] int idPostagem)
        {
            try
            {
                await _repositorio.DeletarPostagemAsync(idPostagem);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }

        #endregion
    }
}