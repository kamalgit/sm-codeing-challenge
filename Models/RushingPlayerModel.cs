﻿using System.Runtime.Serialization;

namespace sm_coding_challenge.Models
{
    [DataContract]
    public class RushingPlayerModel : PlayerModel
    {
        [DataMember(Name = "yds")]
        public string Yds { get; set; }

        [DataMember(Name = "att")]
        public string Att { get; set; }

        [DataMember(Name = "tds")]
        public string Tds { get; set; }

        [DataMember(Name = "fum")]
        public string Fum { get; set; }
    }
}
