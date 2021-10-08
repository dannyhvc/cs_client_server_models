using System.ComponentModel.DataAnnotations;

namespace ExercisesDAL
{
    public class SchoolEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timer { get; set; }
    }
}
