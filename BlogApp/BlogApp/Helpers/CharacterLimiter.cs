namespace BlogApp.Helpers
{
    public static class CharacterLimiter
    {   
        
            public static string LimitCharacters(string description, int maxLength)
            {
                if (string.IsNullOrEmpty(description) || description.Length <= maxLength)
                {
                    return description;
                }

                var decubetext = description.Substring(0, maxLength);
                if (!string.IsNullOrEmpty(description) && description.Length > maxLength)
                {
                     decubetext += "...";
                }

                return decubetext;
            }
        
    }
}
