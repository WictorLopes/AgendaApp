using Agenda.Domain.Entities;
using Agenda.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var contatos = await _contatoRepository.ObterTodosAsync();
            return Ok(contatos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var contato = await _contatoRepository.ObterPorIdAsync(id);
            if (contato == null)
                return NotFound();

            return Ok(contato);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Contato contato)
        {
            await _contatoRepository.AdicionarAsync(contato);
            return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, contato);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Contato contato)
        {
            var existe = await _contatoRepository.ObterPorIdAsync(id);
            if (existe == null)
                return NotFound();

            contato.Id = id;
            await _contatoRepository.AtualizarAsync(contato);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _contatoRepository.RemoverAsync(id);
            return NoContent();
        }
    }
}
