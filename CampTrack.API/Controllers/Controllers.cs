using CampTrack.Application.Contracts;
using CampTrack.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CampTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;
    public CategoriasController(ICategoriaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoriaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateCategoriaDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly IEquipoService _service;
    public EquiposController(IEquipoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("disponibles")]
    public async Task<IActionResult> GetDisponibles() => Ok(await _service.GetDisponiblesAsync());

    [HttpGet("buscar")]
    public async Task<IActionResult> Search([FromQuery] string nombre) => Ok(await _service.SearchAsync(nombre));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEquipoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEquipoDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoService _service;
    public PrestamosController(IPrestamoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("activos")]
    public async Task<IActionResult> GetActivos() => Ok(await _service.GetActivosAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePrestamoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/devolver")]
    public async Task<IActionResult> Devolver(int id)
    {
        var updated = await _service.DevolverAsync(id);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}

[ApiController]
[Route("api/[controller]")]
public class RevisionesController : ControllerBase
{
    private readonly IRevisionService _service;
    public RevisionesController(IRevisionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("equipo/{equipoId}")]
    public async Task<IActionResult> GetByEquipo(int equipoId) => Ok(await _service.GetByEquipoAsync(equipoId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRevisionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
