namespace ASP_Core.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

namespace MyFirstWebApp.Models
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}