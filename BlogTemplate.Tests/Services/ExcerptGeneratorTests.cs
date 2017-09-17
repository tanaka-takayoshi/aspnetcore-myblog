using My_Blog.Models;
using My_Blog.Services;
using My_Blog.Tests.Fakes;
using Xunit;

namespace My_Blog.Tests.Services
{
    public class ExcerptGeneratorTests
    {
        [Fact]
        public void CreateExcerpt_BodyLengthExceedsMaxLength_ExcerptIsTruncated()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator(5);
            string testExcerpt = testExcerptGenerator.CreateExcerpt("This is the body");
            Assert.Equal("This ...", testExcerpt);
        }
        [Fact]
        public void CreateExcerpt_BodyLengthDoesNotExceedMaxLength_ExcerptEqualsBody()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator(50);
            string testExcerpt = testExcerptGenerator.CreateExcerpt("This is the body");
            Assert.Equal("This is the body", testExcerpt);
        }
    }
}
