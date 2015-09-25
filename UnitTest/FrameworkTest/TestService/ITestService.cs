using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.TestService
{
    public interface ITestService
    {
        string SayHello(dynamic words);

        string SayHelloCaching(dynamic words);
    }
}
