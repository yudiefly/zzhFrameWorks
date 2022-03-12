using System.Threading.Tasks;

namespace OrleansBasics
{
    public interface IHello:Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }

    /// <summary>
    /// 学生
    /// </summary>
    public interface IStudent : Orleans.IGrainWithIntegerKey
    {
        /// <summary>
        /// 打招呼
        /// </summary>
        /// <returns></returns>
        Task<string> SayHello();
        /// <summary>
        /// 设置个人信息
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="studentName">姓名</param>
        /// <returns></returns>
        Task SetStudentInfo(int studentId, string studentName);

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="code">消息code类型</param>
        /// <param name="senderId">消息发送人id</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        Task ReceiveMessages(string code, object senderId, string message);
    }

    /// <summary>
    /// 教室
    /// </summary>
    public interface IClassroom : Orleans.IGrainWithIntegerKey
    {
        /// <summary>
        /// 报名登记并拿到学号
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        Task<int> Enroll(string name);

        /// <summary>
        /// 学生入座
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Task<bool> Seated(IStudent student);

        /// <summary>
        /// 发言
        /// </summary>
        /// <param name="student">当前的学生</param>
        /// <param name="message">发言内容</param>
        /// <returns></returns>
        Task<bool> Speech(IStudent student, string message);
    }
}
