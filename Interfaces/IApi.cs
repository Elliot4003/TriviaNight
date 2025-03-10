
namespace TriviaNight.Interfaces
{
    public interface IApi
    {
        Uri baseAddress 
        {
            get { return new Uri("https://opentdb.com/"); }
            set { baseAddress = new Uri("https://opentdb.com/"); }
        }

        HttpClient client { 
            get { return new HttpClient(); }
            set { client = new HttpClient(); }
        }
    }
}
