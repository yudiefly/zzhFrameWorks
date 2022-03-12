using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace OrleansBasics
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger logger;
        public HelloGrain(ILogger<HelloGrain> _logger)
        {
            this.logger = _logger;
        }
        public Task<string> SayHello(string greeting)
        {
            logger.LogInformation($"\n SayHello message received:greeting='{greeting}'");
            return Task.FromResult($"\n Client said:'{greeting}',so HelloGrain says:Hello!");
        }
    }

    /// <summary>
    /// 学生 
    /// </summary>
    public class Student : Orleans.Grain, IStudent
    {
        /// <summary> 学号 </summary>
        private int Id;
        /// <summary> 姓名 </summary>
        private string Name;

        /// <summary>
        /// 打招呼
        /// </summary>
        /// <returns></returns>
        public Task<string> SayHello()
        {
            var id = this.GrainReference.GrainIdentity.PrimaryKeyLong;//当前Grain的key
            Console.WriteLine($"\n {id}收到SayHello消息 \n");
            return Task.FromResult($"\n 大家好，我是{id} \n");
        }
        public Task SetStudentInfo(int studentId, string studentName)
        {
            Id = studentId;
            Name = studentName;
            return Task.CompletedTask;
        }

        public Task ReceiveMessages(string code, object senderId, string message)
        {
            switch (code)
            {
                case "加入新同学":
                    {
                        ConsoleHelper.WriteSuccessLine($"【{Name}】：欢迎新同学");
                        break;
                    }
                case "同学发言":
                    {
                        ConsoleHelper.WriteSuccessLine($"【{Name}】听到了学号为【{senderId}】的同学说的【{message}】");
                        break;
                    }
                default:
                    {
                        ConsoleHelper.WriteSuccessLine($"【{Name}】：我听不懂你们在说啥");
                        break;
                    }
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 教室
    /// </summary>
    public class Classroom : Orleans.Grain, IClassroom
    {
        /// <summary> 教室内的学生 </summary>
        private List<IStudent> Students = new List<IStudent>();

        /// <summary>
        /// 报名登记并拿到学号
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        public async Task<int> Enroll(string name)
        {
            int studentID = Students.Count + 1;
            var aaa = this.GetPrimaryKeyLong();
            IStudent student = GrainFactory.GetGrain<IStudent>(studentID);
            await student.SetStudentInfo(studentID, name);//等待一下
            Students.Add(student);
            return studentID;
        }

        /// <summary>
        /// 学生入座
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<bool> Seated(IStudent student)
        {
            if (!Students.Contains(student))
            {
                return Task.FromResult(false);//没登记的学生不给坐
            }
            foreach (var item in Students)
            {
                if (item.GetPrimaryKeyLong() != student.GetPrimaryKeyLong())
                {
                    item.ReceiveMessages("加入新同学", this.GetPrimaryKeyLong(), $"学号{student.GetPrimaryKeyLong()}的童靴加入了我们，大家欢迎");//不等待
                }
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// 发言
        /// </summary>
        /// <param name="student">当前的学生</param>
        /// <param name="message">发言内容</param>
        public Task<bool> Speech(IStudent student, string message)
        {
            if (!Students.Contains(student))
            {
                return Task.FromResult(false);//没登记的学生闭嘴
            }
            foreach (var item in Students)
            {
                if (item.GetPrimaryKeyLong() != student.GetPrimaryKeyLong())
                {
                    item.ReceiveMessages("同学发言", (int)student.GetPrimaryKeyLong(), message);//不等待
                }
            }
            return Task.FromResult(true);
        }
    }
    /// <summary>
    /// 控制台帮助类
    /// </summary>
    public static class ConsoleHelper
    {

        static void WriteColorLine(string str, ConsoleColor color)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = currentForeColor;
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, color);
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, color);
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string str, ConsoleColor color = ConsoleColor.Green)
        {
            WriteColorLine(str, color);
        }
    }

}
