using Recipe_Book.Controllers;

namespace Recipe_Book
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Controller ctr = new Controller();
            ctr.Start();
        }
    }
}
