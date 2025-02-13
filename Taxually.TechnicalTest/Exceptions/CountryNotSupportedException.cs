namespace Taxually.TechnicalTest.Exceptions;

public class CountryNotSupportedException(string key) : Exception($"Country with the key: \"{key}\" not found")
{
}
