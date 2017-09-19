using System;

namespace Anbo1DatabaseConsoleClient
{
    /// <summary>
    /// DTO aka. model class
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }

        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return Id + " " + Name + " " + Semester + " " + TimeStamp;
        }
    }
}
