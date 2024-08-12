namespace Infinity_Scrolling_MAUI.Model
{
    public class BlogPost
    {
        public BlogPost(int id, string title, string authorName, DateTime publicationDate, string imagePath, string url)
        {
            Id = id;
            Title = title;
            AuthorName = authorName;
            PublicationDate = publicationDate;
            ImagePath = imagePath;
            Url = url;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public string AvatarPath => AuthorName?.ToLower() + ".jpg";
    }
}
