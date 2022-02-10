using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofisprojesi
{
    public enum DbActionResult
    {
        Successful = 0,
        UnknownError = 1,
        AgeError = 2,
        OfficeNotFound = 3,
        AlreadyExists = 4,
        HaveDebitError = 5,
        NameOrLastNameError = 6,
        NotFound = 7,
        NotHaveFixture = 8,
        NotHaveEmployee = 9,
        EmployeeFalse = 10,
        FixtureFalse = 11,
       
    }
    public enum debitservicesenum{
        DebitActive=1,
        DebitAll=2,
        DebitPasive=3,
    }
    public enum fixtureservicesenum{
        FixtureActive=1,
        FixtureAll=2,
        FixturePasive=3,
    }
}