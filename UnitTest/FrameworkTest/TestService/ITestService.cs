namespace FrameworkTest.TestService
{
    public interface ITestService
    {
        string SayHello(dynamic words);

        string SayHelloCaching(dynamic words);
    }
}
