namespace RentLog.DomainLib11.Configuration
{
    public interface ILauncherParams
    {
        string  DatabasesPath   { get; }
        string  UpdatedExeDir   { get; }
        string  CredentialsKey  { get; }
    }
}
