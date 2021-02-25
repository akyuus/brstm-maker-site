using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brstm_maker_site.Models
{
    public class CourseInfo
    {
        public static readonly Dictionary<string, string> courseMappings = new Dictionary<string, string>()
        {
            {"finish-first", "o_FanfareGP1_32"},
            {"finish-ok", "o_FanfareGP2_32"},
            {"finish-bad", "o_FanfareGPdame_32"},
            {"race-intro-wifi", "o_Crs_In_Fan_Wifi"},
            {"lc", "n_circuit32"},
            {"mmm", "n_farm"},
            {"mg", "n_kinoko"},
            {"tf", "STRM_N_FACTORY"},
            {"mc", "n_circuit32"},
            {"cm", "n_shopping32"},
            {"dks", "n_snowboard32"},
            {"wgm", "STRM_N_TRUCK"},
            {"dc", "n_daisy32"},
            {"kc", "STRM_N_WATER"},
            {"mt", "n_maple"},
            {"gv", "n_volcano32"},
            {"ddr", "STRM_N_DESERT"},
            {"mh", "STRM_N_RIDGEHIGHWAY"},
            {"bcwii", "STRM_N_KOOPA"},
            {"rr", "n_rainbow32"},
            {"rpb", "r_gc_beach32"},
            {"ryf", "r_ds_jungle32"},
            {"gv2", "r_sfc_obake32"},
            {"rmr", "r_64_circuit32"},
            {"rsl", "r_64_sherbet32"},
            {"sgb", "r_agb_beach32"},
            {"rds", "r_ds_town32"},
            {"rws", "r_gc_stadium32"},
            {"rdh", "r_ds_desert32"},
            {"bc3", "r_agb_kuppa32"},
            {"dkjp", "r_64_jungle32"},
            {"rmc", "r_gc_circuit32"},
            {"mc3", "r_sfc_circuit32"},
            {"rpg", "r_ds_garden32"},
            {"dkm", "r_gc_mountain32"},
            {"rbc", "r_64_kuppa32"}
        };

        public static readonly Dictionary<string, int> multiChannelCourses = new Dictionary<string, int>()
        {
            {"tf", 4},
            {"wgm", 4},
            {"kc", 8},
            {"ddr", 4},
            {"mh", 4},
            {"bcwii", 8} //technically only needs to be 6, but 8 is safer
        };

        public bool allCaps { get; set; }
        public bool isCourse { get; set; }
        public int channelCount { get; set; }
        public string fileName { get; set; }

        public CourseInfo(string courseAbbreviation)
        {
            fileName = courseMappings[courseAbbreviation];
            channelCount = multiChannelCourses.ContainsKey(courseAbbreviation) ? multiChannelCourses[courseAbbreviation] : 2;
            allCaps = channelCount > 2 ? true : false;
            isCourse = courseAbbreviation.Contains('-') ? false : true;
        }

    }
}
