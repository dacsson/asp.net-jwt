using SimbirGO.Models;

namespace SimbirGO.DTO.Vehicle;

public class VehicleDTO
{
    /// <summary>
    /// Идентификатор транспорта
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Название транспорта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Цена за день аренды
    /// </summary>
    public int RentCost { get; set; }

    /// <summary>
    /// Свободен ли транспорт
    /// </summary>
    public bool isTaken { get; set; }

    /// <summary>
    /// Тип транспорта
    /// </summary>
    public Models.Vehicle.Type TypeOfVehicle { get; set; }
}

