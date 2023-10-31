using SimbirGO.Models;

namespace SimbirGO.DTO.Rent;

public class RentDTO
{
    /// <summary>
    /// Идентификатор договора аренды
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Название арендуемого транспорта
    /// </summary>
    public string NameOfVehicle { get; set; }

    /// <summary>
    /// Цена за время аренды
    /// </summary>
    public int RentCost { get; set; }

    /// <summary>
    /// Насколько дней арендовано
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Тип транспорта
    /// </summary>
    public Models.Vehicle.Type TypeOfVehicle { get; set; }
}

