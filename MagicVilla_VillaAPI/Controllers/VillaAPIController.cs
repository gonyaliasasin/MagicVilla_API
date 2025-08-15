using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{

    //private readonly ILogger<VillaAPIController> _logger;

    //public VillaAPIController(ILogger<VillaAPIController> logger)
    //{
    //    _logger = logger;
    //}

    private readonly ApplicationDbContext _db;

    public VillaAPIController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        //_logger.LogInformation("Getting all villas");
        return Ok(await _db.Villas.ToListAsync());
    }


    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VillaDTO>> GetVilla(int id)
    {

        if (id == 0)
        {
            //_logger.LogError("Get Villa Error with Id " + id);
            return BadRequest();
        }

        var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
        if (villa == null)
        {
            return NotFound();
        }

        return Ok(villa);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
    {

        if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
        {
            ModelState.AddModelError("Custom Error", "Villa already exists!");
            return BadRequest(ModelState);
        }

        if (villaDTO == null)
        {
            return BadRequest(villaDTO);
        }

        //if (villaDTO.Id > 0)
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError);
        //}

        Villa model = new()
        {
            Amenity = villaDTO.Amenity,
            Details = villaDTO.Details,
            ImageUrl = villaDTO.ImageUrl,
            Name = villaDTO.Name,
            Occupancy = villaDTO.Occupancy,
            Rate = villaDTO.Rate,
            Sqft = villaDTO.Sqft
        };

        await _db.Villas.AddAsync(model);
        await _db.SaveChangesAsync();

        return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
    }


    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVilla(int id)
    {

        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

        if (villa == null)
        {
            return NotFound();
        }

        _db.Villas.Remove(villa);
        await _db.SaveChangesAsync();
        return NoContent();

    }


    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
    {

        if (villaDTO == null || id != villaDTO.Id)
        {
            return BadRequest();
        }

        //var item = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
        //if (item is not null)
        //{
        //    item.Amenity= villaDTO.Amenity;
        //    item.Details = villaDTO.Details;
        //    item.Id = villaDTO.Id;
        //    item.ImageUrl = villaDTO.ImageUrl;
        //    item.Name = villaDTO.Name;
        //    item.Occupancy = villaDTO.Occupancy;
        //    item.Rate = villaDTO.Rate;
        //    item.Sqft= villaDTO.Sqft;
        //    item.UpdatedDate = DateTime.UtcNow;
        //    item.CreatedDate = DateTime.UtcNow;
        //}


        //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        //villa.Name = villaDTO.Name;
        //villa.Sqft= villaDTO.Sqft;
        //villa.Occupancy = villaDTO.Occupancy;

        Villa model = new()
        {
            Amenity = villaDTO.Amenity,
            Details = villaDTO.Details,
            Id = villaDTO.Id,
            ImageUrl = villaDTO.ImageUrl,
            Name = villaDTO.Name,
            Occupancy = villaDTO.Occupancy,
            Rate = villaDTO.Rate,
            Sqft = villaDTO.Sqft
        };

        _db.Villas.Update(model );
        await _db.SaveChangesAsync();
        return NoContent();
    }


    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        VillaUpdateDTO villaDTO = new()
        {
            Amenity = villa.Amenity,
            Details = villa.Details,
            Id = villa.Id,
            ImageUrl = villa.ImageUrl,
            Name = villa.Name,
            Occupancy = villa.Occupancy,
            Rate = villa.Rate,
            Sqft = villa.Sqft
        };

        if (villa == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(villaDTO, ModelState);

        Villa model = new Villa()
        {
            Amenity = villaDTO.Amenity,
            Details = villaDTO.Details,
            Id = villaDTO.Id,
            ImageUrl = villa.ImageUrl,
            Name = villaDTO.Name,
            Occupancy = villaDTO.Occupancy,
            Rate = villaDTO.Rate,
            Sqft = villaDTO.Sqft
        };

        _db.Villas.Update(model);
        await _db.SaveChangesAsync();

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return NoContent();
    }
}