using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Spark.Parser.Markup;
using SparkSense.Parsing;
using System;

namespace SparkSense.Tests.Parsing
{
    [TestFixture]
    public class ParseContextNodeTests
    {
        [Test]
        public void ShouldReturnAttributeNodeGivenPositionAtStartOfAttribute()
        {
            var nodeType = SparkSyntax.ParseContext("<div><use content=\"main\"/></div>", position: 10);

            Assert.That(nodeType, Is.EqualTo(typeof(AttributeNode)));
        }

        [Test]
        public void ShouldReturnElementNodeGivenPositionAtStartOfNewlyOpenedElement()
        {
            Type nodeType = SparkSyntax.ParseContext("<div><</div>", position: 6);

            Assert.That(nodeType, Is.EqualTo(typeof(ElementNode)));
        }

        [Test]
        public void ShouldReturnElementNodeGivenPositionInNameOfElement()
        {
            Type nodeType = SparkSyntax.ParseContext("<div><us</div>", position: 8);

            Assert.That(nodeType, Is.EqualTo(typeof(ElementNode)));
        }

        [Test]
        public void ShouldReturnExpressionNodeGivenPositionAfterDollarOpeningBrace()
        {
            Type nodeType = SparkSyntax.ParseContext("${", position: 2);

            Assert.That(nodeType, Is.EqualTo(typeof(ExpressionNode)));
        }

        [Test]
        public void ShouldReturnExpressionNodeGivenPositionAfterExclamationOpeningBrace()
        {
            Type nodeType = SparkSyntax.ParseContext("!{", position: 2);

            Assert.That(nodeType, Is.EqualTo(typeof(ExpressionNode)));
        }

        [Test]
        public void ShouldReturnTextNodeGivenPositionAfterOpeningBrace()
        {
            Type nodeType = SparkSyntax.ParseContext("{", position: 1);

            Assert.That(nodeType, Is.EqualTo(typeof(TextNode)));
        }
    }
}
