using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbirGO.Contexts;
using SimbirGO.DTO.Auth;
using SimbirGO.DTO.Vehicle;
using SimbirGO.Models;
using SimbirGO.Services;

namespace SimbirGO.Controllers;

/// <summary>
/// Контроллер для управление транспортом
/// </summary>
[Route("api/vehicles")]
[ApiController]
public class VehicleController : Controller
{
    private readonly SimbirDbContext _dbContext;

    /// <summary>
    /// Создание экземпляра класса <see cref="UserController"/>
    /// </summary>
    /// <param name="authorizationService">Экземляр сервиса авторизации</param>
    /// <param name="optilogsDbContext">Экземпляр контекста базы данных Optilogs</param>
    /// <param name="mapper">Экземпляр маппера</param>
    public VehicleController(SimbirDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Возвращает все транспортные средства
    /// </summary>
    /// <returns>Список транспорта</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetVehicles()
    {
        var users = await _dbContext.Vehicles.ToListAsync();
        return Ok(users.Adapt<IList<VehicleDTO>>());
    }

    /// <summary>
    /// Добавляет в систему новый транспорт
    /// </summary>
    /// <param name="vehicleDTO"></param>
    /// <returns>Результат создания транспорта</returns>
    [HttpPost("/create")]
    [Authorize(Roles = "Root")]
    public async Task<ActionResult> CreateVehicle(VehicleDTO vehicleDTO)
    {
        var vehToAdd = vehicleDTO.Adapt<Vehicle>();
        _dbContext.Vehicles.Add(vehToAdd);
        _dbContext.SaveChanges();
        return Ok();
    }

    /// <summary>
    /// Удаляет транспорт из системы
    /// </summary>
    /// <param name="id">Идентификатор транспорта</param>
    /// <returns>Результат удаления транспорта</returns>
    [HttpPost("/delete")]
    [Authorize(Roles = "Root")]
    public async Task<ActionResult> DeleteVehicle(long id)
    {
        var vehToDel = _dbContext.Vehicles.Single(v => v.Id == id);
        _dbContext.Vehicles.Remove(vehToDel);
        _dbContext.SaveChanges();
        return Ok();
    }

    /// <summary>
    /// Возвращает конрктеный транспорт
    /// </summary>
    /// <param name="id">Идентификатор транспорта</param>
    /// <returns>Найденный транспорт</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult> GetVehicle(long id)
    {
        var veh = _dbContext.Vehicles.Single(x => x.Id == id);
        return Ok(veh.Adapt<VehicleDTO>());
    }
}
