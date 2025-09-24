using System;

class Program
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}


class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    private List<Comment> comments = new List<Comment>();

    
    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

        public int GetNumberOfComments()
    {
        return comments.Count;
    }

    
    public void DisplayVideoInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthInSeconds} seconds");
        Console.WriteLine($"Number of Comments: {GetNumberOfComments()}");

        Console.WriteLine("Comments:");
        foreach (Comment comment in comments)
        {
            Console.WriteLine($" - {comment.CommenterName}: {comment.Text}");
        }
        Console.WriteLine(new string('-', 40));
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World! This is the YouTube Video Project.");
        Console.WriteLine();

        
        List<Video> videos = new List<Video>();

       
        Video video1 = new Video { Title = "C# Tutorial Basics", Author = "CodeAcademy", LengthInSeconds = 600 };
        video1.AddComment(new Comment("Alice", "Great explanation!"));
        video1.AddComment(new Comment("Bob", "Very helpful, thanks."));
        video1.AddComment(new Comment("Charlie", "Can you make one about classes?"));
        videos.Add(video1);

       
        Video video2 = new Video { Title = "Learn Encapsulation", Author = "DevTeacher", LengthInSeconds = 720 };
        video2.AddComment(new Comment("David", "This made encapsulation clear."));
        video2.AddComment(new Comment("Eve", "Loved the examples!"));
        video2.AddComment(new Comment("Frank", "Subbed for more content."));
        videos.Add(video2);

       
        Video video3 = new Video { Title = "Intro to Abstraction", Author = "TechGuru", LengthInSeconds = 540 };
        video3.AddComment(new Comment("Grace", "Really easy to understand."));
        video3.AddComment(new Comment("Heidi", "Can you do inheritance next?"));
        video3.AddComment(new Comment("Ivan", "This helped with my assignment."));
        videos.Add(video3);

       
        foreach (Video v in videos)
        {
            v.DisplayVideoInfo();
        }
    }
}
