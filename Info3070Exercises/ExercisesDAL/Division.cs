using System.Collections.Generic;

#nullable disable

namespace ExercisesDAL
{
    public partial class Division : SchoolEntity
    {
        public Division()
        {
            Students = new HashSet<Student>();
        }

        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
