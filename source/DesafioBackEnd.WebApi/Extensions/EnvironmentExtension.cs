namespace DesafioBackEnd.WebApi.Extensions;

public static class EnvironmentExtension
{
    public static bool IsDockerContainer(this IHostEnvironment hostEnvironment)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
        {
            return true;
        }

        return false;
    }
}
