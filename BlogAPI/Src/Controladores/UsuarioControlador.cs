using BlogAPI.Src.Modelos;
using BlogAPI.Src.Repositorios;
using BlogAPI.Src.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogAPI.Src.Controladores
{
    [ApiController]
    [Route("api/Usuarios")]
    [Produces("application/json")]
    public class UsuarioControlador : ControllerBase
    {
        #region Atributos

        private readonly IUsuario _repositorio;
        private readonly IAutenticacao _servicos;

        #endregion


        #region Construtores

        public UsuarioControlador(IUsuario repositorio, IAutenticacao servicos)
        {
            _repositorio = repositorio;
            _servicos = servicos;
        }

        #endregion


        #region Métodos

        /// <summary>
        /// Criar novo Usuario
        /// </summary>
        /// <param name="usuario">Contrutor para criar usuario</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        /// POST /api/Usuarios/cadastrar
        /// {
        /// "nome": "Gustavo",
        /// "email": "gustavo@email.com",
        /// "senha": "12345",
        /// "foto": "URLFOTO",
        /// "tipo": "NORMAL"
        /// }
        ///
        /// </remarks>
        /// <response code="201">Retorna usuario criado</response>
        /// <response code="401">E-mail ja cadastrado</response>
        [HttpPost("cadastrar")]
        [AllowAnonymous]
        public async Task<ActionResult> NovoUsuarioAsync([FromBody] Usuario usuario)
        {
            try
            {
                await _servicos.CriarUsuarioSemDuplicarAsync(usuario);
                return Created($"api/Usuarios/email/{usuario.Email}", usuario);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Pegar Autorização
        /// </summary>
        /// <param name="usuario">Construtor para logar usuario</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        /// POST /api/Usuarios/logar
        /// {
        /// "email": "gustavo@domain.com",
        /// "senha": "134652"
        /// }
        ///
        /// </remarks>
        /// <response code="201">Retorna usuario criado</response>
        /// <response code="401">E-mail ou senha invalido</response>
        [HttpPost("logar")]
        [AllowAnonymous]
        public async Task<ActionResult> LogarAsync([FromBody] Usuario usuario)
        {
            var auxiliar = await _repositorio.PegarUsuarioPeloEmailAsync(usuario.Email);
            if (auxiliar == null) return Unauthorized(new
            {
                Mensagem = "E-mail invalido"
            });
            if (auxiliar.Senha != _servicos.CodificarSenha(usuario.Senha))
                return Unauthorized(new { Mensagem = "Senha invalida" });
            var token = "Bearer " + _servicos.GerarToken(auxiliar);
            return Ok(new { Usuario = auxiliar, Token = token });
        }

        /// <summary>
        /// Pegar usuario pelo Email
        /// </summary>
        /// <param name="emailUsuario">E-mail do usuario</param>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna o usuario</response>
        /// <response code="404">Email não existente</response>
        [HttpGet("email/{emailUsuario}")]
        [Authorize(Roles ="NORMAL,ADMINISTRADOR")]
        public async Task<ActionResult> PegarUsuarioPeloEmailAsync([FromRoute] string emailUsuario)
        {
            var usuario = await _repositorio.PegarUsuarioPeloEmailAsync(emailUsuario);
            if (usuario == null) return NotFound(new
            {
                Mensagem = "Usuario não encontrado"
            });
            return Ok(usuario);
        }

        #endregion
    }
}