using System.ComponentModel;

namespace AMFramework_Lib.Structures
{
    /// <summary>
    /// Structure that handles ranges and converts it into a list. AmRanges admits different formatting
    /// -> csv format e.g. 1,2,3,4
    /// -> range format e.g. 1-5:1
    /// -> numeric e.g. 5
    /// </summary>
    public struct Struct_AMRange
    {
        public List<double> Values { get; set; }
        public Struct_AMRange()
        {
            Values = new List<double>();
        }

        /// <summary>
        /// Add numeric value to current range
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public int Add(double newValue)
        {
            Values.Add(newValue);
            return 0;
        }

        /// <summary>
        /// Add value by format (csv or range formats)
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public int Add(string newValue)
        {
            switch (CheckType(newValue))
            {
                case SplitType.NONE:
                    return 1;
                case SplitType.ARRAY_TYPE:
                    return Add_From_csvFormat(newValue);
                case SplitType.RANGE_TYPE:
                    return Add_From_rangeFormat(newValue);
                case SplitType.NUMERICSTRING_TYPE:
                    return Add(double.Parse(newValue));
                default:
                    break;
            }

            return 1;
        }


        #region HelperFunctions
        /// <summary>
        /// Enum for available parsing formats
        /// </summary>
        private enum SplitType
        {
            [Description("Default value")]
            NONE,
            [Description("Array values given by csv format")]
            ARRAY_TYPE,
            [Description("Range defined by format (Start-End:stepsize e.g. 1-5:1 )")]
            RANGE_TYPE,
            [Description("string that can be cast as numeric")]
            NUMERICSTRING_TYPE
        }

        /// <summary>
        /// Checks what type of format is specified
        /// </summary>
        /// <param name="checkThis"></param>
        /// <returns></returns>
        private SplitType CheckType(string checkThis)
        {
            double dummyValue;
            if (checkThis.Contains("-")) return SplitType.RANGE_TYPE;
            else if (checkThis.Contains(",")) return SplitType.ARRAY_TYPE;
            else if (double.TryParse(checkThis, out dummyValue)) return SplitType.NUMERICSTRING_TYPE;

            return SplitType.NONE;
        }
        /// <summary>
        /// Parses csv format type
        /// </summary>
        /// <param name="csvValue"></param>
        /// <returns></returns>
        private int Add_From_csvFormat(string csvValue)
        {
            List<string> splitV = csvValue.Split(",").ToList();

            foreach (var item in splitV)
            {
                double parsedValue;
                if (!double.TryParse(item, out parsedValue)) return 1;

                Values.Add(parsedValue);
            }

            return 0;
        }
        /// <summary>
        /// Parses range format
        /// </summary>
        /// <param name="csvValue"></param>
        /// <returns></returns>
        private int Add_From_rangeFormat(string csvValue)
        {
            List<string> splitV = csvValue.Split("-").ToList();

            if (splitV.Count != 2) return 1;

            double startParsedValue;
            if (!double.TryParse(splitV[0], out startParsedValue)) return 1;

            List<string> splitRange = splitV[1].Split(":").ToList();
            if (splitRange.Count != 2) return 1;

            double endParsedValue;
            if (!double.TryParse(splitRange[0], out endParsedValue)) return 1;

            double stepSizeParsedValue;
            if (!double.TryParse(splitRange[1], out stepSizeParsedValue)) return 1;


            double currentValue = startParsedValue;
            while (currentValue <= endParsedValue)
            {
                Values.Add(currentValue);
                currentValue += stepSizeParsedValue;
            }

            return 0;
        }


        #endregion


    }
}
