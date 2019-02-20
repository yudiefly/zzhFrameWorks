using System;

namespace ExampleModel
{
    /// <summary>
    /// 医生信息
    /// </summary>
    [Serializable]
    public class DoctorInfoMQ
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Introduce { get; set; }
        public string Level { get; set; }
        public string PhoneNumber { get; set; }
        public string ServiceDeptName { get; set; }
        public string Speciality { get; set; }
        public string HeadImgUrl { get; set; }
    }
}
