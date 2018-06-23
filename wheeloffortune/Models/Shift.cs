using System;
using System.ComponentModel.DataAnnotations;

namespace wheeloffortune.Models
{
    public class Shift
    {
        public string EngineerName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ShiftDate { get; set; }
        public int ShiftNumber { get; set; }
    }
}