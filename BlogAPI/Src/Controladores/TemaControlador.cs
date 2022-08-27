using BlogAPI.Src.Modelos;
using BlogAPI.Src.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogAPI.Src.Controladores
{
    [ApiController]
    [Route("api/Temas")]
    [Produces("application/json")]
    public class TemaControlador : ControllerBase
    {
        #region Atributos

        private readonly ITema _repositorio;

        #endregion


        #region Construtores

        public TemaControlador(ITema repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion


        #region Métodos

        /// <summary>
        /// Pegar todas os Temas
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna os temas</response>
        /// <response code="204">Nenhum tema cadastrado</response>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> PegarTodosTemasAsync()
        {
            var lista = await _repositorio.PegarTodosTemasAsync();
            if (lista.Count < 1) return NoContent();
            return Ok(lista);
        }

        /// <summary>
        /// Pegar Tema pelo Id
        /// </summary>
        /// <param name="idTema">Id do tema</param>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna o tema</response>
        /// <response code="404">Id não existente</response>
        [HttpGet("id/{idTema}")]
        [Authorize]
        public async Task<ActionResult> PegarTemaPeloIdAsync([FromRoute] int idTema)
        {
            try
            {
                return Ok(await _repositorio.PegarTemaPeloIdAsync(idTema));
            }
            catch (Exception ex)
            {
                return NotFound(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Criar novo Tema
        /// </summary>
        /// <param name="tema">Construtor para criar tema</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        /// POST /api/Temas
        /// {
        /// "descricao": "Programação"
        /// }
        ///
        /// </remarks>
        /// <response code="201">Retorna o tema criado</response>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> NovoTemaAsync([FromBody] Tema tema)
        {
            await _repositorio.NovoTemaAsync(tema);
            return Created($"api/Temas", tema);
        }

        /// <summary>
        /// Atualizar Tema
        /// </summary>
        /// <param name="tema">Contrutor para atualizar tema</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        /// POST /api/Usuarios/cadastrar
        /// {
        /// "id": "1",
        /// "descricao": "Linguagens de programação"
        /// }
        ///
        /// </remarks>
        /// <response code="200">Retorna o tema atualizado</response>
        /// <response code="400">Má requisição</response>
        [HttpPut]
        [Authorize(Roles ="ADMINISTRADOR")]
        public async Task<ActionResult> AtualizarTema([FromBody] Tema tema)
        {
            try
            {
                await _repositorio.AtualizarTemaAsync(tema);
                return Ok(tema);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Excluir Tema
        /// </summary>
        /// <param name="idTema">Id do tema</param>
        /// <returns>ActionResult</returns>
        /// <response code="204">Tema excluido</response>
        /// <response code="404">Id não existente</response>
        [HttpDelete("deletar/{idTema}")]
        [Authorize(Roles ="ADMINISTRADOR")]
        public async Task<ActionResult> DeletarTema([FromRoute] int idTema)
        {
            try
            {
                await _repositorio.DeletarTemaAsync(idTema);
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