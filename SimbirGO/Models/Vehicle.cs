namespace SimbirGO.Models;

public class Vehicle
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
    public bool IsTaken { get; set; }

    /// <summary>
    /// Возможные типы транспорта
    /// </summary>
    public enum Type { Motorcycle, Bicycle, Car, Scooter}

    /// <summary>
    /// Тип транспорта
    /// </summary>
    public Type TypeOfVehicle { get; set; }
}
