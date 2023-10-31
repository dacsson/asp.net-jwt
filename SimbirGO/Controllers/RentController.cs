using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirGO.Contexts;
using SimbirGO.DTO.Rent;

namespace SimbirGO.Controllers;

/// <summary>
/// Контроллер для управления арендой транспорта
/// </summary>
[Route("rent")]
[ApiController]
public class RentController : Controller
{
    private readonly SimbirDbContext _dbContext;
    
    public RentController(SimbirDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Считает цену арендуемого транспорта и бронирует транспорт 
    /// </summary>
    /// <param name="id">Идентификатор транспорта</param>
    /// <returns>Результат аренды</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> RentVehicle(long id, int rent_duration)
    {
        var vehToRent = _dbContext.Vehicles.Single(v => v.Id == id);

        if (vehToRent.IsTaken) 
            return Json(new { status = "Error", message = "This vehicle is taken" });

        var rent_cost = rent_duration * vehToRent.RentCost;

        var rentResult = new RentDTO { Duration = rent_duration, NameOfVehicle = vehToRent.Name, RentCost= rent_cost, TypeOfVehicle = vehToRent.TypeOfVehicle };

        //#IF ОПЛАЧЕНО добавить условие оплаты
        vehToRent.IsTaken = true;
        _dbContext.SaveChanges();

        return Ok(rentResult);
    }
}
