public static class EnumExtensions
    {
        //The Enum constraint works only with C#7.3 or later
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            var type = typeof(TEnum);
            var member = type.GetMember(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.
            GetCustomAttribute(member[0], typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.
            ToString();
        }
    }
    
//---------------------------------------------------------------------------------------------------------------------------------
//Usage:
//---------------------------------------------------------------------------------------------------------------------------------

public enum Quantity
    {
        [Description("Ez van akkor ha nincs semmennyi")]
        None,

        [Description("Ez van akkor ha csak néhány")]
        Few = 5,

        Six,

        [Description("Ez van akkor ha sok")]
        Many = 100,

        [Description("Ez van akkor ha már rengeteg")]
        Lots = 1000
    }
    
    static void Main(string[] args)
        {
            Console.WriteLine(Quantity.Few.GetDescription());
            Console.WriteLine(Quantity.Lots.GetDescription());
            Console.WriteLine(Quantity.Many.GetDescription());
            Console.WriteLine(Quantity.None.GetDescription());
            Console.WriteLine(Quantity.Six.GetDescription());
            Console.ReadKey();
        }
