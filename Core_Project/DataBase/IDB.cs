using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Core_Project.DataBase
{
    public class CF
    {
        [Key]
        public string STUDENT_ID { get; set; }
        public string BATCH_ID { get; set; }
        public IList<CORE_STUDENTS> CORE_STUDENTS { get; set; }
    }

    public class IDB_COURS
    {
        [Key]
        public string SUB_ID { get; set; }
        public string IDB_SUBJECT { get; set; }
        public IList<CORE_STUDENTS> corestudents { get; set; }
    }

    public class CORE_STUDENTS
    {
        [Key]
        public string SL_NO { get; set; }
        [ForeignKey("CF")]
        public string STUDENT_ID { get; set; }
        [ForeignKey("corestudents")]
        public string SUB_ID { get; set; }
        public string IDB_SUBJECT { get; set; }
        public string NAME { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CORE_DATE { get; set; }
        public string PICTURE { get; set; } 
        public CF CF { get; set; }
        public IDB_COURS corestudents { get; set; }

    }
}
