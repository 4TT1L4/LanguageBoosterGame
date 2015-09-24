using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LanguageBoosterService
{
    [DataContract]
    public class Word
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Question { get; set; }

        [DataMember]
        public string Answer { get; set; }
    }
}