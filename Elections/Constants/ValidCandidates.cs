using System.Collections.Generic;
using System.Linq;

namespace Elections.Constants
{
    public static class ValidCandidates
    {
        private static string JFK = "John F. Kennedy";
        private static string DT = "Donald Trump";
        private static string HC = "Hillary Clinton";
        private static string JB = "Joe Biden";
        private static string JR = "Jack Randall";

        public static List<string> allCandidates { get; set; } = new List<string>()
        {
            JFK, DT, HC, JB, JR
        };
        public static string GetFullName(string code)
        {
            if (code == "JFK")
                return JFK;
            else if (code == "DT")
                return DT;
            else if (code == "HC")
                return HC;
            else if (code == "JB")
                return JB;
            else if (code == "JR")
                return JR;
            return null;
        }
        public static string GetCodeByFullName(string fullName)
        {
            if (fullName == JFK)
                return "JFK";
            else if (fullName == DT)
                return "DT";
            else if (fullName == HC)
                return "HC";
            else if (fullName == JB)
                return "JB";
            else if (fullName == JR)
                return "JR";
            return null;
        }
    }
}
