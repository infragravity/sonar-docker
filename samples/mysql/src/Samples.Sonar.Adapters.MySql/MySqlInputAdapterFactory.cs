using System.Configuration;
using Microsoft.Extensions.Logging;
using Infragravity.Sonar.Abstractions;
namespace Samples.Sonar.Adapters.MySql
{
    public sealed class MySqlAdapterFactory: IInputAdapterFactory
    {
        public IInputAdapter Create(ConnectionStringSettings connectionSettings, ConfigurationElement serverConfig, ILoggerFactory loggerFactory)
        {
            return new MySqlInputAdapter(connectionSettings);
        }
        public bool SupportsConnectionPooling
        {
            get{return true;}
        }

        public void Dispose() {}
    }
}