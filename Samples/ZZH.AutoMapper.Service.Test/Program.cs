using System;
using System.Collections.Generic;
using System.Linq;

namespace ZZH.AutoMapper.Service.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var automaptest = new AutoMapperTests();
            automaptest.Should_MapToDestination_List_Test();
            automaptest.Should_MapToDestination_Test();
            Console.ReadLine();
        }
    }
    /// <summary>
    /// 测试类
    /// </summary>
    public class AutoMapperTests
    {
        public AutoMapperTests()
        {
            new MapperBuilder().RegisterAssemblies().Build();
        }

        public void Should_MapToDestination_Test()
        {
            var source = new SourceEntity
            {
                Id = 2,
                Name = "s-t",
                InBound = DateTime.Now,
                IsDeleted = false,
            };

            var dest = source.MapTo<DestinationEntity>();
            if (dest == null)
            {
                Console.WriteLine("转换失败！源数据：{0}", JsonHelper.SerializeObject(source));
            }
            else
            {
                Console.WriteLine("源数据：{0}；转换后的数据：{1}", JsonHelper.SerializeObject(source), JsonHelper.SerializeObject(dest));
            }
        }
        
        public void Should_MapToDestination_List_Test()
        {
            var sources = new List<SourceEntity>
            {
                new SourceEntity
                {
                    Id = 1,
                    Name = "s-t-1",
                    InBound = DateTime.Now,
                    IsDeleted = false,
                },
                new SourceEntity
                {
                    Id = 2,
                    Name = "s-t-2",
                    InBound = DateTime.Now,
                    IsDeleted = false,
                },
                new SourceEntity
                {
                    Id = 3,
                    Name = "s-t-3",
                    InBound = DateTime.Now,
                    IsDeleted = true,
                }
            };

            var dests = sources.MapTo<DestinationEntity>();
            if (dests == null)
            {
                Console.WriteLine("转换失败！源数据：{0}", JsonHelper.SerializeObject(sources));

            }
            else
            {
                Console.WriteLine("源数据：{0}；转换后的数据：{1}", JsonHelper.SerializeObject(sources), JsonHelper.SerializeObject(dests));
            }

            var source = sources.Last();
            var dest = dests.Last();

            Console.WriteLine("多个转换后的结果：{0};【{1}】", JsonHelper.SerializeObject(source), JsonHelper.SerializeObject(dest));

        }
    }
}
