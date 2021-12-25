using Microsoft.Extensions.Configuration;

namespace UserControl.Api
{
    public class Settings
    {
        public Settings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string GetKet()
        {
            return Configuration.GetConnectionString("KeyToken");
        }
    }
}