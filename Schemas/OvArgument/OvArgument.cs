using BPMSoft.Core;
using System;

/// <summary>
/// Содержит методы для проверки входных аргументов.
/// </summary>
public static class Argument
{
    /// <summary>
    /// Проверить, что аргумент-строка не null и не пустая.
    /// В случае неудачной проверки кидает исключение.
    /// </summary>
    /// <param name="value">Аргумент для проверки.</param>
    /// <param name="valueName">Имя аргумента.</param>
    /// <exception cref="ArgumentException"><paramref name="value" /> is <c>null</c> or empty.</exception>
    public static void EnsureNotNullOrEmpty(string value, string valueName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"String '{valueName}' cannot be null or empty.");
        }
    }
}