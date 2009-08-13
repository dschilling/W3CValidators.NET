// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Test
{
    using System;
    using System.Text;
    using Markup;
    using NUnit.Framework;

    [TestFixture]
    public class MarkupValidatorResponseTest
    {
        private MarkupValidatorResponse _response;

        [SetUp]
        public void SetUp()
        {
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("W3CValidators.Test.response.xml"))
            {
                _response = new MarkupValidatorResponse(stream);
            }
        }

        [Test]
        public void Uri_Should_Return_Correct_Result()
        {
            Assert.That(_response.Uri, Is.EqualTo(new Uri("http://raw-data.org/sites/default/files/invalidXhtml11.html")));
        }

        [Test]
        public void CheckedBy_Should_Return_Correct_Result()
        {
            Assert.That(_response.CheckedBy, Is.EqualTo(new Uri("http://validator.w3.org/")));
        }

        [Test]
        public void DocType_Should_Return_Correct_Result()
        {
            Assert.That(_response.DocType, Is.EqualTo("-//W3C//DTD XHTML 1.1//EN"));
        }

        [Test]
        public void Charset_Should_Return_Correct_Result()
        {
            Assert.That(_response.Charset, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void Validity_Should_Return_Correct_Result()
        {
            Assert.That(_response.Validity, Is.EqualTo(false));
        }

        [Test]
        public void Error_Count_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors.Count, Is.EqualTo(1));
        }

        [Test]
        public void Error_Line_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors[0].Line, Is.EqualTo(8));
        }

        [Test]
        public void Error_Col_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors[0].Col, Is.EqualTo(5));
        }

        [Test]
        public void Error_Message_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors[0].Message, Is.EqualTo("character data is not allowed here"));
        }

        [Test]
        public void Error_MessageId_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors[0].MessageId, Is.EqualTo("63"));
        }

        [Test]
        public void Error_Explanation_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors[0].Explanation, Is.StringStarting("\n                      <p class=\"helpwanted\">\n      <a\n"));
        }

        [Test]
        public void Error_Source_Should_Return_Correct_Result()
        {
            Assert.That(_response.Errors[0].Source, Is.EqualTo("    <strong title=\"Position where error was detected.\">T</strong>his file does not conform to XHTML 1.1."));
        }

        [Test]
        public void Warning_Count_Should_Return_Correct_Result()
        {
            Assert.That(_response.Warnings.Count, Is.EqualTo(0));
        }

        [Test]
        public void Debug_Should_Contain_The_Correct_Data()
        {
            Assert.That(_response.Debug["Parser"], Is.EqualTo("SGML::Parser::OpenSP"));
        }
    }
}