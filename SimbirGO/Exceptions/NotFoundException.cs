namespace SimbirGO.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается, если не был найден ресурс
/// </summary>
/// <typeparam name="T">Код ошибки, который должен содержаться в enum</typeparam>
public class NotFoundException<T> : Exception where T : Enum
{
    public NotFoundException(T errorCode) : base(errorCode.ToString())
    {
    }
}
