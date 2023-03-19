using System.ComponentModel.DataAnnotations;
using System;

namespace Core_Project.Models
{
    public class IDB_STUDENTS_VM
    {
        public string SL_NO { get; set; }
        public string STUDENT_ID { get; set; }
        [Required(ErrorMessage = "Please enter NAME")]
        public string NAME { get; set; }
        public string BATCH_ID { get; set; }
        public string SUB_ID { get; set; }
        public string IDB_SUBJECT { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CORE_DATE { get; set; }
        public string PICTURE { get; set; }
    }
}
