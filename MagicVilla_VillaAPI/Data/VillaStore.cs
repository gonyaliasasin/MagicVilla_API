using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data;

public  static class VillaStore
{
    public static List<VillaDTO> villaList = new List<VillaDTO>
    {
        new VillaDTO{Id=1,Name = "Mountain View", Sqft = 400, Occupancy = 4},
        new VillaDTO{Id=2,Name = "Ocean View", Sqft = 300, Occupancy = 3},
        new VillaDTO{Id=3,Name = "Island View", Sqft = 200, Occupancy = 2},
        new VillaDTO{Id=4,Name = "Lake View", Sqft = 100, Occupancy = 1}
    };
}