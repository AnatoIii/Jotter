using BL;
using BL.Helpers;
using BL.Serializer.Factory;
using Model;
using Model.ModelData;
using Ninject.Modules;
using System.Collections.Generic;
using System.Configuration;

namespace Jotter
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISerializersFactory<UserCredentials>>().To<SerializerFactory<UserCredentials>>();
            Bind<ISerializersFactory<IEnumerable<Note>>>().To<SerializerFactory<IEnumerable<Note>>>();
            Bind<ISerializersFactory<IEnumerable<Category>>>().To<SerializerFactory<IEnumerable<Category>>>();

            var serverUrl = ConfigurationManager.AppSettings["serverUrl"];
            Bind<JotterHttpClient>().To<JotterHttpClient>().WithConstructorArgument("serverUrl", serverUrl);

            var defaultFilePath = ConfigurationManager.AppSettings["defaultFilePath"];
            Bind<IStorage>().To<Storage>().InSingletonScope().WithConstructorArgument("defaultFilePath", defaultFilePath);
        }
    }
}
