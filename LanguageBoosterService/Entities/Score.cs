using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LanguageBoosterService
{
    [DataContract]
    public class Score
    {
        [DataMember]
        public int Id{ get; set; }

        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public string PlayerName { get; set; }
    }
}