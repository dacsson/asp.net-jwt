namespace SimbirGO.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается, если возникли проблемы с доступом пользователя
/// </summary>
/// <typeparam name="T">Код ошибки, который должен содержаться в enum</typeparam>
public class ForbiddenException<T> : Exception where T : Enum
{
    public ForbiddenException(T errorCode) : base(errorCode.ToString())
    {
    }
}
