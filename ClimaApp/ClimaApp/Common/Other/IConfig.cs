using SQLite.Net.Interop;

namespace ClimaApp
{
    public interface IConfig
    {
        string diretorioSQLite { get; }
        ISQLitePlatform plataforma { get; }
    }
}
