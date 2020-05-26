using BL;
using BL.Serializer.Factory;
using Model;
using Model.ModelData;
using Ninject.Modules;
using System.Collections.Generic;

namespace Jotter
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISerializersFactory<UserCredentials>>().To<SerializerFactory<UserCredentials>>();
            Bind<ISerializersFactory<IEnumerable<Note>>>().To<SerializerFactory<IEnumerable<Note>>>();
            Bind<ISerializersFactory<IEnumerable<Category>>>().To<SerializerFactory<IEnumerable<Category>>>();

            Bind<IStorage>().To<Storage>().InSingletonScope();
        }
    }
}
