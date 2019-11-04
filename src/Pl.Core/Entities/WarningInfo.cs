namespace Pl.Core.Entities
{
    public class WarningInfo
    {
        public WarningLevel Level { get; set; }

        public string Message { get; set; }

        public string LinkToFix { get; set; }
    }

    public enum WarningLevel
    {
        Pass,
        Warning,
        Fail
    }
}