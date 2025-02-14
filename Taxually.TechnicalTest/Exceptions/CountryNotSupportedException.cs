namespace Taxually.TechnicalTest.Exceptions;

public class CountryNotSupportedException(string key) : Exception($"Country with code \"{key}\" not supported")
{
}
