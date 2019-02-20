using System;
using System.Collections.Generic;
using System.Text;
using ZZH.AutoMapper.Service.Utils;

namespace ZZH.AutoMapper.Service
{
    public class MapperBuilder
    {
        private readonly MapperOptions _options = new MapperOptions();

        public MapperBuilder Use(Action<MapperOptions> optionAction)
        {
            if (optionAction == null)
            {
                throw new ArgumentNullException(nameof(optionAction));
            }

            optionAction(_options);

            return this;
        }

        public MapperBuilder RegisterAssemblies()
        {
            _options.AddRegisterAssemblies(AssemblyUtil.GetAssemblies().ToArray());
            return this;
        }

        public void Build()
        {
            if (_options.AssembliesToRegister.Count > 0)
            {
                var manager = new MapperProfileManager();
                foreach (var assembly in _options.AssembliesToRegister)
                {
                    manager.CreateMap(assembly);
                }
                manager.Buid();
            }
        }
    }
}
