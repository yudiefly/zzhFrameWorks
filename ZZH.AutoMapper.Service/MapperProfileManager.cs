using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZZH.AutoMapper.Service
{
    /// <summary>
    /// Mapper 映射关系管理器
    /// </summary>
    public class MapperProfileManager
    {
        private readonly MapperConfigurationExpression mapperConfig;

        public MapperProfileManager()
        {
            mapperConfig = new MapperConfigurationExpression();
        }

        #region public methods

        /// <summary>
        /// 创建映射关系
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public MapperProfileManager CreateMap(Assembly assembly)
        {
            CreateMap(assembly.GetTypes());

            return this;
        }

        /// <summary>
        /// 创建映射关系。
        /// </summary>
        /// <param name="types">要创建映射关系的类型</param>
        public MapperProfileManager CreateMap(IEnumerable<Type> types)
        {
            var mapperTypes = types.Where(t => t.IsDefined(typeof(MapperAttribute), false));
            foreach (var type in mapperTypes)
            {
                CreateMap<MapperAttribute>(type);
            }

            return this;
        }

        /// <summary>
        /// 构建 Mapper 对象
        /// </summary>
        public void Buid()
        {
            Mapper.Initialize(mapperConfig);
        }

        #endregion

        #region private methods

        private void CreateMap<TAttribute>(Type type)
           where TAttribute : MapperAttribute
        {
            foreach (var mapper in type.GetCustomAttributes(typeof(TAttribute), false).OfType<MapperAttribute>())
            {
                var mappingExpression = mapperConfig.CreateMap(mapper.SourceType, type);
                IMappingExpression reverMappingExpression = null;
                if (mapper.CanReverse)
                {
                    reverMappingExpression = mappingExpression.ReverseMap();
                }

                var props = type.GetProperties().Where(p => p.IsDefined(typeof(MapMemberAttribute), false));
                foreach (var prop in props)
                {
                    var memberNameAttr = prop.GetCustomAttribute<MapMemberAttribute>(false);
                    mappingExpression.ForMember(prop.Name, conf => conf.MapFrom(memberNameAttr.Member));
                    if (reverMappingExpression != null)
                    {
                        reverMappingExpression.ForMember(memberNameAttr.Member, conf => conf.MapFrom(prop.Name));
                    }
                }
            }
        }

        #endregion
    }
}
