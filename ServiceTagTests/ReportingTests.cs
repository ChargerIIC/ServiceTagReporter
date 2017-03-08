using FluentAssertions;
using NSubstitute;
using DellWarranty;
using System;
using System.Data;
using Xunit;

namespace ServiceTagTests
{
    public class ReportingTests
    {

        #region Variables

        string _validServiceTag;
        string _invalidServiceTag;
        string _badServiceTag;

        #endregion Variables

        #region Setup

        public ReportingTests()
        {
            _validServiceTag = "HPB7BQ1"; //generates functional tests
            _invalidServiceTag = "10101010101013348";
            _badServiceTag = "XXX000F";

            WarrantyEngine.dataTable.Rows.Clear();
        }

        #endregion Setup

        #region Private Methods

        #endregion Private Methods

        #region Tests

        [Fact]
        public void DellSoapProcessor_Setup_IntializesDataSet()
        {
            WarrantyEngine.dataTable.Should().NotBeNull();
        }

        [Fact]
        public void DellSoapProcessor_Given3Tags_Gives3Responses()
        {
            WarrantyEngine.FetchTag(_validServiceTag);
            WarrantyEngine.FetchTag(_validServiceTag);
            WarrantyEngine.FetchTag(_validServiceTag);

            WarrantyEngine.dataTable.Rows.Count.Should().Be(3);
        }

        [Fact]
        public void DellSoapProcessor_Given3BadTags_Gives0Responses()
        {
            WarrantyEngine.FetchTag(_invalidServiceTag);
            WarrantyEngine.FetchTag(_invalidServiceTag);
            WarrantyEngine.FetchTag(_invalidServiceTag);

            WarrantyEngine.dataTable.Rows.Count.Should().Be(0);
        }

        [Fact]
        public void DellSoapProcessor_GivenServiceTag_GeneratesResponse()
        {
            WarrantyEngine.FetchTag(_validServiceTag);
            WarrantyEngine.dataTable.Rows.Count.Should().Be(1);
        }

        [Fact]
        public void DellSoapProcessor_GivenInvalidServiceTag_DoesNotGenerateResponse()
        {
            WarrantyEngine.FetchTag(_invalidServiceTag);
            WarrantyEngine.dataTable.Rows.Count.Should().Be(0);
        }

        [Fact]
        public void DellSoapProcessor_ValidServiceTagResponse_IncludesModel()
        {
            WarrantyEngine.FetchTag(_validServiceTag);
            WarrantyEngine.dataTable.Rows[0].ItemArray[1].Should().Be("Inspiron One 2320");
        }

        [Fact]
        public void DellSoapProcessor_ValidServiceTagReponse_IncludesWarrantyDate()
        {
            WarrantyEngine.FetchTag(_validServiceTag);
            WarrantyEngine.dataTable.Rows[0].ItemArray[3].ShouldBeEquivalentTo(DateTime.Parse("03/26/2013"));
        }

        [Fact]
        public void DellSoapProcessor_ValidServiceTagResponse_IncludesPurchaseDate()
        {
            WarrantyEngine.FetchTag(_validServiceTag);
            WarrantyEngine.dataTable.Rows[0].ItemArray[2].ShouldBeEquivalentTo(DateTime.Parse("03/26/2012"));
        }

        [Fact]
        public void DellSoapProcessor_Fetch_Stripswhitespace()
        {
            var firstTag = "HPB7BQ1\r"; 
            var secondTag = "HPB7BQ1  ";

            WarrantyEngine.FetchTag(firstTag);
            WarrantyEngine.FetchTag(secondTag);
            WarrantyEngine.dataTable.Rows.Count.Should().Be(2);
        }
        #endregion Tests
    }
}
