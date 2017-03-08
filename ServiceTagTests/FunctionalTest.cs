using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

//This test class is two functional tests to verify that the live data transactions
//work and that both Use Cases are still valid
//Important note: Test Case B requires a Dell Machine to be the target
//TODO: Change this to a known static Dell machine on my network.
namespace ServiceTagTests
{
    public class FunctionalTest
    {
       #region Variables

        #endregion Variables

        #region Setup

        private FunctionalTest()
        {

        }

        #endregion Setup

        #region Private Methods

        #endregion Private Methods

        #region Tests

        //Test Case A

        //User A starts the application

        //User A wants to know the results for the following service tags
        // 'HPB7BQ1', '', ''

        //User A sees the text entry box and enters the service tags one
        //by one

        //User A hits the 'Fetch' button

        //User A notes gets the warranty and model information desired

        //Test Case B

        //User B wants to know the warranty information for thier PC

        //User B starts the application

        //User B enters thier local IP address into the Find box

        //User B hits the 'Find Service Tag' button

        //User B notes that thier service tag has been addded to the list

        //User B hits the 'Fetch' button

        //User B receives their warranty information and model

        #endregion Tests
    }
}
