namespace TweetClassifer.Shared.Models
{
    public class TweetVM
    { 
        public string Text { get; set; }
        public string URL { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
