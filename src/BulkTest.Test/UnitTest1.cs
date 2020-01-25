using BulkTest.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BulkTest.Test
{
    public class UnitTest1
    {
        //[Fact]
        //public void Generate_main_view()
        //{
        //    string jsonData = TestKit.ReadFile();
        //    Path path = Utils.Utils.Deserialize<Path>(jsonData);
        //    path.Should().NotBeNull();

        //    List<ManInTheMiddle> lst = new List<ManInTheMiddle>();
        //    API api = path.Paths.Where(x => x.Name == "GetPreCodeList").First();
        //    lst = addSubs(api.Parameters, 0);
        //}

        //private List<ManInTheMiddle> addSubs(List<Parameter> parameters, int intent)
        //{
        //    List<ManInTheMiddle> lst = new List<ManInTheMiddle>();
        //    foreach (Parameter p in parameters)
        //    {
        //        if (p.SubTypeParameters?.Count > 0)
        //        {
        //            lst.Add(new ManInTheMiddle(p, intent));
        //            lst.AddRange(addSubs(p.SubTypeParameters, intent + 1));
        //        }
        //        else
        //            lst.Add(new ManInTheMiddle(p, intent));
        //    }

        //    return lst;
        //}
    }
}
